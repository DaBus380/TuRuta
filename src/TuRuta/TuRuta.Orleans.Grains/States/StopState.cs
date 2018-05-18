using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains.States
{
    public class StopState
    {
		public Point Location { get; set; }
		public string Name { get; set; }
		public List<IRouteGrain> Routes { get; set; } = new List<IRouteGrain>();
	}
}
