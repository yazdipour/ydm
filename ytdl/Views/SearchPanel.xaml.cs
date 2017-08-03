using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
		private async void SlidableListItem_RightCommandRequested(object sender, EventArgs e)
		{
			var slidableitem = sender as SlidableListItem;
			var item = slidableitem.DataContext as DownloadedItems;
			try
			{
				var key = await Api.GetVideo(item.Id);
				if (key == null) return;
				await Api.FillSizeAsync(key);
			}
			catch { CloseHelp.ShowMSG("Error!"); }
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
				var dlr = await Api.SearchVideo(str, maxRes);
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
		List<CheckBox> ListCheck=null;

		private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string html = htmlTextBox.Text.Trim();
			if (html.Length < 20) return;
			Regex regex = new Regex(@"data-video-id=""(.*?)""");
			var matchs = regex.Matches(html);
			var count = App.Usr.nrCanDownload;
			ListCheck = new List<CheckBox>();
			foreach (Match match in matchs)
			{
				try
				{
					var m = match.Value.Substring(14).Replace("\"", "");
					if (m.Length < 6) continue;
					ListCheck.Add(new CheckBox() { Content = "https://www.youtube.com/watch?v=" + m, IsChecked = (--count>0)});
				}
				catch { }
			}
			FindName("LstChk");
			FindName("BtnExtract");
			FindName("Selecters");
			
			LstChk.ItemsSource = ListCheck;
		}

		private async void Button_Click_2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var count = ListCheck.FindAll(obj => obj.IsChecked == true).Count;
			if (count>App.Usr.nrCanDownload)
			{
				CloseHelp.ShowMSG("You can't download "+count+" videos with your account");
			}
			else
			{
				MotherPanel.StaticRing.IsLoading = true;
				foreach (var item in ListCheck.FindAll(obj => obj.IsChecked == true))
				{
					try
					{
						var key = await Api.GetVideo(item.Content.ToString(), false,false);
						if (key == null) continue;
						await Api.FillSizeAsync(key);
					}
					catch { }
				}
				MotherPanel.StaticRing.IsLoading = false;
			}
		}

		private void Button_Click_De(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			foreach(var item in (ListCheck.FindAll(obj => obj.IsChecked == true)))
				item.IsChecked = false;
			LstChk.ItemsSource = ListCheck;
		}

		private void Button_Click_Se(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			foreach (var item in (ListCheck.FindAll(obj => obj.IsChecked == false)))
				item.IsChecked = true;
			LstChk.ItemsSource = ListCheck;
		}
	}
}
