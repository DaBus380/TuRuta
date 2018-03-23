﻿using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Client.Buses;
using TuRuta.Client.Routes;

namespace TuRuta.Client
{
    public class TuRutaClient
    {
        public static RoutesClient RoutesClient { get; } = new RoutesClient();
        public static BusesClient BusesClient { get; } = new BusesClient();
    }
}