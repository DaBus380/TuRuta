using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common
{
    public class Constants
    {
        public static string RouteGrainName { get; } = "routes";
        public static string BusGrainName { get; } = "buses";
		public static string StopGrainName { get; } = "stops";
        public static string BusConfigGrainName { get; } = "bus-config";
        public static string BusPlatesGrainName { get; } = "bus-plate-relation";
        public static string NoRouteConfiguredGrainName { get; } = "non-route-selected";
        public static string NoConfigGrainName { get; } = "no-config-buses";

        public static string ClusterId { get; } = "dp13";
    }
}
