using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TuRuta.Web.Services.Interfaces;
using TuRuta.Common.ViewModels.ConfigVMs;

namespace TuRuta.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        private readonly IConfigService _configService;
        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        [HttpGet("[action]")]
        public Task<List<string>> NoConfig()
            => _configService.GetNoConfig();

        [HttpGet("[action]/{macAddress}")]
        public Task<BusConfigVM> BusConfig(string macAddress)
            => _configService.GetConfig(macAddress);

        [HttpGet("[action]")]
        public async Task<PubnubConfig> PubNub()
            => await _configService.GetPubnub();
    }
}
