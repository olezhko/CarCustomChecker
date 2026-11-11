using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class Declarations : ContentPage
{
	public Declarations()
	{
		InitializeComponent();

		AdBanner.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.DeclarationsAdsAndroid : Constants.DeclarationsAdsiOS;
		AdBanner.AdLoaded += (s, e) => Console.WriteLine($"[Ads] Declarations: ad loaded ({AdBanner.AdUnitId})");
		AdBanner.AdFailedToLoad += (s, error) => Console.WriteLine($"[Ads] Declarations: ad failed to load ({AdBanner.AdUnitId}): {error}");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is DeclarationsViewModel viewModel)
		{
			await viewModel.LoadAsync();
		}
	}
}
