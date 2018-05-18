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
        public List<IStopGrain> Stops { get; set; } = new List<IStopGrain>();
		public string Name { get; set; }
        public List<IBusGrain> Buses { get; set; } = new List<IBusGrain>();
    }
}
