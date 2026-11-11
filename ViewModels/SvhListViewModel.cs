using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class SvhListViewModel : BaseViewModel
{
	private readonly IDataService dataService = DependencyService.Get<IDataService>();

	public SvhListViewModel()
	{
		Offices = new ObservableCollection<CustomsOfficeGroup>();
		RefreshCommand = new Command(async () =>
		{
			await LoadDataAsync();
			IsRefreshing = false;
		});
		OpenPlaceCommand = new Command<CustomsPlace>(async place => await OpenPlaceAsync(place));
	}

	public async Task LoadDataAsync()
	{
		IsBusy = true;
		try
		{
			var offices = await dataService.GetCustomsOffices(CustomsPlaceKind.Svh);
			if (offices != null)
			{
				Offices = new ObservableCollection<CustomsOfficeGroup>(offices
					.OrderBy(o => o.SortIndex)
					.Where(o => o.Places is { Count: > 0 })
					.Select(o => new CustomsOfficeGroup(o.Name, o.Places)));
				OnPropertyChanged(nameof(Offices));
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			IsBusy = false;
		}
	}

	private static async Task OpenPlaceAsync(CustomsPlace place)
	{
		if (place == null)
		{
			return;
		}

		await Shell.Current.GoToAsync($"{nameof(Views.SvhDetails)}?id={place.Id}");
	}

	#region Properties

	public ObservableCollection<CustomsOfficeGroup> Offices { get; set; }

	public ICommand RefreshCommand { get; }

	public ICommand OpenPlaceCommand { get; }

	#endregion
}
