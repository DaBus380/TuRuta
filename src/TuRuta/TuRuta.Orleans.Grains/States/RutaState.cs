using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains.States
{
	[Serializable]
    public class RutaState
    {
		public List<Parada> AllParadas { get; set; }
		public string Nombre { get; set; }
		public List<IBusGrain> AllBuses { get; set; }
    }
}
