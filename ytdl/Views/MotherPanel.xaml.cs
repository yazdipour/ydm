using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using ytdl.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;

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
		#region Acrylic
		//applyAcrylicAccent(MainGrid);
		//private void applyAcrylicAccent(Panel panel)
		//{
		//	_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
		//	_hostSprite = _compositor.CreateSpriteVisual();
		//	_hostSprite.Size = new Vector2((float)panel.ActualWidth, (float)panel.ActualHeight);
		//	ElementCompositionPreview.SetElementChildVisual(panel, _hostSprite);
		//	_hostSprite.Brush = _compositor.CreateHostBackdropBrush();
		//}
		//Compositor _compositor;
		//SpriteVisual _hostSprite;
		//private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		//{
		//	if (_hostSprite != null)
		//		_hostSprite.Size = e.NewSize.ToVector2();
		//}
		#endregion
		private async void Page_Loading(FrameworkElement sender, object args)
		{
			LoadingControl.IsLoading = true;
			StaticNr = NrAppbar;
			if (await Api.MakeMyDayAsync())
			{
				StaticRing = LoadingControl;
				insideFrame.Navigate(typeof(Home));
				LoadingControl.IsLoading = false;
			}
			else
			{
				loadingTxt.Text = "Error! Connection problem";
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
			if (insideFrame == null || !insideFrame.CanGoBack) return;
			e.Handled = true;
			insideFrame.GoBack();
		}
		private async void GetAllLinks_Click(object sender, RoutedEventArgs e)
		{
			//MenuFlyOut.GetAllLinks
			var res = await Api.GetAllVideoLinkAsync();
			var dataPackage = new DataPackage();
			dataPackage.SetText(res);
			Clipboard.SetContent(dataPackage);
		}
		private async void RmAll_Click(object sender, RoutedEventArgs e)
		{
			var m = await AkavacheHelper.ReadStringLocal("MainList");
			var ser = JsonConvert.DeserializeObject<List<DownloadedItems>>(m);
			foreach (var dl in ser)
				await AkavacheHelper.RemoveFromLocal("LI" + dl.Id);
			await AkavacheHelper.RemoveFromLocal("MainList");
			Api.clist = new System.Collections.ObjectModel.ObservableCollection<DownloadedItems>();

			new Helper().ReloadFrame(Frame);
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				new Helper().ReloadFrame(Frame);
			}
			catch { }
		}
		private void UserDash_Click(object sender, RoutedEventArgs e)
		{
			insideFrame.Navigate(typeof(UserPanel));
		}
	}
}
