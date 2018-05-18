using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using TuRuta.Bot.Services.Interfaces;

namespace TuRuta.Bot.Services
{
    public class LuisService : ILuisService
    {
        private LuisClient LuisClient { get; }
        public LuisService(IConfiguration configuration)
        {
            var appKey = configuration.GetValue<string>("LuisAppKey");
            var appId = configuration.GetValue<string>("LuisAppId");
            LuisClient = new LuisClient(appId, appKey);
        }

        public Task<LuisResult> FindIntent(string text)
            => LuisClient.Predict(text);
    }
}
