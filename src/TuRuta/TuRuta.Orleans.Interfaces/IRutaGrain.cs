using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using Orleans;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Interfaces
{
    public interface IRutaGrain : IGrainWithGuidKey
    {
		Task<List<Parada>> AllParadas();
    }
}
