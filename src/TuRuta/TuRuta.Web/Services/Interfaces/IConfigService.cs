using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IConfigService
    {
        Task<BusConfigVM> GetConfig(string macAddress);
    }
}
