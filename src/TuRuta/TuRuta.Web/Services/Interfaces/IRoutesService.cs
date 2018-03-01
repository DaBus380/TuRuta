using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IRoutesService
    {
        Task<RouteVM> Create(string name);
    }
}
