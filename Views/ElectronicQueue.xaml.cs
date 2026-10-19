using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class ElectronicQueue : ContentPage
{
	public ElectronicQueue()
	{
		InitializeComponent();
        AdBanner.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.ElectronicQueueAdsAndroid : Constants.ElectronicQueueAdsiOS;
        AdBanner.AdLoaded += (s, e) => Console.WriteLine($"[Ads] ElectronicQueue: ad loaded ({AdBanner.AdUnitId})");
        AdBanner.AdFailedToLoad += (s, error) => Console.WriteLine($"[Ads] ElectronicQueue: ad failed to load ({AdBanner.AdUnitId}): {error}");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as ElectronicQueueViewModel;
        await viewModel.LoadDataAsync();
    }
}