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
		public string changed = "";
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
		}
		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Hide();
		}
		#endregion

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
			DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
			dataTransferManager.DataRequested += DataTransferManager_DataRequested;
		}
		private async void Link_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string url = txtBox.Text;
			switch ((sender as Button).Tag.ToString())
			{
				case "0":
					//web
					await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
					break;
				case "1":
					//copy
					var dataPackage = new DataPackage();
					dataPackage.SetText(url);
					Clipboard.SetContent(dataPackage);
					break;
				case "2":
					//stream
					changed = url;
					Hide();
					break;
				case "3":
					//share
					DataTransferManager.ShowShareUI();
					break;
			}
		}

		private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
		{
			DataRequest request = args.Request;
			request.Data.SetText(txtBox.Text+ "\n\n Watch Youtube Videos without censorship with YDM: https://goo.gl/QM2KC5");
			request.Data.Properties.Title = dl.Title;
		}
		private async void xlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FindName("txtBox");
			FindName("btnBox");
			var clicked = xlist.SelectedItem as LinkItems;
			txtBox.Text = clicked.url;
			sizeTxt.Text = "";
			sizeLoading.IsIndeterminate = true;
			string size=await Api.FillSizeAsync(dl.Id, clicked.url, xlist.SelectedIndex);
			sizeTxt.Text = "File size: " + size;
			sizeLoading.IsIndeterminate = false;
		}


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
			changed = "reload";
			Hide();
		}
	}
}
