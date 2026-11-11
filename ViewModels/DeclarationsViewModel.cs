using CarCustomChecker.Models;
using CarCustomChecker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCustomChecker.ViewModels;

public class DeclarationsViewModel : BaseViewModel
{
	// The official site only searches once the number has at least 5 characters.
	private const int MinRegNumberLength = 5;

	private readonly IDataService dataService = DependencyService.Get<IDataService>();
	private IReadOnlyList<CustomsPlace> places = [];
	private CustomsPlace? selectedPlace;
	private string regNumberToAdd = string.Empty;
	private bool placesLoaded;

	public DeclarationsViewModel()
	{
		Items = new ObservableCollection<TrackedDeclarationViewModel>(
			SettingsService.LoadDeclarations().Select(d => new TrackedDeclarationViewModel(d)));

		AddCommand = new Command(async () => await AddAsync());
		DeleteCommand = new Command<TrackedDeclarationViewModel>(Delete);
		RefreshCommand = new Command(async () =>
		{
			await UpdateAllAsync();
			IsRefreshing = false;
		});
	}

	public async Task LoadAsync()
	{
		if (!placesLoaded)
		{
			await LoadPlacesAsync();
		}

		await UpdateAllAsync();
	}

	private async Task LoadPlacesAsync()
	{
		var offices = await dataService.GetCustomsOffices(CustomsPlaceKind.Vpto);
		if (offices == null)
		{
			return;
		}

		placesLoaded = true;
		Places = offices.OrderBy(o => o.SortIndex).SelectMany(o => o.Places ?? []).ToList();
	}

	private async Task AddAsync()
	{
		var number = RegNumberToAdd?.Trim() ?? string.Empty;
		if (SelectedPlace == null || number.Length < MinRegNumberLength)
		{
			await Shell.Current.DisplayAlertAsync("Декларации",
				$"Выберите ВПТО и введите регистрационный номер (от {MinRegNumberLength} символов).", "OK");
			return;
		}

		if (Items.Any(i => i.Declaration.PlaceId == SelectedPlace.Id
			&& string.Equals(i.RegNumber, number, StringComparison.OrdinalIgnoreCase)))
		{
			RegNumberToAdd = string.Empty;
			return;
		}

		var item = new TrackedDeclarationViewModel(new TrackedDeclaration(SelectedPlace.Id, SelectedPlace.Name, number));
		Items.Add(item);
		Save();
		RegNumberToAdd = string.Empty;

		await UpdateAsync(item);
	}

	private void Delete(TrackedDeclarationViewModel item)
	{
		if (item != null && Items.Remove(item))
		{
			Save();
		}
	}

	private void Save() => SettingsService.SaveDeclarations(Items.Select(i => i.Declaration));

	private async Task UpdateAllAsync()
	{
		if (IsBusy)
		{
			return;
		}

		IsBusy = true;
		try
		{
			await Task.WhenAll(Items.ToList().Select(UpdateAsync));
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task UpdateAsync(TrackedDeclarationViewModel item)
	{
		try
		{
			var result = await dataService.SearchVpto(item.Declaration.PlaceId, item.RegNumber, 1);
			if (result?.Data == null)
			{
				return; // request failed: keep the last known status
			}

			// The API does a partial match; only accept the exact number so a typo or a short
			// number never shows another declaration's status.
			var match = result.Data.FirstOrDefault(d =>
				string.Equals(d.RegNumber?.Trim(), item.RegNumber, StringComparison.OrdinalIgnoreCase));

			item.Result = match;
			item.NotFound = match == null;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}

	#region Properties

	public IReadOnlyList<CustomsPlace> Places
	{
		get => places;
		private set => SetProperty(ref places, value);
	}

	public CustomsPlace? SelectedPlace
	{
		get => selectedPlace;
		set => SetProperty(ref selectedPlace, value);
	}

	public string RegNumberToAdd
	{
		get => regNumberToAdd;
		set => SetProperty(ref regNumberToAdd, value);
	}

	public ObservableCollection<TrackedDeclarationViewModel> Items { get; }

	public ICommand AddCommand { get; }

	public ICommand DeleteCommand { get; }

	public ICommand RefreshCommand { get; }

	#endregion
}
