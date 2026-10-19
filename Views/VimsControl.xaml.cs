using CarCustomChecker.Models;

namespace CarCustomChecker.Views;

public partial class VimsControl : ContentPage
{
	public VimsControl()
	{
		InitializeComponent();

        AdBanner.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.ControlAdsAndroid : Constants.ControlAdsiOS;
        AdBanner.AdLoaded += (s, e) => Console.WriteLine($"[Ads] VimsControl: ad loaded ({AdBanner.AdUnitId})");
        AdBanner.AdFailedToLoad += (s, error) => Console.WriteLine($"[Ads] VimsControl: ad failed to load ({AdBanner.AdUnitId}): {error}");
    }
}