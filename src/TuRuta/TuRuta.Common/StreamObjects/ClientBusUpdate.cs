using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common.StreamObjects
{
    public class ClientBusUpdate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid BusId { get; set; }
    }
}
