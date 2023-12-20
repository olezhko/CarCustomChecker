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
}