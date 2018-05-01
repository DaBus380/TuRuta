using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;
using TuRuta.Common.Models;

namespace TuRuta.Web.Services.Mocks
{
    public class MockBusService : IBusService
    {
        private BusVM Bus { get; } = new BusVM 
        {
            LicensePlate = "MP-18-11",
            Status = 1,
            Location = new Point
            {
                Latitude = 20.786873,
                Longitude = -103.522932
            },
            Id = Guid.NewGuid()
        };

        private List<string> Lists = new List<string>
        {
            Guid.NewGuid().ToString()
        };

        public Task<List<string>> FindBusByPlates(string plates) => Task.FromResult(Lists);
        public Task<BusInfoVM> GetBus(Guid Id)
            => Task.FromResult(new BusInfoVM
            {
                PreviewsStop = "nigga stop",
                NextStop = "white stop",
                CurrentStop = "yellow stop"
            });
        public Task<List<string>> GetNoConfiguredBuses() => Task.FromResult(Lists);
        public Task<List<string>> GetNoConfiguredPlates() => Task.FromResult(Lists);
        
        public Task SetPlates(Guid busId, string plates) => Task.CompletedTask;
        public Task SetRoute(Guid busId, Guid routeId) => Task.CompletedTask;
    }
}
