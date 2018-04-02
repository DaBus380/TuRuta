using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Enums;
using TuRuta.Common.Models;

namespace TuRuta.Common.Device
{
    [Serializable]
    public class RouteBusUpdate
    {
        public Point Location { get; set; }
        public BusStatus Status { get; set; }
    }
}
