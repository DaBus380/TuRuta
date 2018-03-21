using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IBusService
    {
        Task<List<string>> GetNoConfiguredBuses();

        Task<List<string>> FindBusByPlates(string plates);

        Task<List<string>> GetNoConfiguredPlates();

        Task SetRoute(Guid busId, Guid routeId);

        Task SetPlates(Guid busId, string plates);
    }
}
