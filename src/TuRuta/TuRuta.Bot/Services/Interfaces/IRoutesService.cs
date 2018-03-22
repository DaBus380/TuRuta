using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Bot.Services.Interfaces
{
    public interface IRoutesService
    {
        Task<RouteVM> GetRoute(string name);
        Task<IEnumerable<string>> FindRoute(string hint);
    }
}
