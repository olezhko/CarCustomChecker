using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class SvhList : ContentPage
{
	private bool loaded;

	public SvhList()
	{
		InitializeComponent();

		AdBanner.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.SvhAdsAndroid : Constants.SvhAdsiOS;
		AdBanner.AdLoaded += (s, e) => Console.WriteLine($"[Ads] SvhList: ad loaded ({AdBanner.AdUnitId})");
		AdBanner.AdFailedToLoad += (s, error) => Console.WriteLine($"[Ads] SvhList: ad failed to load ({AdBanner.AdUnitId}): {error}");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// The warehouse list rarely changes; load once and let pull-to-refresh update it.
		if (!loaded && BindingContext is SvhListViewModel viewModel)
		{
			loaded = true;
			await viewModel.LoadDataAsync();
		}
	}
}
