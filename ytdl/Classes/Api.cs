using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using ytdl.Models;

namespace ytdl.Classes
{
	static class Api
	{
		public static ObservableCollection<DownloadedItems> clist = new ObservableCollection<DownloadedItems>();
		private static Boolean CheckIsConnect()
		{
			if (App.Today == 0)
			{
				CloseHelp.ShowMSG("Error in connecting to server! Please restart the application");
				return false;
			}
			return true;
		}
		private static Boolean CheckCharge()
		{
			if (App.Usr.nrCanDownload <= 0)
				return false;
			return CheckIsConnect();
		}
		private static string Token
		{
			get
			{
				var inside = (App.Usr.Id + "|" + App.Today);
				string token = CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(inside)));
				return token;
			}
		}
		public static async Task<Boolean> MakeMyDayAsync()
		{
			try
			{
				string account = LocalSettingManager.ReadSetting("Account");
				App.Usr = JsonConvert.DeserializeObject<User>(account);
				string url = "https://shahriar.in/app/ydm/dl/getdate.php?i=" +
					CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(App.Usr.Id.ToString())));
				url = await CloseHelp.DownloadPages(new CancellationToken(), url);
				url = CloseHelp.MultiBase64Decode(url, 6);
				if (url.Substring(0, 3).Equals("Err")) throw new Exception();
				var arr = url.Split('|');
				App.Today = Convert.ToInt32(arr[0]);
				App.Usr.nrCanDownload = Convert.ToInt32(arr[1]);
				if (App.Usr.nrCanDownload < 0) App.Usr.nrCanDownload = 0;
				Views.MotherPanel.StaticNr.Label = App.Usr.nrCanDownload.ToString();
				return true;
			}
			catch { return false; }
		}
		public async static Task<string> GetVideo(string input,bool AutoLoadingGUI=true,bool ShowMsg=true)
		{
			List<DownloadedItems> myList = new List<DownloadedItems>(clist);
			var tempo = input;
			if (input.Contains("?v=")) tempo=input.Substring(input.IndexOf("?v=")+3);
			var x = myList.Find(obj => obj.Id == tempo);
			if (x!=null)
			{
				if (ShowMsg) CloseHelp.ShowMSG("این ویدیو در لیست موجود است");
				return null;
			}
			if (!CheckCharge())
			{
				if (ShowMsg) CloseHelp.ShowMSG("اکانت خود را شارژ کنید");
				return null;
			}
			string key = null;
			try
			{
				if(AutoLoadingGUI) Views.MotherPanel.StaticRing.IsLoading = true;
				string video = CloseHelp.Base64Encode(input);
				string url = "https://shahriar.in/app/ydm/dl/getvideo.php?u=" + Token + "&i=" + video;
				string respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
				if (respond.Substring(0, 3) == "Err")
				{
					Views.MotherPanel.StaticRing.IsLoading = false;
					CloseHelp.ShowMSG(respond);
					return null;
				}
				var temp = respond.Split(new string[] { "[*]" }, StringSplitOptions.RemoveEmptyEntries);

				var DLI = JsonConvert.DeserializeObject<DownloadedItems>(temp[0]);
				clist.Insert(0, DLI);
				DLI.Duration = ConvertDuration(DLI.Duration);
				key = "LI" + DLI.Id;
				await AkavacheHelper.SaveStringLocal("MainList", JsonConvert.SerializeObject(clist));
				var LI = JsonConvert.DeserializeObject<LinkItems[]>(temp[1]);
				foreach (var item in LI)
					item.str = item.type + " | " + item.quality;
				await AkavacheHelper.SaveStringLocal(key, JsonConvert.SerializeObject(LI));
				Views.MotherPanel.StaticNr.Label = App.Usr.nrCanDownload.ToString();
				App.Usr.nrCanDownload--;
			}
			catch
			{
				if(ShowMsg)CloseHelp.ShowMSG("Problem in getting your video! Please try later");
			}
			if (AutoLoadingGUI) Views.MotherPanel.StaticRing.IsLoading = false;
			return key;
		}
		public static async Task FillSizeAsync(string key)
		{
			var ls = await AkavacheHelper.ReadStringLocal(key);
			var LI = JsonConvert.DeserializeObject<LinkItems[]>(ls);
			foreach (var item in LI)
			{
				item.str = item.type + " | " + item.quality;
				try
				{
					item.size = await GetSize(item.url);
				}
				catch { item.size = "!"; }
				item.str += " | " + item.size;
				item.url = null;
			}
			await AkavacheHelper.SaveStringLocal(key, JsonConvert.SerializeObject(LI));
		}
		public static string GetVideoLink(string id, string quality)
		{
			string videoId = CloseHelp.Base64Encode(id);
			string url = "https://shahriar.in/app/ydm/dl/getvideo.php?u=" + Token + "&i=" + videoId + "&format=" + quality;
			return url;
		}
		public static async void GetAllVideoLinkAsync()
		{
			var links = new List<string>();
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(await AkavacheHelper.ReadStringLocal("MainList"));
			foreach (var dl in ser)
			{
				var save = await AkavacheHelper.ReadStringLocal("LI" + dl.Id);
				try
				{
					var ls = JsonConvert.DeserializeObject<LinkItems[]>(save);
					string url = GetVideoLink(dl.Id, ls[0].quality);
					links.Add(url);
				}
				catch { }
			}
			var linksString= string.Join(Environment.NewLine, links.ToArray());
			var dataPackage = new DataPackage();
			dataPackage.SetText(linksString);
			Clipboard.SetContent(dataPackage);
		}
		public async static Task<List<DownloadedItems>> SearchVideo(string input, string maxRes)
		{
			var str = CloseHelp.Reverse(CloseHelp.Base64Encode(input));
			string url = "https://shahriar.in/app/ydm/search/search.php?q=" + str + "&maxResults=" + CloseHelp.Base64Encode(maxRes);
			string respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
			if (respond.Substring(0, 3) == "Err")
			{
				return null;
			}
			return JsonConvert.DeserializeObject<List<DownloadedItems>>(respond);
		}
		public async static Task<List<DownloadedItems>> GetPlayList(string str)
		{
			if (str.Contains("list="))
				str = str.Substring(str.IndexOf("list=") + 5);
			str = CloseHelp.Reverse(CloseHelp.Base64Encode(str));
			string url = "https://shahriar.in/app/ydm/search/playList.php?q=" + str;
			string respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
			if (respond.Substring(0, 3) == "Err")
			{
				return null;
			}
			return JsonConvert.DeserializeObject<List<DownloadedItems>>(respond);
		}
		public static async Task<string> GetSize(string url)
		{
			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.Method = "HEAD";
				req.ContinueTimeout = 1000;
				var resp = await req.GetResponseAsync();
				float len = (float)resp.ContentLength / (1024 * 1024);
				return len.ToString("0.00") + " MB";
			}
			catch { return "!"; }
		}
		private static string ConvertDuration(string drr)
		{
			try
			{
				int dr = Convert.ToInt32(drr);
				return string.Format("{0}:{1}", dr / 60, dr % 60);
			}
			catch { return "!"; }
		}

		public static async void GetBatchOfVideos(List<string> urls)
		{
			Views.MotherPanel.StaticRing.IsLoading = true;
			foreach (var item in urls)
			{
				try
				{
					var key = await GetVideo(item, false, false);
					if (key == null) continue;
					await FillSizeAsync(key);
				}
				catch { }
			}
			Views.MotherPanel.StaticRing.IsLoading = false;
		}
	}
}
