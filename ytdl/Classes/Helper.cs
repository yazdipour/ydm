using System;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ytdl.Classes
{
	class Helper
	{
		internal SolidColorBrush ConvertFrom(string color)
		{
			if (!color.Contains("#"))
				color = "#" + color;
			if (color.Length > 7)
			{
				var A = Convert.ToByte(color.Substring(1, 2), 16);
				var R = Convert.ToByte(color.Substring(3, 2), 16);
				var G = Convert.ToByte(color.Substring(5, 2), 16);
				var B = Convert.ToByte(color.Substring(7, 2), 16);
				return new SolidColorBrush(Color.FromArgb(A, R, G, B));
			}
			if (color.Length > 4)
			{
				var R = Convert.ToByte(color.Substring(1, 2), 16);
				var G = Convert.ToByte(color.Substring(3, 2), 16);
				var B = Convert.ToByte(color.Substring(5, 2), 16);
				return new SolidColorBrush(Color.FromArgb(255, R, G, B));
			}
			else
				throw new Exception();
		}
		internal async void ChangeStatus()
		{
			try
			{
				StatusBar statusBar = StatusBar.GetForCurrentView();
				statusBar.BackgroundColor = ConvertFrom("#BD2810").Color;
				statusBar.ForegroundColor = Colors.White;
				statusBar.ProgressIndicator.ProgressValue = 0;
				statusBar.BackgroundOpacity = 1;
				await statusBar.ShowAsync();
			}
			catch (Exception) { }
		}
		internal void ChangeTitle()
		{
			var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
			titleBar.InactiveBackgroundColor = titleBar.BackgroundColor = ConvertFrom("#BD2810").Color;
			titleBar.InactiveForegroundColor = titleBar.ForegroundColor = Colors.White;
			titleBar.ButtonInactiveBackgroundColor = titleBar.ButtonBackgroundColor = ConvertFrom("#BD2810").Color;
			titleBar.ButtonForegroundColor = Colors.White;
		}

		internal void ReloadFrame(Frame Frame)
		{
			try
			{
				var type = Frame.CurrentSourcePageType;
				Frame.Navigate(type);
				Frame.BackStack.Remove(Frame.BackStack.Last());
			}
			catch { }
		}

		internal void Visibility_TitleBar(bool visible)
		{
			var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = !visible;
			Windows.UI.Xaml.Window.Current.SetTitleBar(null);
		}
	}
}
