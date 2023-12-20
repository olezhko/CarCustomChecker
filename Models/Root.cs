using Newtonsoft.Json;

namespace CarCustomChecker.Models;

public record Root(
	[property: JsonProperty("current_page")] int? CurrentPage,
	[property: JsonProperty("data")] IReadOnlyList<CarItem> Data,
	[property: JsonProperty("first_page_url")] string FirstPageUrl,
	[property: JsonProperty("from")] int? From,
	[property: JsonProperty("last_page")] int? LastPage,
	[property: JsonProperty("last_page_url")] string LastPageUrl,
	[property: JsonProperty("links")] IReadOnlyList<Link> Links,
	[property: JsonProperty("next_page_url")] string NextPageUrl,
	[property: JsonProperty("path")] string Path,
	[property: JsonProperty("per_page")] int? PerPage,
	[property: JsonProperty("prev_page_url")] object PrevPageUrl,
	[property: JsonProperty("to")] int? To,
	[property: JsonProperty("total")] int? Total
);