using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Client;
using TuRuta.Client.Routes;
using TuRuta.Bot.Services.Interfaces;
using TuRuta.Common.ViewModels;

namespace TuRuta.Bot.Services
{
    public class RoutesServices : IRoutesService
    {
        private RoutesClient RoutesClient { get; } = TuRutaClient.RoutesClient;

        public Task<IEnumerable<string>> FindRoute(string hint)
            => RoutesClient.Find(hint);

        public Task<RouteVM> GetRoute(string name)
            => RoutesClient.Get(name);
    }
}
