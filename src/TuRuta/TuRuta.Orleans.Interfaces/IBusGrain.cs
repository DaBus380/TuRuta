using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using TuRuta.Common.ViewModels;

namespace TuRuta.Orleans.Interfaces
{
    public interface IBusGrain : IGrainWithGuidKey
    {
        Task<BusVM> GetBusVM();

        Task SetRoute(Guid route);

        Task SetPlates(string plates);
    }
}
