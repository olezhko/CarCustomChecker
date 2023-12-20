using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

internal class MainPageViewModel : BaseViewModel
{
	private string vinSearch = string.Empty;
	private BorderPoint selectedPto;
	private int page = 1;
	private int totalItems = 0;
	private IDataService dataService = DependencyService.Get<IDataService>();
	public MainPageViewModel()
	{
		PtoCollection = new ObservableCollection<BorderPoint>
		{
			new BorderPoint("Все ПТО", string.Empty),
			new BorderPoint("06649 - Белтаможсервис-ТЛЦ", Constants.BeltamozhServiceTLC),
			new BorderPoint("06650 - Минск-Белтаможсервис-2", Constants.MinskBeltamozhService2)
		};

		CarItems = new ObservableCollection<CarItem>();

		SelectedPto = PtoCollection.First();
	}

	public async Task LoadAsync()
	{
		CarItems.Clear();
		Page = 1;
		await SearchAsync();
	}

	private async void LoadNewItems()
	{
		await SearchAsync();
	}

	private async Task SearchAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var result = await dataService.GetCarItems(page, 20, SelectedPto.PTO, VinSearch);
			foreach (var item in result.Data)
			{
				CarItems.Add(item);
			}

			TotalItems = result.Total.Value;
			OnPropertyChanged(nameof(CarItems));
			Page++;
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

	#region Properties

	public int TotalItems
	{
		get => totalItems;
		set => SetProperty(ref totalItems, value);
	}

	public int Page
	{
		get => page;
		set => SetProperty(ref page, value);
	}

	public string VinSearch
	{
		get => vinSearch;
		set => SetProperty(ref vinSearch, value);
	}

	public BorderPoint SelectedPto
	{
		get => selectedPto;
		set => SetProperty(ref selectedPto, value);
	}

	public ObservableCollection<BorderPoint> PtoCollection { get; set; }

	public ObservableCollection<CarItem> CarItems { get; set; }

	public ICommand LoadNewItemsCommand => new Command(LoadNewItems);

	#endregion
}