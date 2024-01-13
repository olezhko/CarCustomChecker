using CarCustomChecker.Models;
using CarCustomChecker.ViewModels;

namespace CarCustomChecker;
public partial class MainPage : ContentPage
{
	MainPageViewModel viewModel;
	public MainPage()
	{
		InitializeComponent();
		viewModel = BindingContext as MainPageViewModel;

		AdMob.AdUnitId = DeviceInfo.Platform == DevicePlatform.Android? Constants.SearchAdsAndroid: Constants.SearchAdsiOS;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}

	private async void Picker_OnSelectedIndexChanged(object? sender, EventArgs e)
	{
		if (IsLoaded)
		{
			await viewModel.LoadAsync();
		}
	}

	private async void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (IsLoaded && e.NewTextValue.Length >= 4)
		{
			await viewModel.LoadAsync();
		}
	}
}