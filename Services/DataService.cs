using CarCustomChecker.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CarCustomChecker.Services;
internal class DataService : IDataService
{
	private const string serviceUrl = "https://api.mon.declarant.by/api";
	private static RestClient InitClient(string url)
	{
		var options = new RestClientOptions(serviceUrl + url)
		{
			ThrowOnAnyError = true,
			MaxTimeout = -1,
		};

		var client = new RestClient(options);
		return client;
	}

	private static RestRequest InitRequest(Method method = Method.Get, object body = null)
	{
		var request = new RestRequest();
		request.Method = method;
		if (body != null)
		{
			request.AddParameter("application/json", body, ParameterType.RequestBody);
		}

		return request;
	}

	public async Task<Root> GetCarItems(int page, int perPage, string ptoCode = "", string search = "")
	{
		var body = new
		{
			page,
			perPage,
			ptoCode,
			search
		};
		var bodyString = JsonConvert.SerializeObject(body);
		var request = InitRequest(Method.Post, bodyString);

		try
		{
			var response = await InitClient($"/vehicle")
				.ExecuteAsync(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var result = JsonConvert.DeserializeObject<Root>(response.Content);
				return result;
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}


		return null;
	}
}