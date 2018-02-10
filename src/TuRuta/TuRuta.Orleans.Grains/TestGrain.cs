using System;
using System.Collections.Generic;
using System.Text;
using Orleans;

using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains
{
    public class TestGrain : Grain, ITestGrain
    {
    }
}
