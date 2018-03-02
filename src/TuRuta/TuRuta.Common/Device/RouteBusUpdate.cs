using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Enums;

namespace TuRuta.Common.Device
{
    [Serializable]
    public class RouteBusUpdate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public BusStatus Status { get; set; }
    }
}
