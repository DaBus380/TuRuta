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
        private HttpClient HttpClient { get; } = new HttpClient
        {
            BaseAddress = new Uri("https://27985237.ngrok.io")
        };

        public async Task<IEnumerable<string>> Find(string hint)
        {
            var response = await HttpClient.GetAsync($"/api/routes/find/{hint}");
            return JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<RouteVM> Get(string name)
        {
            var response = await HttpClient.GetAsync($"/api/routes/{name}");
            var text = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RouteVM>(await response.Content.ReadAsStringAsync());
        }
    }
}
