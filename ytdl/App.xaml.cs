using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ytdl.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace ytdl
{
	sealed partial class App : Application
	{
		private static User usr;
		internal static int Today = 0;
		internal static User Usr
		{
			get { return usr; }
			set { usr = value; }
		}
		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;
			AppCenter.Start("de21106e-0606-4849-869c-de65c671e627", typeof(Analytics));
			Analytics.SetEnabledAsync(true);
		}
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			Frame rootFrame = Window.Current.Content as Frame;
			if (rootFrame == null)
			{
				rootFrame = new Frame();
				rootFrame.NavigationFailed += OnNavigationFailed;
				//if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				//{
				//	//Load state from previously suspended application
				//}
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					if (Classes.LocalSettingManager.ExistsSetting("Account"))
					{
						rootFrame.Navigate(typeof(Views.MotherPanel), e.Arguments);
						try
						{
							var hlp = new Classes.Helper();
							hlp.ChangeTitle();
							hlp.ChangeStatus();
						}
						catch { }
					}
					else
					{
						rootFrame.Navigate(typeof(Views.WelcomePage), e.Arguments);
						try
						{
							var hlp = new Classes.Helper();
							hlp.Visibility_TitleBar(false);
						}
						catch { }
					}
				}
				// Ensure the current window is active
				Window.Current.Activate();
			}
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			Classes.AkavacheHelper.ShutDown();
			var deferral = e.SuspendingOperation.GetDeferral();
			//Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
