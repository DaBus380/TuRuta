using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Client.Stops
{
    class StopsClient
    {
		public StopsClient(string baseAddress = null)
		{
			var address = baseAddress ?? "http://localhost:56340/";
			HttpClient.BaseAddress = new Uri(address);
		}

		private HttpClient HttpClient { get; } = new HttpClient();
		public async Task<StopVM> Get(string name)
		{
			var response = await HttpClient.GetAsync($"/api/Stops/{name}");
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<StopVM>(await response.Content.ReadAsStringAsync());
			}
			return default(StopVM);
		}
		public async Task<IEnumerable<string>> Find(string hint)
		{
			var response = await HttpClient.GetAsync($"/api/Stops/Find/{hint}");
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());
			}
			return default(IEnumerable<string>);
		}
		public async Task<RouteVM> GetRoutes(string id)
		{
			var response = await HttpClient.GetAsync($"/api/Stops/GetRoutes/{id}");
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<RouteVM>(await response.Content.ReadAsStringAsync());
			}
			return default(RouteVM);
		}
	}
}
