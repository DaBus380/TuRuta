using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Orleans.Grains.Services.Interfaces
{
    public interface IDistanceCalculator
    {
        double GetDistance(double lat1, double lat2, double lon1, double lon2);
    }
}
