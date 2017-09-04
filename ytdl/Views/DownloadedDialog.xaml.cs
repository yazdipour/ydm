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
#region Head
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
		#endregion
		private async void OpenYT_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Uri uri = new Uri("http://www.youtube.com/watch?v=" + dl.Id);
			await Windows.System.Launcher.LaunchUriAsync(uri);
		}

		private async void rm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			//ReadLocalValue it 1st
			var m = await AkavacheHelper.ReadStringLocal("MainList");
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(m);
			ser.Remove(ser.Find(obj => obj.Id == dl.Id));
			//save changes
			await AkavacheHelper.SaveStringLocal("MainList", JsonConvert.SerializeObject(ser));
			await AkavacheHelper.RemoveFromLocal("LI" + dl.Id);
			changedBool = true;
			Hide();
		}
		private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			oTitle.Text = dl.Title;
			Duration.Text = "Duration : " + dl.Duration;
			View.Text = dl.Views + " Views";
			Img.ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dl.Img, UriKind.Absolute));
			var save = await AkavacheHelper.ReadStringLocal("LI" + dl.Id);
			try
			{
				var ls = JsonConvert.DeserializeObject<LinkItems[]>(save);
				xlist.ItemsSource = ls;
			}
			catch { }

		}
		private async void Link_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string url = txtBox.Text;
			if ((sender as Button).Tag.ToString()== "0")
			{
				await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
			}
			else
			{
				var dataPackage = new DataPackage();
				dataPackage.SetText(url);
				Clipboard.SetContent(dataPackage);
			}
		}
		private async void xlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FindName("txtBox");
			FindName("btnBox");
			var clicked = xlist.SelectedItem as LinkItems;
			txtBox.Text = Api.GetVideoLink(dl.Id,clicked.quality);
			sizeTxt.Text = "";
			sizeLoading.IsIndeterminate = true;
			string size=await Api.FillSizeAsync(dl.Id, clicked.url, xlist.SelectedIndex);
			sizeTxt.Text = "File size: " + size;
			sizeLoading.IsIndeterminate = false;
		}
	}
}
