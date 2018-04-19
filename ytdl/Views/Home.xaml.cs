using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;
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
		private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (xlist.Items.Count == 0) LoadListAsync();
		}
		private async void LoadListAsync()
		{
			try
			{
				var sv = await AkavacheHelper.ReadStringLocal("MainList");
				Api.clist = JsonConvert.DeserializeObject<ObservableCollection<DownloadedItems>>(sv);
			}
			catch { }
			xlist.ItemsSource = Api.clist;
		}
		//Click on item
		private async void xlist_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				var dl = e.ClickedItem as DownloadedItems;
				if (dl == null) return;
				var dialog = new DownloadedDialog() { dl = dl };
				var t = dialog.ShowAsync();
				await t;
				if (t.Status == Windows.Foundation.AsyncStatus.Completed)
					if (dialog.changed== "reload") LoadListAsync();
					else if (dialog.changed.Length>2) Frame.Navigate(typeof(StreamPanel),dialog.changed);
			}
			catch { }
		}
		//Big Red Button
		private async void GetInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string input = urlText.Text.Trim();
			if (input.Length < 4) return;
			try
			{
				var key = await Api.GetVideo(input);
				if (key != null) xlist.ItemsSource = Api.clist;
			}
			catch { CloseHelp.ShowMSG("Error!"); }
		}
		//Copy Button
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
		//Slide To Remove
		private void SlidableListItem_RightCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			if (slidableitem == null) return;
			var item = slidableitem.DataContext as DownloadedItems;
			if (item == null) return;
			async void save()
			{
				try
				{
					await AkavacheHelper.RemoveFromLocal("LI" + item.Id);
					await AkavacheHelper.SaveStringLocal("MainList", JsonConvert.SerializeObject(Api.clist));
				}
				catch { }
			}
			try
			{
				Api.clist.Remove(item);
				xlist.ItemsSource = Api.clist;
				save();
			}
			catch { }
		}
	}
}
