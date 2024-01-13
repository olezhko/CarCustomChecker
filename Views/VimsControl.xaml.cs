using CarCustomChecker.Models;

namespace CarCustomChecker.Views;

public partial class VimsControl : ContentPage
{
	public VimsControl()
	{
		InitializeComponent();

        AdMob.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.ControlAdsAndroid : Constants.ControlAdsiOS;
    }
}