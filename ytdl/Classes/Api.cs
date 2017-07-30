using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
			{
				//TODO broken
				CloseHelp.ShowMSG("Charge your account");
				return false;
			}
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
				string url = "https://shahriar.in/app/ydm/dl/getdate.php?i=" + CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(App.Usr.Id.ToString())));
				url = await CloseHelp.DownloadPages(new CancellationToken(), url);
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
		public async static void GetVideo(string input)
		{
			if (!CheckCharge()) return;
			Views.MotherPanel.StaticRing.IsLoading = true;
			string video = CloseHelp.Base64Encode(input);
			string url = "https://shahriar.in/app/ydm/dl/getvideo.php?u=" + Token + "&i=" + video;
			string respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
			if (respond.Substring(0, 3) == "Err") { Views.MotherPanel.StaticRing.IsLoading = false; CloseHelp.ShowMSG(respond); return; }
			var temp = respond.Split(new string[] { "[*]" }, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				//TODO: DB
				var DLI = JsonConvert.DeserializeObject<DownloadedItems>(temp[0]);
				var LI = JsonConvert.DeserializeObject<LinkItems[]>(temp[1]);
				clist.Insert(0, DLI);
				DLI.Duration = ConvertDuration(DLI.Duration);
				foreach (var item in LI)
				{
					try
					{
						item.size = await GetSize(item.url);
					}
					catch { }
					item.str = item.quality + " | " + item.type + " | " + item.size;
					//item.type.Replace("video/", "")
					item.url= null;
				}
				LocalSettingManager.SaveSetting("DI", JsonConvert.SerializeObject(clist));
				LocalSettingManager.SaveSetting("LI" + DLI.Id, JsonConvert.SerializeObject(LI));
				App.Usr.nrCanDownload--;
				Views.MotherPanel.StaticNr.Label= App.Usr.nrCanDownload.ToString();
			}
			catch
			{
				CloseHelp.ShowMSG("Problem in getting your video! Please try later");
			}
			Views.MotherPanel.StaticRing.IsLoading = false;
		}
		public static string GetVideoLink(string id, string quality)
		{
			string u = CloseHelp.Base64Encode(CloseHelp.Base64Encode(CloseHelp.Reverse((App.Today + App.Usr.Id).ToString())));
			string videoId = CloseHelp.Base64Encode(id);
			var f = CloseHelp.Base64Encode(quality);
			string url = "https://shahriar.in/app/ydm/dl/getvideo.php?u=" + Token + "&i=" + videoId + "&f=" + f;
			return url;
		}
		public static string GetAllVideoLink()
		{
			//Todo DB
			var links = new List<string>();
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(LocalSettingManager.ReadSetting("DI"));
			foreach (var dl in ser)
			{
				var save = LocalSettingManager.ReadSetting("LI" + dl.Id);
				try
				{
					var ls = JsonConvert.DeserializeObject<LinkItems[]>(save);
					string url = GetVideoLink(dl.Id, ls[0].quality);
					links.Add(url);
				}
				catch { }
			}
			return string.Join(Environment.NewLine, links.ToArray());
		}

		public async static Task<List<DownloadedItems>> SearchVideo(string input, string maxRes)
		{
			var str = CloseHelp.Reverse(CloseHelp.Base64Encode(input));
			string url = "https://shahriar.in/app/ydm/search/search.php?q=" + str + "&maxResults=" + CloseHelp.Base64Encode(maxRes);
			string respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
			if (respond.Substring(0, 3) == "Err") {
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
	}
}
