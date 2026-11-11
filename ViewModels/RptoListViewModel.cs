using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class RptoListViewModel : BaseViewModel
{
	private readonly IDataService dataService = DependencyService.Get<IDataService>();

	public RptoListViewModel()
	{
		Offices = new ObservableCollection<RptoOfficeGroup>();
		RefreshCommand = new Command(async () =>
		{
			await LoadDataAsync();
			IsRefreshing = false;
		});
		OpenPlaceCommand = new Command<RptoPlace>(async place => await OpenPlaceAsync(place));
	}

	public async Task LoadDataAsync()
	{
		IsBusy = true;
		try
		{
			var offices = await dataService.GetRptoOffices();
			if (offices != null)
			{
				Offices = new ObservableCollection<RptoOfficeGroup>(
					offices.Select(o => new RptoOfficeGroup(o.Name, o.Places ?? [])));
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

	private static async Task OpenPlaceAsync(RptoPlace place)
	{
		if (place == null)
		{
			return;
		}

		await Shell.Current.GoToAsync($"{nameof(Views.RptoDetails)}?id={place.Id}");
	}

	#region Properties

	public ObservableCollection<RptoOfficeGroup> Offices { get; set; }

	public ICommand RefreshCommand { get; }

	public ICommand OpenPlaceCommand { get; }

	#endregion
}
