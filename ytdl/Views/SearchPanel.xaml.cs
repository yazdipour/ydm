using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
			try
			{
				var slidableitem = sender as SlidableListItem;
				var item = slidableitem.DataContext as DownloadedItems;
				var key = await Api.GetVideo(item.Id);
				if (key == null) return;
				//await Api.FillSizeAsync(key);
			}
			catch { CloseHelp.ShowMSG("Error!"); }
		}
		private async void SlidableListItem_LeftCommandRequested(object sender, EventArgs e)
		{
			try
			{
				var slidableitem = sender as SlidableListItem;
				var item = slidableitem.DataContext as DownloadedItems;
				Uri uri = new Uri("http://www.youtube.com/watch?v=" + item.Id);
				await Windows.System.Launcher.LaunchUriAsync(uri);
			}
			catch { }
		}

		private async void lst1_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				var item = e.ClickedItem as DownloadedItems;
				var key = await Api.GetVideo(item.Id);
				if (key == null) return;
				//await Api.FillSizeAsync(key);
			}
			catch { CloseHelp.ShowMSG("Error!"); }
		}
		/// <summary>
		/// Pivot 1
		/// </summary>
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
		/// <summary>
		/// Pivot 2 PL
		/// </summary>
		private void CpButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			try
			{
				List<DownloadedItems> x = (lst2.ItemsSource as List<DownloadedItems>);
				if (x.Count > App.Usr.nrCanDownload)
				{
					CloseHelp.ShowMSG("You can't download " + x.Count + " videos with your account");
				}
				else
				{
					List<string> ls = new List<string>();
					foreach (var item in x)
						ls.Add(item.Id);
					Api.GetBatchOfVideos(ls);
				}
			}
			catch { }
		}
		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var tag = (sender as Button).Tag.ToString();
			try
			{
				if (tag == "2") FindName("GetAll");
				LoadItemsAsync(tag);
			}
			catch
			{
				CloseHelp.ShowMSG("Something happend!");
			}
		}
		/// <summary>
		/// Pivot 3 WL
		/// </summary>
		List<CheckBox> ListCheck = null;

		private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string html = htmlTextBox.Text.Trim();
			string filter = FilterBox.Text.Trim();
			if (html.Length < 20) return;
			if (filter.Length < 2) return;
			Regex regex = new Regex(filter + @"=""(.*?)""");
			var matchs = regex.Matches(html);
			var count = App.Usr.nrCanDownload;
			ListCheck = new List<CheckBox>();
			var tempList= new List<string>();
			foreach (Match match in matchs)
			{
				try
				{
					var m = match.Value.Substring(filter.Length + 1).Replace("\"", "");
					if (m.Length < 6) continue;
					if (m.Contains("?v=")) { m = m.Substring(m.IndexOf("?v=")+3); }
					tempList.Add(m);
				}
				catch { }
			}
			tempList = tempList.Distinct().ToList();
			foreach (var item in tempList)
				ListCheck.Add(new CheckBox() { Content = "https://www.youtube.com/watch?v=" + item, IsChecked = (--count > 0) });
			FindName("LstChk");
			FindName("BtnExtract");
			FindName("Selecters");
			LstChk.ItemsSource = ListCheck;
		}

		private void Button_Click_2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var ls = ListCheck.FindAll(obj => obj.IsChecked == true);
			if (ls.Count > App.Usr.nrCanDownload)
			{
				CloseHelp.ShowMSG("You can't download " + ls.Count + " videos with your account");
			}
			else
			{
				var lss = new List<string>();
				foreach (var item in ls)
				{
					lss.Add(item.Content.ToString());
				}
				Api.GetBatchOfVideos(lss);
			}
		}

		private void Button_Click_De(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			foreach (var item in (ListCheck.FindAll(obj => obj.IsChecked == true)))
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
