using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;

namespace TuRuta.Orleans.Grains.Services.Interfaces
{
    public interface IDistanceCalculator
    {
        double GetDistance(Point pointA, Point pointB);
    }
}
