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
    }

	private async void Refresh()
	{
		await LoadDataAsync();
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
	#endregion
}