using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;

namespace CarCustomChecker
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var configBuilder = new RequestConfiguration.Builder();

            configBuilder.SetTestDeviceIds(new List<string>() { "558BEEA7EC8B11BD288CD4BC81AACA59" });

            MobileAds.RequestConfiguration = configBuilder.Build();
        }
    }
}
