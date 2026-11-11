using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class RptoDetailsViewModel : BaseViewModel
{
	private readonly IDataService dataService = DependencyService.Get<IDataService>();
	private int placeId;
	private RptoPlace? place;
	private string search = string.Empty;
	private int page = 1;
	private int lastPage = 1;
	private int totalItems;

	// Bumped on every reload so responses from an older reload are dropped.
	private int generation;
	private bool isLoading;

	public RptoDetailsViewModel()
	{
		Vehicles = new ObservableCollection<RptoVehicle>();
		RefreshCommand = new Command(async () => await ReloadQueueAsync());
		LoadMoreCommand = new Command(async () => await LoadMoreAsync());
	}

	public async Task LoadAsync(int id)
	{
		placeId = id;
		var details = await dataService.GetRptoPlace(id);
		if (details != null)
		{
			Place = details;
		}

		await ReloadQueueAsync();
	}

	public async Task ReloadQueueAsync()
	{
		generation++;
		isLoading = false;
		Vehicles.Clear();
		page = 1;
		lastPage = 1;

		await LoadPageAsync();
		IsRefreshing = false;
	}

	private async Task LoadMoreAsync()
	{
		if (isLoading || page > lastPage || Vehicles.Count == 0)
		{
			return;
		}

		await LoadPageAsync();
	}

	private async Task LoadPageAsync()
	{
		if (placeId == 0)
		{
			return;
		}

		var requestGeneration = generation;
		isLoading = true;
		IsBusy = true;
		try
		{
			var result = await dataService.GetRptoVehicles(placeId, Search?.Trim() ?? string.Empty, page);
			if (requestGeneration != generation || result?.Data == null)
			{
				return;
			}

			foreach (var vehicle in result.Data)
			{
				Vehicles.Add(vehicle);
			}

			TotalItems = result.Total ?? Vehicles.Count;
			lastPage = result.LastPage ?? page;
			page++;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			if (requestGeneration == generation)
			{
				isLoading = false;
				IsBusy = false;
			}
		}
	}

	#region Properties

	public RptoPlace? Place
	{
		get => place;
		set => SetProperty(ref place, value);
	}

	public string Search
	{
		get => search;
		set => SetProperty(ref search, value);
	}

	public int TotalItems
	{
		get => totalItems;
		set => SetProperty(ref totalItems, value);
	}

	public ObservableCollection<RptoVehicle> Vehicles { get; }

	public ICommand RefreshCommand { get; }

	public ICommand LoadMoreCommand { get; }

	#endregion
}
