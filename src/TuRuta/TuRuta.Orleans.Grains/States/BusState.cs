using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;

namespace TuRuta.Orleans.Grains.States
{
	[Serializable]
    public class BusState
    {
        public string Plates { get; set; } = string.Empty;
        public Point Location { get; set; } = new Point();
        public Guid RouteId { get; set; }
	}
}
