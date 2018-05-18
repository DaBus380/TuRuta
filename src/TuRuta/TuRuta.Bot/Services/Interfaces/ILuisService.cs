using Microsoft.Cognitive.LUIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuRuta.Bot.Services.Interfaces
{
    public interface ILuisService
    {
        Task<LuisResult> FindIntent(string text);
    }
}
