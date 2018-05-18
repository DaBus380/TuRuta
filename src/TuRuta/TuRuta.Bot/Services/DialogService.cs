using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingMapsRESTToolkit;

using TuRuta.Bot.Services.Interfaces;
using TuRuta.Common.ViewModels;
using Microsoft.Extensions.Configuration;

namespace TuRuta.Bot.Services
{
    public class DialogService : IDialogService
    {
        private string BingMapsKey { get; }
        private IRoutesService Routes { get; }
        private ILuisService Luis { get; }
        public DialogService(
            IConfiguration configuration,
            IRoutesService routes,
            ILuisService luis)
        {
            Luis = luis;
            Routes = routes;
            BingMapsKey = configuration.GetValue<string>("BingMapsKey");
        }

        public async Task<Activity> GetResponse(Activity activity)
        {
            var result = await Luis.FindIntent(activity.Text);
            var response = activity.CreateReply();
            if(result.TopScoringIntent.Name == "Ask")
            {
                string routeName = "";
                foreach (var value in result.Entities.First().Value)
                {
                    routeName += value.Value;
                }
                response = await GetRoute(routeName, response);
            }
            else if(result.TopScoringIntent.Name == "Search")
            {
                string routeName = "";
                foreach (var value in result.Entities.First().Value)
                {
                    routeName += value.Value;
                }

                response = await SearchRoute(routeName, response);
            }

            return response;
        }

        private async Task<Activity> SearchRoute(string name, Activity response)
        {
            var searchResult = await Routes.FindRoute(name);
            
            if(searchResult.Count() != 0)
            {
                response.Text = "Encontré estas rutas";
                response.SuggestedActions = new SuggestedActions
                {
                    Actions = new List<CardAction>()
                };

                foreach (var result in searchResult)
                {

                    response.SuggestedActions.Actions.Add(new CardAction { Title = result, Value = $"Dame la ruta {result}", Type = ActionTypes.ImBack });
                }
            }
            else
            {
                response.Text = "No pudimos encontrar una ruta con ese nombre...";
            }

            return response;
        }

        private async Task<Activity> GetRoute(string name, Activity response)
        {
            var route = await Routes.GetRoute(name);
            if (route == null)
            {
                response.Text = "No existe una ruta con ese nombre...";
                return response;
            }

            response.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            response.Attachments = new List<Attachment>
            {
                BuildCard(route).ToAttachment()
            };

            return response;
        }

        private HeroCard BuildCard(RouteVM routeVM)
        {
            var request = new ImageryRequest
            {
                ImagerySet = ImageryType.Road,
                BingMapsKey = BingMapsKey
            };

            request.Pushpins = routeVM.Stops.Select(stop => new ImageryPushpin
            {
                Location = new Coordinate(stop.Location.Latitude, stop.Location.Longitude),
                IconStyle = 3
            }).ToList();
            
            var newCard = new HeroCard()
            {
                Title = routeVM.Name,
                Text = "Encontré esta ruta",
                Images = new List<CardImage>
                {
                    new CardImage(url: request.GetRequestUrl())
                }
            };

            return newCard;
        }
    }
}
