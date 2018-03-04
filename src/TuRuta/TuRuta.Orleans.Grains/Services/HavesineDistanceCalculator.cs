using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class HavesineDistanceCalculator : IDistanceCalculator
    {
        public double GetDistance(Point pointA, Point pointB)
        {
            double ToRadians(double coordinate)
                => (coordinate * Math.PI) / 180;

            var lat1Radians = ToRadians(pointA.Latitude);
            var lat2Radians = ToRadians(pointB.Latitude);
            var latDiff = ToRadians(pointB.Latitude - pointA.Latitude);
            var lonDiff = ToRadians(pointB.Longitude - pointA.Longitude);


            var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) + Math.Cos(lat1Radians) * Math.Cos(lat2Radians) * Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return c * (6371 * 1000);
        }
    }
}
