using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Client.Buses
{
    public class BusesClient
    {
		public BusesClient(string baseAddress = null)
		{
			var address = baseAddress ?? "http://localhost:56340/";
			HttpClient.BaseAddress = new Uri(address);
		}
		private HttpClient HttpClient { get; } = new HttpClient();
		public async Task<BusInfoVM> Info(Guid busId)
        {
			var response = await HttpClient.GetAsync($"/api/Buses/{busId}");
			if(response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<BusInfoVM>(await response.Content.ReadAsStringAsync());
			}
			return default(BusInfoVM);
        }
    }
}
