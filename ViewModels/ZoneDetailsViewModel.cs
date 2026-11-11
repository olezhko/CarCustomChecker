using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class ZoneDetailsViewModel : BaseViewModel
{
	private readonly IDataService dataService = DependencyService.Get<IDataService>();
	private string checkpointId = string.Empty;
	private ZoneMonitoring? monitoring;
	private ZoneInfo? info;
	private ZoneVehicleTypeOption selectedVehicleType;
	private string search = string.Empty;
	private int totalItems;

	public ZoneDetailsViewModel()
	{
		VehicleTypes =
		[
			new ZoneVehicleTypeOption("Легковые", ZoneVehicleType.Car),
			new ZoneVehicleTypeOption("Грузовые", ZoneVehicleType.Truck),
			new ZoneVehicleTypeOption("Автобусы", ZoneVehicleType.Bus),
			new ZoneVehicleTypeOption("Мотоциклы", ZoneVehicleType.Motorcycle),
		];
		selectedVehicleType = VehicleTypes[0];
		Vehicles = new ObservableCollection<ZoneQueueEntry>();
		RefreshCommand = new Command(async () =>
		{
			await LoadDataAsync();
			IsRefreshing = false;
		});
	}

	public async Task LoadAsync(string id)
	{
		checkpointId = id;
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		if (string.IsNullOrEmpty(checkpointId))
		{
			return;
		}

		IsBusy = true;
		try
		{
			var result = await dataService.GetZoneMonitoring(checkpointId);
			if (result != null)
			{
				monitoring = result;
				Info = result.Info;
				ApplyFilter();
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

	private void ApplyFilter()
	{
		Vehicles.Clear();
		if (monitoring == null)
		{
			TotalItems = 0;
			return;
		}

		var queue = monitoring.GetQueue(SelectedVehicleType.Type).ToList();
		TotalItems = queue.Count;

		var text = Search?.Trim() ?? string.Empty;
		foreach (var entry in queue.Where(e => text.Length == 0
			|| (e.RegNum?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false)))
		{
			Vehicles.Add(entry);
		}
	}

	#region Properties

	public ZoneInfo? Info
	{
		get => info;
		set => SetProperty(ref info, value);
	}

	public IReadOnlyList<ZoneVehicleTypeOption> VehicleTypes { get; }

	public ZoneVehicleTypeOption SelectedVehicleType
	{
		get => selectedVehicleType;
		set => SetProperty(ref selectedVehicleType, value, onChanged: ApplyFilter);
	}

	public string Search
	{
		get => search;
		set => SetProperty(ref search, value, onChanged: ApplyFilter);
	}

	public int TotalItems
	{
		get => totalItems;
		set => SetProperty(ref totalItems, value);
	}

	public ObservableCollection<ZoneQueueEntry> Vehicles { get; }

	public ICommand RefreshCommand { get; }

	#endregion
}
