using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class RptoList : ContentPage
{
	private bool loaded;

	public RptoList()
	{
		InitializeComponent();

		AdBanner.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.RptoAdsAndroid : Constants.RptoAdsiOS;
		AdBanner.AdLoaded += (s, e) => Console.WriteLine($"[Ads] RptoList: ad loaded ({AdBanner.AdUnitId})");
		AdBanner.AdFailedToLoad += (s, error) => Console.WriteLine($"[Ads] RptoList: ad failed to load ({AdBanner.AdUnitId}): {error}");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// The office list rarely changes; load once and let pull-to-refresh update it.
		if (!loaded && BindingContext is RptoListViewModel viewModel)
		{
			loaded = true;
			await viewModel.LoadDataAsync();
		}
	}
}
