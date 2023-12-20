using CarCustomChecker.Models;
using CarCustomChecker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

class VimsControlViewModel : BaseViewModel
{
	private IDataService dataService = DependencyService.Get<IDataService>();
	private string _vinToAdd;

	public VimsControlViewModel()
	{
		var vins = SettingsService.LoadVinList();
		CarItems = new ObservableCollection<CarItemViewModel>();

		foreach (var vin in vins)
		{
			if (!string.IsNullOrEmpty(vin))
			{
				AddVin(vin);
			}
		}
	}

	private void AddVin(string vin)
	{
		if (CarItems.All(item => item.Vin.Value != vin))
		{
			CarItems.Add(new CarItemViewModel()
			{
				Vin = new FieldValueViewModel<string>(vin),
				DateIn = new FieldValueViewModel<string>(Constants.NoData),
				PtoCode = new FieldValueViewModel<string>(Constants.NoData),
				TimeIn = new FieldValueViewModel<string>(Constants.NoData),
			});

			SaveVins();
			VinToAdd = string.Empty;
		}
	}

	private void DeleteVin(string vin)
	{
		CarItems.Remove(CarItems.First(item => item.Vin.Value == vin));
		SaveVins();
	}

	private void SaveVins()
	{
		var items = CarItems.Select(item => item.Vin.Value);
		SettingsService.Save(items);
	}

	private void UpdateInfo()
	{
		Parallel.ForEach(CarItems, async (item) =>
		{
			var result = await dataService.GetCarItems(1, 1,string.Empty,item.Vin.Value);
			if (result != null)
			{
				var car = result.Data.FirstOrDefault();

				item.DateIn.Value = car != null? car.DateIn: Constants.NotFound;
				item.TimeIn.Value = car != null? car.TimeIn : Constants.NotFound;
				item.PtoCode.Value = car != null? car.PtoCode : Constants.NotFound;
			}
		});
	}

	#region Properties

	public string VinToAdd
	{
		get => _vinToAdd;
		set => SetProperty(ref _vinToAdd, value);
	}

	public ICommand AddVinCommand => new Command<string>(AddVin);
	public ICommand UpdateInfoCommand => new Command(UpdateInfo);
	public ICommand DeleteVinCommand => new Command<string>(DeleteVin);
	public ObservableCollection<CarItemViewModel> CarItems { get; set; }
	#endregion
}