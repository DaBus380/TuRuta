using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Bot.Services.Interfaces;

namespace TuRuta.Bot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private IConfiguration Configuration { get; }
        private IDialogService DialogService { get; }
        public MessagesController(
            IDialogService dialogService,
            IConfiguration configuration)
        {
            Configuration = configuration;
            DialogService = dialogService;
        }

        [HttpPost]
        [Authorize(Roles = "Bot")]
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            var appCredentials = new MicrosoftAppCredentials(Configuration);
            if(appCredentials.MicrosoftAppId == null || appCredentials.MicrosoftAppPassword == null)
            {
                appCredentials.MicrosoftAppId = Configuration.GetValue<string>("MicrosoftAppIdKey");
                appCredentials.MicrosoftAppPassword = Configuration.GetValue<string>("MicrosoftAppPasswordKey");
            }

            var client = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);
            if(activity.Type == ActivityTypes.Message)
            {
                var reply = await DialogService.GetResponse(activity);

                await client.Conversations.ReplyToActivityAsync(reply);
            }
            return Ok();
        }
    }
}
