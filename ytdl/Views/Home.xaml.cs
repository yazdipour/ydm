using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
				if (dialog.changedBool) LoadListAsync();
			}
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
		private async void GetInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string input = urlText.Text.Trim();
			if (input.Length < 4)
				return;
			try
			{
				var key = await Api.GetVideo(input);
				if (key == null) return;
				xlist.ItemsSource = Api.clist;
			}
			catch { CloseHelp.ShowMSG("Error!"); }
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
		private void SlidableListItem_RightCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			var item = slidableitem.DataContext as DownloadedItems;
			Api.clist.Remove(item);
			xlist.ItemsSource = Api.clist;
			async void save()
			{
				await AkavacheHelper.RemoveFromLocal("LI" + item.Id);
				await AkavacheHelper.SaveStringLocal("MainList", JsonConvert.SerializeObject(Api.clist));
			}
			save();
		}
	}
}
