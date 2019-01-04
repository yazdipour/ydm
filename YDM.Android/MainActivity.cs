using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;

namespace YDM.Droid
{
    [Activity(Label = "YDM", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true
        , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
        , ScreenOrientation = ScreenOrientation.Portrait
        , WindowSoftInputMode = SoftInput.StateVisible | SoftInput.AdjustResize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            LoadApplication(new App());

            //#if DEBUG
            //XAMLator.Server.PreviewServer.Run();
            //#endif
        }

        public override void OnBackPressed()
        => XF.Material.Droid.Material.HandleBackButton(base.OnBackPressed);
    }
}