using System;
using System.Collections.Generic;
using System.Text;

using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class HavesineDistanceCalculator : IDistanceCalculator
    {
        public double GetDistance(double lat1, double lat2, double lon1, double lon2)
        {
            double ToRadians(double coordinate)
                => (coordinate * Math.PI) / 180;

            var lat1Radians = ToRadians(lat1);
            var lat2Radians = ToRadians(lat2);
            var latDiff = ToRadians((lat2 - lat1));
            var lonDiff = ToRadians(lon2 - lon1);

            var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) + Math.Cos(lat1Radians) * Math.Cos(lat2Radians) * Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return c * (6371 * 1000);
        }
    }
}
