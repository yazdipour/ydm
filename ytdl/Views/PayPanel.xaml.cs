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
		string LoginUrl = "https://shahriar.in/app/ydm/pay/?e=" + (App.Usr.Email);

		private void WebView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
		{
			string send = sender.Source.ToString().Trim();
			if (send.Contains("shahriar.in/app/ydm/pay/done.php"))
			{
				Frame.BackStack.Remove(Frame.BackStack.Last());
				Frame.Navigate(typeof(UserPanel), true);
			}
		}
	}
}
