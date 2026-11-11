using CarCustomChecker.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
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

	public Task<IReadOnlyList<CustomsOffice>> GetCustomsOffices(int placeKind)
	{
		var body = JsonConvert.SerializeObject(new { place_kind = placeKind });
		return ExecuteAsync<IReadOnlyList<CustomsOffice>>("/customs-place", InitRequest(Method.Post, body));
	}

	public Task<CustomsPlace> GetCustomsPlace(int id) =>
		ExecuteAsync<CustomsPlace>($"/customs-place/{id}", InitRequest(Method.Get));

	public Task<PagedResult<VptoDeclaration>> SearchVpto(int placeId, string regNumber, int page)
	{
		var body = JsonConvert.SerializeObject(new
		{
			place_id = placeId,
			declStatus = string.Empty,
			docTypes = Array.Empty<string>(),
			regNumber,
			page
		});
		return ExecuteAsync<PagedResult<VptoDeclaration>>("/vpto", InitRequest(Method.Post, body));
	}

	public Task<PagedResult<SvhEntry>> SearchSvh(int placeId, string regNum, string transport, string numItem, int page)
	{
		var body = JsonConvert.SerializeObject(new
		{
			place_id = placeId,
			numItem,
			dateIn = string.Empty,
			transport,
			regNum,
			page
		});
		return ExecuteAsync<PagedResult<SvhEntry>>("/svh", InitRequest(Method.Post, body));
	}

	public Task<IReadOnlyList<RptoOffice>> GetRptoOffices() =>
		ExecuteAsync<IReadOnlyList<RptoOffice>>("/rpto", InitRequest(Method.Get));

	public Task<RptoPlace> GetRptoPlace(int id) =>
		ExecuteAsync<RptoPlace>($"/rpto/{id}", InitRequest(Method.Get));

	public Task<PagedResult<RptoVehicle>> GetRptoVehicles(int id, string search, int page)
	{
		var body = JsonConvert.SerializeObject(new { id, search, page });
		return ExecuteAsync<PagedResult<RptoVehicle>>("/rpto/vehicles", InitRequest(Method.Post, body));
	}

	// mon.declarant.by calls this endpoint with token=test; the token used for /info/checkpoint returns empty queues here.
	public Task<ZoneMonitoring> GetZoneMonitoring(string checkpointId) =>
		ExecuteAsync<ZoneMonitoring>(
			new RestClient(new RestClientOptions($"https://belarusborder.by/info/monitoring-new?token=test&checkpointId={Uri.EscapeDataString(checkpointId)}")
			{
				ThrowOnAnyError = true,
			}),
			InitRequest(Method.Get));

	private static Task<T> ExecuteAsync<T>(string url, RestRequest request) where T : class =>
		ExecuteAsync<T>(InitClient(url), request);

	private static async Task<T> ExecuteAsync<T>(RestClient client, RestRequest request) where T : class
	{
		try
		{
			var response = await client.ExecuteAsync(request);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return JsonConvert.DeserializeObject<T>(response.Content);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}

		return null;
	}

    public async Task<ElectronicQueueResult> GetElectronicQueueResult()
    {
        var request = InitRequest(Method.Get);

        try
        {
            var options = new RestClientOptions("https://belarusborder.by/info/checkpoint?token=bts47d5f-6420-4f74-8f78-42e8e4370cc4")
            {
                ThrowOnAnyError = true,
            };

            var client = new RestClient(options);
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<ElectronicQueueResult>(response.Content);
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