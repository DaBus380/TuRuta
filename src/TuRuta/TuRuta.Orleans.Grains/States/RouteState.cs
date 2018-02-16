using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains.States
{
	[Serializable]
    public class RouteState
    {
		public List<Stop> Stops { get; set; }
		public string Name { get; set; }
		public List<IBusGrain> Buses { get; set; }
    }
}
