using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Grains.States;

namespace TuRuta.Orleans.Grains.Extensions
{
    public static class StopStateExtensions
    {
		public static StopVM ToVM(this StopState stopState, Guid stopId)
			=> new StopVM
			{
				Name = stopState.Name,
				Id = stopId,
				Location = stopState.Location
			};
	}
}
