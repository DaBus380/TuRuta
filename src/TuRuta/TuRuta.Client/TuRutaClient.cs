using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Client.Buses;
using TuRuta.Client.Routes;
using TuRuta.Client.Stops;

namespace TuRuta.Client
{
    public class TuRutaClient
    {
        public static RoutesClient RoutesClient { get; } = new RoutesClient();
        public static BusesClient BusesClient { get; } = new BusesClient();

        public static RoutesClient RoutesClientAndroid { get; } = new RoutesClient("https://f7a13bef.ngrok.io/");
    }
}
