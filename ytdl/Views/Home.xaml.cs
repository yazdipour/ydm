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
				Api.clist = JsonConvert.DeserializeObject<ObservableCollection<DownloadedItems>>(sv);
			}
			catch { }
			xlist.ItemsSource = Api.clist;
		}

		private void GetInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string input = urlText.Text.Trim();
			if (input.Length < 4)
				return;
			Api.GetVideo(input);
			xlist.ItemsSource = Api.clist;
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
			Api.clist.Remove(item);
			xlist.ItemsSource = Api.clist;
			LocalSettingManager.RemoveSetting("LI" + item.Id);
			LocalSettingManager.SaveSetting("DI", JsonConvert.SerializeObject(Api.clist));
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
		private void SearchBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SearchPanel));
		}
	}
}
