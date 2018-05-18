using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using TuRuta.Common.ViewModels.ConfigVMs;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class ConfigClient : IConfigClient
    {
        HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:56340")
        };

        public async Task<PubnubConfig> GetPubnubConfig()
        {
            var response = await httpClient.GetAsync("/api/config/pubnub");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PubnubConfig>(json);
            }

            return default(PubnubConfig);
        }
    }
}
