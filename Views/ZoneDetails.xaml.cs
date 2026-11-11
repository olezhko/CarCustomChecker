using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class ZoneDetails : ContentPage, IQueryAttributable
{
	public ZoneDetails()
	{
		InitializeComponent();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("id", out var value)
			&& value?.ToString() is { Length: > 0 } id
			&& BindingContext is ZoneDetailsViewModel viewModel)
		{
			await viewModel.LoadAsync(id);
		}
	}
}
