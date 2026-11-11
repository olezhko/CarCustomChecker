using CarCustomChecker.Models;
using Newtonsoft.Json;

namespace CarCustomChecker.Services;

public class SettingsService
{
	private static string Vims
	{
		get => Preferences.Get(nameof(Vims), string.Empty);
		set => Preferences.Set(nameof(Vims), value);
	}

	public static List<string> LoadVinList()
	{
		var array = Vims.Split(',');
		return array.ToList();
	}

	public static void Save(IEnumerable<string> array)
	{
		var vimString = string.Join(',', array);
		Vims = vimString;
	}

	private static string Declarations
	{
		get => Preferences.Get(nameof(Declarations), string.Empty);
		set => Preferences.Set(nameof(Declarations), value);
	}

	public static List<TrackedDeclaration> LoadDeclarations()
	{
		try
		{
			return JsonConvert.DeserializeObject<List<TrackedDeclaration>>(Declarations) ?? [];
		}
		catch (JsonException)
		{
			return [];
		}
	}

	public static void SaveDeclarations(IEnumerable<TrackedDeclaration> declarations)
	{
		Declarations = JsonConvert.SerializeObject(declarations);
	}
}
