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