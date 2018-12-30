using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using YDM.Share.Models;
using System.Reactive.Linq;
using Akavache;
using YDM.Share;

namespace YDM.UWP.Views
{
	public sealed partial class DownloadedDialog : ContentDialog
	{
		#region Head
		public string changed = "";
		internal DownloadedItems dl;

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
			try
			{
				oTitle.Text = dl.Title;
				Duration.Text = "Duration : " + dl.Duration;
				View.Text = dl.Views + " Views";
				Img.ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(dl.Img, UriKind.Absolute));
				await LoadListAsync();
				DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
				dataTransferManager.DataRequested += DataTransferManager_DataRequested;
			}
			catch
			{
				Hide();
			}
		}

		private async System.Threading.Tasks.Task LoadListAsync()
		{
			try
			{
				xlist.ItemsSource = await BlobCache.LocalMachine.GetObject<LinkItems[]>("LI" + dl.Id);
			}
			catch { }
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
			request.Data.SetText(txtBox.Text + "\n\n Watch Youtube Videos without censorship with YDM: https://goo.gl/QM2KC5");
			request.Data.Properties.Title = dl.Title;
		}

		private async void xlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FindName("txtBox");
			FindName("btnBox");
			var clicked = xlist.SelectedItem as LinkItems;
			//txtBox.Text = Api.GetVideoLink(dl.Id, clicked.tag);
			//sizeTxt.Text = "";
			//sizeLoading.IsIndeterminate = true;
			//string size = await Api.FillSizeAsync(dl.Id, clicked.url, xlist.SelectedIndex);
			//sizeTxt.Text = "File size: " + size;
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
			try
			{
				var ser = await BlobCache.LocalMachine.GetObject<List<DownloadedItems>>("MainList");
				ser.Remove(ser.Find(obj => obj.Id == dl.Id));
				//save changes
				await BlobCache.LocalMachine.InsertObject("MainList", ser);
				await BlobCache.LocalMachine.Invalidate("LI" + dl.Id);
				changed = "reload";
			}
			catch { }
			Hide();
		}
	}
}
