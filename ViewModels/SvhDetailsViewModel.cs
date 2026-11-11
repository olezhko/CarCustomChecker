using CarCustomChecker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class SvhDetailsViewModel : BaseViewModel
{
	private readonly IDataService dataService = DependencyService.Get<IDataService>();
	private int placeId;
	private CustomsPlace? place;
	private string regNum = string.Empty;
	private string transport = string.Empty;
	private string numItem = string.Empty;
	private int page = 1;
	private int lastPage = 1;
	private int totalItems;

	// Bumped on every reload so responses from an older search are dropped.
	private int generation;
	private bool isLoading;

	public SvhDetailsViewModel()
	{
		Entries = new ObservableCollection<SvhEntry>();
		SearchCommand = new Command(async () => await ReloadAsync());
		ClearCommand = new Command(async () =>
		{
			RegNum = Transport = NumItem = string.Empty;
			await ReloadAsync();
		});
		RefreshCommand = new Command(async () =>
		{
			await ReloadAsync();
			IsRefreshing = false;
		});
		LoadMoreCommand = new Command(async () => await LoadMoreAsync());
	}

	public async Task LoadAsync(int id)
	{
		placeId = id;
		var details = await dataService.GetCustomsPlace(id);
		if (details != null)
		{
			Place = details;
		}

		await ReloadAsync();
	}

	private async Task ReloadAsync()
	{
		generation++;
		isLoading = false;
		Entries.Clear();
		page = 1;
		lastPage = 1;
		await LoadPageAsync();
	}

	private async Task LoadMoreAsync()
	{
		if (isLoading || page > lastPage || Entries.Count == 0)
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
			var result = await dataService.SearchSvh(placeId, RegNum.Trim(), Transport.Trim(), NumItem.Trim(), page);
			if (requestGeneration != generation || result?.Data == null)
			{
				return;
			}

			foreach (var entry in result.Data)
			{
				Entries.Add(entry);
			}

			TotalItems = result.Total ?? Entries.Count;
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

	public CustomsPlace? Place
	{
		get => place;
		set => SetProperty(ref place, value);
	}

	public string RegNum
	{
		get => regNum;
		set => SetProperty(ref regNum, value ?? string.Empty);
	}

	public string Transport
	{
		get => transport;
		set => SetProperty(ref transport, value ?? string.Empty);
	}

	public string NumItem
	{
		get => numItem;
		set => SetProperty(ref numItem, value ?? string.Empty);
	}

	public int TotalItems
	{
		get => totalItems;
		set => SetProperty(ref totalItems, value);
	}

	public ObservableCollection<SvhEntry> Entries { get; }

	public ICommand SearchCommand { get; }

	public ICommand ClearCommand { get; }

	public ICommand RefreshCommand { get; }

	public ICommand LoadMoreCommand { get; }

	#endregion
}
