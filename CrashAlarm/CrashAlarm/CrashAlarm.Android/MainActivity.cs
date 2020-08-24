using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using Prism;
using Prism.Ioc;

namespace CrashAlarm.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private PermissionService _permissionService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _permissionService = new PermissionService();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            UserDialogs.Init(this);
            LoadApplication(new App(new AndroidInitializer()));

        }

        //async Task TryToGetPermissions()
        //{
        //    async Task GetPermissionsAsync()
        //    {
        //        const string permission = Manifest.Permission.SendSms;
        //        if (CheckSelfPermission(permission) == (int) Android.Content.PM.Permission.Granted)
        //        {
        //            Toast.MakeText(this, "Permission for SMS granted", ToastLength.Short).Show();
        //            return;
        //        }

        //        RequestPermissions(Manifest.Permission.SendSms, 0);
        //    }
        //}

        //private async Task requestPermision()
        //{
        //    await _permissionService.RequestStoragePermission();
        //    await _permissionService.RequestCameraPermission();
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

