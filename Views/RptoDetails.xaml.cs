using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class RptoDetails : ContentPage, IQueryAttributable
{
	public RptoDetails()
	{
		InitializeComponent();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("id", out var value)
			&& int.TryParse(value?.ToString(), out var id)
			&& BindingContext is RptoDetailsViewModel viewModel)
		{
			await viewModel.LoadAsync(id);
		}
	}
}
