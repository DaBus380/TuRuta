using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using TuRuta.Common.ViewModels.ConfigVMs;

namespace TuRuta.Device.Configuration
{
    class ConfigurationClient
    {
        private HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://4cd8389b.ngrok.io")
        };

        public async Task<BusConfigVM> GetConfig()
        {
            var result = await httpClient.GetAsync($"/api/config/busconfig/{GetMac()}");
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<BusConfigVM>(await result.Content.ReadAsStringAsync());
            }

            return default(BusConfigVM);
        }

        private string GetMac()
            => NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic =>
                (nic.OperationalStatus == OperationalStatus.Up)
                &&
                (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
            .Last()
            .GetPhysicalAddress()
            .ToString();
    }
}
