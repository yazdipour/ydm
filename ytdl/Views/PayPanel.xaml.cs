using System.Linq;
using Windows.UI.Xaml.Controls;
using ytdl.Classes;

namespace ytdl.Views
{
	public sealed partial class PayPanel : Page
	{
		public PayPanel()
		{
			this.InitializeComponent();
		}
		string LoginUrl = "https://shahriar.in/app/ydm/pay/?e=" + CloseHelp.Base64Encode(App.Usr.Email);
		string destinationUrl = "https://shahriar.in/app/ydm/auth/done.php";

		private void WebView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
		{
			string send = sender.Source.ToString().Trim();
			if (send == destinationUrl)
			{
				Frame.BackStack.Remove(Frame.BackStack.Last());
				Frame.Navigate(typeof(UserPanel),true);
			}
		}
	}
}
