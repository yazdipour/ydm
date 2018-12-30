using System;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace YDM.UWP.Views
{
	public sealed partial class StreamPanel : Page
	{
        public StreamPanel() => InitializeComponent();
        protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var url= e.Parameter.ToString();
			Uri manifestUri = new Uri(url);
			mediaPlayerElement.Source = MediaSource.CreateFromUri(manifestUri);
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			mediaPlayerElement.MediaPlayer.Play();
			//Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Stream Usage");
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			mediaPlayerElement.MediaPlayer.Pause();
		}
	}
}
