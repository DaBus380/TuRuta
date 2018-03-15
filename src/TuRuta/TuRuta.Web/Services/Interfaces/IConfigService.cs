using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Common.ViewModels.ConfigVMs;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IConfigService
    {
        Task<List<string>> GetNoConfig();

        Task<BusConfigVM> GetConfig(string macAddress);

        Task<PubnubConfig> GetPubnub();
    }
}
