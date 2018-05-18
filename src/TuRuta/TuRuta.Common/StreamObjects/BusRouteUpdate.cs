using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;

namespace TuRuta.Common.StreamObjects
{
    [Serializable]
    public class BusRouteUpdate
    {
        public Point Position { get; set; }
        public Guid BusId { get; set; }
    }
}
