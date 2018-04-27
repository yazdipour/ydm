using Microsoft.AppCenter.Analytics;
using Windows.UI.Xaml.Controls;
using System;
using Windows.ApplicationModel.Email;
using ytdl.Classes;
using ytdl.Models;
using Windows.UI.Popups;
using Akavache;
using System.Reactive.Linq;

namespace ytdl.Views
{
	public sealed partial class UserPanel : Page
	{
		public UserPanel()
		{
			this.InitializeComponent();
			Img = App.Usr.Picture;
			leftDay = App.Usr.nrCanDownload;
			mailTxt = App.Usr.Email;
			Analytics.TrackEvent("Visit UserPanel");
		}
		User usr = new User();
		string Img = "", mailTxt, Version = "V2.0";
		int leftDay = 0;
		private async void Logout_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			string msg = "آیا قصد خروج از این حساب را دارید؟";
			var dialog = new MessageDialog(msg);
			dialog.Commands.Add(new UICommand("بیخیال"));
			dialog.Commands.Add(new UICommand("باشه", OnOKButtonClicked));
			await dialog.ShowAsync();
			Analytics.TrackEvent("Click UserPanel.Logout");
		}
		private async void OnOKButtonClicked(IUICommand command)
		{
			LocalSettingManager.RemoveSetting("Account");
			await BlobCache.LocalMachine.Invalidate("MainList");
			await Api.YdmApi.Logout();
			Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
			rootFrame.Navigate(typeof(WelcomePage));
			Analytics.TrackEvent("Logout");
		}
		private async void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			MotherPanel.StaticRing.IsLoading = true;
			await Api.MakeMyDayAsync();
			MotherPanel.StaticRing.IsLoading = false;
			new Helper().ReloadFrame(Frame);
			Analytics.TrackEvent("Click UserPanel.Reload");
		}

		#region BTN
		private async void Err1_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var mail = new EmailMessage();
			mail.To.Add(new EmailRecipient { Address = "app@shahriar.in" });
			mail.Subject = "مشکل در شارژ YDM" + Version;
			await EmailManager.ShowComposeNewEmailAsync(mail);
			Analytics.TrackEvent("Click UserPanel.Err1");
		}

		private async void Err2_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var mail = new EmailMessage();
			mail.To.Add(new EmailRecipient { Address = "app@shahriar.in" });
			mail.Subject = "مشکل در برنامه YDM" + Version;
			await EmailManager.ShowComposeNewEmailAsync(mail);
			Analytics.TrackEvent("Click UserPanel.Err2");
		}

		private async void Charge_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var url = Api.BASE_URL + "/pay/?e=" + App.Usr.Email;
			await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
			Analytics.TrackEvent("Click UserPanel.ChargeBtn");
		}
		#endregion

	}
}
