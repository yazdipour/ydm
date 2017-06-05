using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ytdl.Classes;
using ytdl.Models;

namespace ytdl.Views
{
	public sealed partial class MotherPanel : Page
	{
		public static Grid StaticRing { get; set; }
		public MotherPanel()
		{
			this.InitializeComponent();
		}
		private void UserDash_Click(object sender, RoutedEventArgs e)
		{
			insideFrame.Navigate(typeof(UserPanel));
		}


		private async void Page_Loading(FrameworkElement sender, object args)
		{

			insideFrame.Navigate(typeof(Home));
			StaticRing = PRing;
			PRing.Visibility = Visibility.Visible;
			//TODO :Remove BackStack LoginPage if exist
			string account = LocalSettingManager.ReadSetting("Account");
			App.Usr = JsonConvert.DeserializeObject<User>(account);
			string get = JsonConvert.SerializeObject(new string[] { App.Usr.Id.ToString(), App.Usr.Email });
			get = CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(get)));
			string url = "http://shahriar.in/app/ytdlr/dl/getdate.php?i=" + get;
			try
			{
				url = await CloseHelp.DownloadPages(new System.Threading.CancellationToken(), url);
				if (url.Substring(0, 3).Equals("Err"))
					throw new System.Exception();
				var arr = url.Split('|');
				App.Usr.leftDay = System.Convert.ToInt32(arr[1]);
				if (App.Usr.leftDay < 0) App.Usr.leftDay = -1;
				App.Today = System.Convert.ToInt32(arr[0]);
				PRing.Visibility = Visibility.Collapsed;
				LeftDayText.Label = (1 + App.Usr.leftDay) + " روز";
			}
			catch
			{
				loadingTxt.Text = "مشکل در اتصال به سرور";
				ring.IsActive = false;
				reloadBtn.Visibility = Visibility.Visible;
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
			if (insideFrame == null || !insideFrame.CanGoBack)
				return;
			e.Handled = true;
			insideFrame.GoBack();
		}
		//Disable
		private void OpenSetting_Click(object sender, RoutedEventArgs e)
		{
			insideFrame.Navigate(typeof(SettingPanel));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var type = Frame.CurrentSourcePageType;
				Frame.Navigate(type);
				Frame.BackStack.Remove(Frame.BackStack.Last());
			}
			catch { }
		}

		private void GetAllLinks_Click(object sender, RoutedEventArgs e)
		{
			var links = new List<string>();
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(LocalSettingManager.ReadSetting("DI"));
			foreach (var dl in ser)
			{
				var save = LocalSettingManager.ReadSetting("LI" + dl.Id);
				try
				{
					var ls = JsonConvert.DeserializeObject<LinkItems[]>(save);
					string url = CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(CloseHelp.Reverse(CloseHelp.Base64Encode(ls[0].tag.ToString() + "#" + dl.Id)))) + "=");
					url = "http://shahriar.in/app/ytdlr/dl/get.php?i=" + url;
					links.Add(url);
				}
				catch { }
			}
			var dataPackage = new DataPackage();
			dataPackage.SetText(string.Join(System.Environment.NewLine, links.ToArray()));
			Clipboard.SetContent(dataPackage);
		}

		private void RmAll_Click(object sender, RoutedEventArgs e)
		{
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(LocalSettingManager.ReadSetting("DI"));
			foreach (var dl in ser)
				LocalSettingManager.RemoveSetting("LI" + dl.Id);
			LocalSettingManager.RemoveSetting("DI");
			Button_Click(null,null);
		}

		private void Batch_Click(object sender, RoutedEventArgs e)
		{
			//var x= Clipboard.GetContent().GetTextAsync();
		}
	}
}
