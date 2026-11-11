using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class ElectronicQueueViewModel : BaseViewModel
{
	private IDataService dataService = DependencyService.Get<IDataService>();

	public ElectronicQueueViewModel()
	{
        Items = new ObservableCollection<ElectronicQueue>();
		RefreshCommand = new Command(Refresh);
		OpenZoneCommand = new Command<ElectronicQueue>(async zone => await OpenZoneAsync(zone));
    }

	private static async Task OpenZoneAsync(ElectronicQueue zone)
	{
		if (string.IsNullOrEmpty(zone?.Id))
		{
			return;
		}

		await Shell.Current.GoToAsync($"{nameof(Views.ZoneDetails)}?id={Uri.EscapeDataString(zone.Id)}");
	}

	private async void Refresh()
	{
		await LoadDataAsync();
		IsRefreshing = false;
	}

	public async Task LoadDataAsync()
	{
		IsBusy = true;
		try
		{
            ElectronicQueueResult data = await dataService.GetElectronicQueueResult();
            Items = new ObservableCollection<ElectronicQueue>(data.Result);
            OnPropertyChanged(nameof(Items));
        }
        catch (Exception)
		{
		}

        IsBusy = false;
	}

    #region Properties
    
	public ObservableCollection<ElectronicQueue> Items { get; set; }
    
	public ICommand RefreshCommand { get; set; }

	public ICommand OpenZoneCommand { get; }
	#endregion
}