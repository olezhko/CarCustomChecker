using Newtonsoft.Json;

namespace CarCustomChecker.Models;
public record CarItem(
	[property: JsonProperty("id")] int? Id,
	[property: JsonProperty("vin")] string Vin,
	[property: JsonProperty("description")] string Description,
	[property: JsonProperty("dateIn")] string DateIn,
	[property: JsonProperty("timeIn")] string TimeIn,
	[property: JsonProperty("updated")] string Updated,
	[property: JsonProperty("DOCID")] int? DOCID,
	[property: JsonProperty("ptoCode")] string PtoCode
);