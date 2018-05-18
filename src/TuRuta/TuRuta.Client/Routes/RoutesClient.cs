using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using TuRuta.Common.ViewModels;

namespace TuRuta.Client.Routes
{
    public class RoutesClient
    {
        public RoutesClient(string baseAddress = null)
        {
            var address = baseAddress ?? "http://localhost:56340/";
            HttpClient.BaseAddress = new Uri(address);
        }

        private HttpClient HttpClient { get; } = new HttpClient();

        public async Task<IEnumerable<string>> Find(string hint)
        {
            var response = await HttpClient.GetAsync($"/api/routes/find/{hint}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());
            }

            return default(IEnumerable<string>);
        }

        public async Task<RouteVM> Get(string name)
        {
            var response = await HttpClient.GetAsync($"/api/routes/{name}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RouteVM>(await response.Content.ReadAsStringAsync());
            }

            return default(RouteVM);
        }
    }
}
