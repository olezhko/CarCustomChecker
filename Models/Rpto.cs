using Newtonsoft.Json;

namespace CarCustomChecker.Models;

/// <summary>Customs office from GET /api/rpto, with its regional clearance points.</summary>
public record RptoOffice(
	[property: JsonProperty("customs_id")] string CustomsId,
	[property: JsonProperty("name")] string Name,
	[property: JsonProperty("places")] IReadOnlyList<RptoPlace> Places
);

/// <summary>A regional clearance point (РПТО), from the office list or GET /api/rpto/{id}.</summary>
public record RptoPlace(
	[property: JsonProperty("id")] int Id,
	[property: JsonProperty("name")] string Name,
	[property: JsonProperty("address")] string Address,
	[property: JsonProperty("timework")] string TimeWork,
	[property: JsonProperty("phones")] string Phones,
	[property: JsonProperty("customs_id")] string CustomsId,
	[property: JsonProperty("updated")] string Updated
);

/// <summary>A vehicle in an РПТО queue, from POST /api/rpto/vehicles.</summary>
public record RptoVehicle(
	[property: JsonProperty("number")] int? Number,
	[property: JsonProperty("tsnumber")] string TsNumber,
	[property: JsonProperty("rdatetime")] string RegisteredAt,
	[property: JsonProperty("tdatetime")] string AcceptedAt,
	[property: JsonProperty("edatetime")] string EndedAt,
	[property: JsonProperty("status")] string Status
);

/// <summary>Laravel-style paginated response.</summary>
public record PagedResult<T>(
	[property: JsonProperty("current_page")] int? CurrentPage,
	[property: JsonProperty("data")] IReadOnlyList<T> Data,
	[property: JsonProperty("last_page")] int? LastPage,
	[property: JsonProperty("total")] int? Total
);

/// <summary>Group of РПТО points under one customs office, for a grouped CollectionView.</summary>
public class RptoOfficeGroup : List<RptoPlace>
{
	public RptoOfficeGroup(string name, IEnumerable<RptoPlace> places) : base(places)
	{
		Name = name;
	}

	public string Name { get; }
}
