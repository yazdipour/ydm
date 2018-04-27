using System.Linq;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;
using System.Reactive.Linq;

namespace ytdl.Views
{

	public sealed partial class LoginPage : Page
	{
		public LoginPage()
		{
			this.InitializeComponent();
		}

		string LoginUrl = Api.BASE_URL + "/auth/auth.php";
		string destinationUrl = Api.BASE_URL + "/auth/index.php?i=";

		private async void WebView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
		{
			try
			{
				string send = sender.Source.ToString().Trim();
				if (send.Length > destinationUrl.Length && send.Substring(0, destinationUrl.Length).Contains(destinationUrl))
				{
					try
					{
						sender.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
						string email = send.Substring(send.IndexOf(destinationUrl) + destinationUrl.Length);
						var temp = Utils.Base64Decode(email);
						email = Utils.Base64Decode(Utils.Reverse(temp.Substring(0, temp.Length - 1)));
						email = Utils.Base64Encode(Utils.Reverse(Utils.Base64Encode("#" + email)));
						var account= await Api.YdmApi.GetUser(email);
						if (account.Substring(0, 3) == "Err") throw new System.Exception();
						LocalSettingManager.SaveSetting("Account", account);
						Frame.BackStack.Remove(Frame.BackStack.Last());
						Frame.Navigate(typeof(MotherPanel));
					}
					catch { sender.Navigate(new System.Uri(LoginUrl)); }
				}
			}
			catch { }
		}
	}
}
