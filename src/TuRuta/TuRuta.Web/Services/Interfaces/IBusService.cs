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
    }
}
