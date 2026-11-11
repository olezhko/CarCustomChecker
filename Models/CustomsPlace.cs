using Newtonsoft.Json;

namespace CarCustomChecker.Models;

/// <summary>Kinds accepted by POST /api/customs-place (place_kind).</summary>
public static class CustomsPlaceKind
{
	public const int Vpto = 1;
	public const int Svh = 2;
}

/// <summary>Customs office from POST /api/customs-place, with its ВПТО or СВХ places.</summary>
public record CustomsOffice(
	[property: JsonProperty("id")] string Id,
	[property: JsonProperty("disp_name")] string Name,
	[property: JsonProperty("sortIndex")] int? SortIndex,
	[property: JsonProperty("places")] IReadOnlyList<CustomsPlace> Places
);

/// <summary>A ВПТО or СВХ place, from the office list or GET /api/customs-place/{id}.</summary>
public record CustomsPlace(
	[property: JsonProperty("id")] int Id,
	[property: JsonProperty("place_code")] string PlaceCode,
	[property: JsonProperty("disp_name")] string Name,
	[property: JsonProperty("disp_address")] string Address,
	[property: JsonProperty("disp_work_time")] string WorkTime,
	[property: JsonProperty("disp_phones")] string Phones,
	[property: JsonProperty("ptocode")] string PtoCode,
	[property: JsonProperty("isActive")] int? IsActive,
	[property: JsonProperty("updated")] string Updated
);

public class CustomsOfficeGroup : List<CustomsPlace>
{
	public CustomsOfficeGroup(string name, IEnumerable<CustomsPlace> places) : base(places)
	{
		Name = name;
	}

	public string Name { get; }
}

/// <summary>Customs declaration from POST /api/vpto. Labels match mon.declarant.by/#/vpto.</summary>
public class VptoDeclaration
{
	[JsonProperty("id")] public long? Id { get; set; }
	[JsonProperty("place_id")] public int? PlaceId { get; set; }
	[JsonProperty("regNumber")] public string? RegNumber { get; set; }
	[JsonProperty("regDate")] public string? RegDate { get; set; }
	[JsonProperty("declType")] public string? DeclType { get; set; }
	[JsonProperty("declMode")] public string? DeclMode { get; set; }
	[JsonProperty("declStatus")] public string? DeclStatus { get; set; }
	[JsonProperty("accNumber")] public string? AccNumber { get; set; }
	[JsonProperty("accDate")] public string? AccDate { get; set; }
	[JsonProperty("requirementText")] public string? RequirementText { get; set; }
	[JsonProperty("refusalReason")] public string? RefusalReason { get; set; }
	[JsonProperty("updated")] public string? Updated { get; set; }

	public static string GetStatusText(string? status) => status switch
	{
		"1" => "Зарегистрирован",
		"2" => "Отказано в выпуске",
		"3" => "Выпуск разрешен",
		"4" => "Выставлено требование",
		_ => string.Empty
	};

	public static Color GetStatusColor(string? status) => status switch
	{
		"1" => Colors.SteelBlue,
		"2" => Colors.Red,
		"3" => Colors.Green,
		"4" => Colors.Orange,
		_ => Colors.Transparent
	};
}

/// <summary>Entry in a temporary storage warehouse, from POST /api/svh. Labels match mon.declarant.by/#/svh.</summary>
public class SvhEntry
{
	[JsonProperty("id")] public long? Id { get; set; }
	[JsonProperty("numItem")] public string? NumItem { get; set; }
	[JsonProperty("dateIn")] public string? DateIn { get; set; }
	[JsonProperty("dateOut")] public string? DateOut { get; set; }
	[JsonProperty("transport")] public string? Transport { get; set; }
	[JsonProperty("regNum")] public string? RegNum { get; set; }
	[JsonProperty("vhNum")] public string? VhNum { get; set; }
	[JsonProperty("prevNum")] public string? PrevNum { get; set; }
	[JsonProperty("documents")] public string? Documents { get; set; }
	[JsonProperty("updated")] public string? Updated { get; set; }

	/// <summary>"type number;type number" → one document per line, as the official site shows it.</summary>
	public string DocumentsText => string.Join(Environment.NewLine,
		(Documents ?? string.Empty).Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
}
