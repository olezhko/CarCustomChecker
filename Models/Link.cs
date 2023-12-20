
using Newtonsoft.Json;

namespace CarCustomChecker.Models;

public record Link(
	[property: JsonProperty("url")] string Url,
	[property: JsonProperty("label")] string Label,
	[property: JsonProperty("active")] bool? Active
);
