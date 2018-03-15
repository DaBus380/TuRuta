using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;

namespace TuRuta.Common.StreamObjects
{
    public class ClientBusUpdate
    {
        public Point Location { get; set; }
        public Guid BusId { get; set; }
        public Stop NextStop { get; set; }
    }
}
