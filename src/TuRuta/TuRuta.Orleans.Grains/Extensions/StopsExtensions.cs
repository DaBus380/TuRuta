using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using TuRuta.Common.ViewModels;

namespace TuRuta.Orleans.Grains.Extensions
{
    static class StopsExtensions
    {
        public static Stop ToStop(this StopVM stopVM)
            => new Stop
            {
                Id = stopVM.Id,
                Name = stopVM.Name,
                Location = stopVM.Location
            };
    }
}
