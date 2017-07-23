using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using ytdl.Models;

namespace ytdl.Views
{
	public sealed partial class Home : Page
	{
		public Home()
		{
			this.InitializeComponent();
		}
		ObservableCollection<DownloadedItems> clist = new ObservableCollection<DownloadedItems>();
		private async void xlist_ItemClick(object sender, ItemClickEventArgs e)
		{
			var dialog = new DownloadedDialog()
			{
				Dl = e.ClickedItem as DownloadedItems
			};
			var t = dialog.ShowAsync();
			await t;
			if (t.Status == Windows.Foundation.AsyncStatus.Completed)
			{
				if (dialog.changedBool) LoadList();
			}
		}
		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (xlist.Items.Count == 0) LoadList();
		}
		private void LoadList()
		{
			try
			{
				var sv = LocalSettingManager.ReadSetting("DI");
				clist = JsonConvert.DeserializeObject<ObservableCollection<DownloadedItems>>(sv);
			}
			catch { }
			xlist.ItemsSource = clist;
		}
		//GetVideo Func
		async void GetTheVideo(string input)
		{
			if (App.Today == 0)
			{
				ShowMSG("خطا در ارتباط! برنامه را ریستارت کنید");
				return;
			}
			if (App.Usr.leftDay < 0)
			{
				if (LocalSettingManager.ExistsSetting("F" + App.Today))
				{
					ShowMSG("حساب خود را شارژ کنید");
					return;
				}
			}
			MotherPanel.StaticRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
			//string u = CloseHelp.Base64Encode(CloseHelp.Base64Encode(CloseHelp.Reverse((App.Today + App.Usr.Id).ToString())));
			string u = CloseHelp.Base64Encode(CloseHelp.Base64Encode(CloseHelp.Reverse(App.Usr.Id.ToString())));
			string video = CloseHelp.Base64Encode(input);
			string url = "http://shahriar.in/app/ytdlr/dl/getvideo.php?u=" + u + "&videoid=" + video;
			string respond = await CloseHelp.DownloadPages(new System.Threading.CancellationToken(false), url);
			if (respond.Substring(0, 3) == "Err")
			{
				ShowMSG(respond);
				return;
			}
			var temp = respond.Split(new string[] { "[*]" }, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				var DLI = JsonConvert.DeserializeObject<DownloadedItems>(temp[0]);
				var LI = JsonConvert.DeserializeObject<LinkItems[]>(temp[1]);
				var urls = JsonConvert.DeserializeObject<string[]>(temp[2]);
				clist.Insert(0, DLI);
				xlist.ItemsSource = clist;
				int dr = Convert.ToInt32(DLI.Duration);
				DLI.Duration = string.Format("{0}:{1}", dr / 60, dr % 60);
				int index = 0;
				foreach (var item in LI)
				{
					try
					{
						item.size = " | " + await CloseHelp.GetSize(urls[index]);
					}
					catch { }
					item.quality += " | " + item.type.Replace("video/", "") + item.size;
					index++;
				}
				LocalSettingManager.SaveSetting("DI", JsonConvert.SerializeObject(clist));
				LocalSettingManager.SaveSetting("LI" + DLI.Id, JsonConvert.SerializeObject(LI));
			}
			catch
			{
				ShowMSG("در گرفتن لینک مشکلی به وجود امد. لطفا لینک را دوباره بررسی کنید");
			}
			MotherPanel.StaticRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		//GetVideo Btn Event
		private void GetInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string input = urlText.Text.Trim();
			if (input.Length < 4)
				return;
			GetTheVideo(input);
		}
		private async void Copy_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var dataPackageView = Clipboard.GetContent();
			if (dataPackageView.Contains(StandardDataFormats.Text))
			{
				try
				{
					urlText.Text = (await dataPackageView.GetTextAsync()).Trim();
				}
				catch { }
			}
		}
		private void SlidableListItem_RightCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			var item = slidableitem.DataContext as DownloadedItems;
			clist.Remove(item);
			xlist.ItemsSource = clist;
			LocalSettingManager.RemoveSetting("LI" + item.Id);
			LocalSettingManager.SaveSetting("DI", JsonConvert.SerializeObject(clist));
		}
		private async void SymbolIcon_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			var dataPackageView = Clipboard.GetContent();
			if (dataPackageView.Contains(StandardDataFormats.Text))
			{
				try
				{
					urlText.Text = (await dataPackageView.GetTextAsync()).Trim();
				}
				catch { }
			}
		}
		//shows error
		async void ShowMSG(string msg)
		{
			var dialog = new MessageDialog(msg);
			MotherPanel.StaticRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			dialog.Commands.Add(new UICommand("Close"));
			await dialog.ShowAsync();
		}

		private void SearchBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SearchPanel));
		}
	}
}
