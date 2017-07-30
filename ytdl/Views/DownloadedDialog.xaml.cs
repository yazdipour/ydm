using Microsoft.Toolkit.Uwp.UI.Animations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using ytdl.Models;

namespace ytdl.Views
{
	public sealed partial class DownloadedDialog : ContentDialog
	{
		public bool changedBool = false;
		private DownloadedItems dl;
		internal DownloadedItems Dl
		{
			get
			{
				return dl;
			}

			set
			{
				dl = value;
			}
		}
		public DownloadedDialog()
		{
			InitializeComponent();
			changedBool = false;
		}
		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Hide();
		}

		private async void xlist_ItemClick(object sender, ItemClickEventArgs e)
		{
			await copiedText.Fade(value: 1, duration: 0, delay: 0).StartAsync();
			var clicked = (e.ClickedItem) as LinkItems;
			string url = Api.GetVideoLink(dl.Id,clicked.quality);
			var dataPackage = new DataPackage();
			dataPackage.SetText(url);
			Clipboard.SetContent(dataPackage);
			copiedText.Visibility = Windows.UI.Xaml.Visibility.Visible;
			FindName("txtBox");
			txtBox.Text = url;
			await copiedText.Fade(value: 0, duration: 1000, delay: 1000).StartAsync();
		}

		private void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			oTitle.Text = dl.Title;
			Duration.Text = "Duration : " + dl.Duration;
			View.Text = dl.Views + " Views";
			Img.ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dl.Img, UriKind.Absolute));
			//TODO: DB
			//Getting Links
			var save = LocalSettingManager.ReadSetting("LI" + dl.Id);
			try
			{
				var ls = JsonConvert.DeserializeObject<LinkItems[]>(save);
				xlist.ItemsSource = ls;
			}
			catch { }
		}

		private async void OpenYT_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Uri uri = new Uri("http://www.youtube.com/watch?v=" + dl.Id);
			await Windows.System.Launcher.LaunchUriAsync(uri);
		}

		private void rm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//TODO: DB
			//ReadLocalValue it 1st
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(LocalSettingManager.ReadSetting("DI"));
			ser.Remove(ser.Find(obj => obj.Id == dl.Id));
			//save changes
			LocalSettingManager.SaveSetting("DI", JsonConvert.SerializeObject(ser));
			LocalSettingManager.RemoveSetting("LI" + dl.Id);
			changedBool = true;
			Hide();
		}
	}
}
