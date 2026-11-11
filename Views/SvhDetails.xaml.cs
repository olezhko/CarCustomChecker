using CarCustomChecker.ViewModels;

namespace CarCustomChecker.Views;

public partial class SvhDetails : ContentPage, IQueryAttributable
{
	public SvhDetails()
	{
		InitializeComponent();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("id", out var value)
			&& int.TryParse(value?.ToString(), out var id)
			&& BindingContext is SvhDetailsViewModel viewModel)
		{
			await viewModel.LoadAsync(id);
		}
	}
}
