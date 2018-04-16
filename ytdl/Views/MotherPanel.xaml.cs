using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using ytdl.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.AppCenter.Analytics;

namespace ytdl.Views
{
	public sealed partial class MotherPanel : Page
	{
		public static Loading StaticRing { get; set; }
		public static AppBarButton StaticNr { get; set; }
		public MotherPanel()
		{
			AkavacheHelper.Init();
			this.InitializeComponent();
		}

		private async void Page_Loading(FrameworkElement sender, object args)
		{
			LoadingControl.IsLoading = true;
			StaticNr = NrAppbar;
			if (await Api.MakeMyDayAsync())
			{
				StaticRing = LoadingControl;
				TopBar.Visibility = Visibility.Visible;
				insideFrame.Navigate(typeof(Home));
				LoadingControl.IsLoading = false;
			}
			else
			{
				loadingTxt.Text = "Error! Connection problem";
				reloadBtn.Visibility = Visibility.Visible;
				Analytics.TrackEvent("Connection_Error");
			}
		}
		private void insideFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			if (insideFrame.CurrentSourcePageType == typeof(Home))
			{
				TopMenu.Visibility = Visibility.Visible;
				SystemNavigationManager.GetForCurrentView().BackRequested -= CurrentView_BackRequested;
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
			}
			else
			{
				SystemNavigationManager.GetForCurrentView().BackRequested += CurrentView_BackRequested;
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
				TopMenu.Visibility = Visibility.Collapsed;
			}
		}
		private void CurrentView_BackRequested(object sender, BackRequestedEventArgs e)
		{
			if (insideFrame == null || !insideFrame.CanGoBack) return;
			e.Handled = true;
			insideFrame.GoBack();
		}

		//Reload - After Offline Dialog
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				new Helper().ReloadFrame(Frame);
			}
			catch { }
		}

		#region NavBar Btns
		private void GetAllLinks_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var m = sender as MenuFlyoutItem;
				Api.GetAllVideoLinkAsync(m.Tag.ToString());
				Analytics.TrackEvent("Click Mother.GetAll");
			}
			catch { }
		}
		private async void RmAll_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var m = await AkavacheHelper.ReadStringLocal("MainList");
				var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(m);
				foreach (var dl in ser)
					await AkavacheHelper.RemoveFromLocal("LI" + dl.Id);
				await AkavacheHelper.RemoveFromLocal("MainList");
			}
			catch { }
			Api.clist = new System.Collections.ObjectModel.ObservableCollection<DownloadedItems>();
			Frame.BackStack.Clear();
			new Helper().ReloadFrame(Frame);
			Analytics.TrackEvent("Click Mother.RmAll");
		}
		private void UserDash_Click(object sender, RoutedEventArgs e)
		{
			Nav(typeof(UserPanel));
		}
		private void NavBtn(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Tag.ToString())
			{
				case "1":
					Nav(typeof(Home));
					break;
				case "2":
					Nav(typeof(SearchPanel), "search");
					break;
				case "3":
					Nav(typeof(SearchPanel), "playlist");
					break;
				case "4":
					Nav(typeof(SearchPanel), "advance");
					break;
			}
		}
		private void Nav(System.Type cls, object param = null)
		{
			if (param != null || insideFrame.CurrentSourcePageType != cls)
				insideFrame.Navigate(cls, param);
		}
		#endregion
	}
}
