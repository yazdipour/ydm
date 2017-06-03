﻿using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ytdl.Models;

namespace ytdl {
    sealed partial class App : Application
    {
        private static User usr;
        internal static int Today=0;
        internal static User Usr
        {
            get { return usr; }
            set { usr = value; }
        }
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    if(Classes.LocalSettingManager.ExistsSetting("Account"))
                        rootFrame.Navigate(typeof(Views.MotherPanel),e.Arguments);
                    else
                        rootFrame.Navigate(typeof(Views.WelcomePage), e.Arguments);
                    try {
                        var hlp = new Classes.Helper();
                        hlp.ChangeTitle();
                        hlp.ChangeStatus();
                    }
                    catch {}
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summarm name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
