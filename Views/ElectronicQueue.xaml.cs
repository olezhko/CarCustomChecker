using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class ElectronicQueue : ContentPage
{
	public ElectronicQueue()
	{
		InitializeComponent();
        AdMob.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android ? Constants.ElectronicQueueAdsAndroid : Constants.ElectronicQueueAdsiOS;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as ElectronicQueueViewModel;
        await viewModel.LoadDataAsync();
    }
}