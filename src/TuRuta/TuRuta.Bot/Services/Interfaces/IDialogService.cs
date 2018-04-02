using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuRuta.Bot.Services.Interfaces
{
    public interface IDialogService
    {
        Task<Activity> GetResponse(Activity activity);
    }
}
