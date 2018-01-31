using System.Collections.Generic;

namespace TuRuta.Common.ViewModels
{
    public class RouteVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<BusVM> Buses { get; set; }
        public IEnumerable<StopVM> Stops { get; set; }
        public IEnumerable<IncidentVM> Incidents { get; set; }
    }
}