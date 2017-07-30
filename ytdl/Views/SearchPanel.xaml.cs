using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using ytdl.Models;

namespace ytdl.Views
{
	public sealed partial class SearchPanel : Page
	{
		public SearchPanel()
		{
			this.InitializeComponent();
		}

		private void SlidableListItem_RightCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			var item = slidableitem.DataContext as DownloadedItems;
			Api.GetVideo(item.Id);
		}
		private async void SlidableListItem_LeftCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			var item = slidableitem.DataContext as DownloadedItems;
			Uri uri = new Uri("http://www.youtube.com/watch?v=" + item.Id);
			await Windows.System.Launcher.LaunchUriAsync(uri);
		}
		private async void LoadItemsAsync(string tag)
		{
			MotherPanel.StaticRing.IsLoading = true;
			if (tag == "1")
			{
				var str = searchField.Text.Trim();
				string maxRes = (cmBox.SelectedItem as ComboBoxItem).Content.ToString();
				if (str.Length < 2) { MotherPanel.StaticRing.IsLoading = false; return; }
				var dlr=await Api.SearchVideo(str, maxRes);
				if (dlr == null)
				{
					CloseHelp.ShowMSG("Err");
					MotherPanel.StaticRing.IsLoading = false;
					return;
				}
				lst1.ItemsSource = dlr;
			}
			else
			{
				var str = searchField2.Text.Trim();
				if (str.Length < 2) { MotherPanel.StaticRing.IsLoading = false; return; }
				var dlr = await Api.GetPlayList(str);
				if (dlr == null)
				{
					CloseHelp.ShowMSG("Err");
					MotherPanel.StaticRing.IsLoading = false;
					return;
				}
				lst2.ItemsSource = dlr;
			}
			MotherPanel.StaticRing.IsLoading = false;
		}

		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var tag = (sender as Button).Tag.ToString();
			LoadItemsAsync(tag);
		}

		private void searchField_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				var tag = (sender as TextBox).Tag.ToString();
				LoadItemsAsync(tag);
				// Make sure to set the Handled to true, otherwise the RoutedEvent might fire twice
				e.Handled = true;
			}
		}

		private async void CpButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var dataPackageView = Clipboard.GetContent();
			if (dataPackageView.Contains(StandardDataFormats.Text))
			{
				try
				{
					searchField2.Text = (await dataPackageView.GetTextAsync()).Trim();
				}
				catch { }
			}
		}
	}
}
