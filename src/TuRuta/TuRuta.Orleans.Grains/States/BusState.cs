using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Orleans.Grains.States
{
	[Serializable]
    public class BusState
    {
		public double CurrentLatitude { get; set; }
		public double CurrentLongitude { get; set; }
        public Guid RouteId { get; set; }

	}
}
