using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels.ConfigVMs;

namespace TuRuta.Orleans.Grains.Services.Interfaces
{
    public interface IConfigClient
    {
        Task<PubnubConfig> GetPubnubConfig();
    }
}
