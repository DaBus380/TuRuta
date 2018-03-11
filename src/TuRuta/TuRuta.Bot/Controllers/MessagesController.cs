using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuRuta.Bot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private IConfiguration Configuration { get; }
        public MessagesController(IConfiguration configuration)
        {
            Configuration = configuration;
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
            var reply = activity.CreateReply();
            if(activity.Type == ActivityTypes.Message)
            {
                reply.Text = $"echo: {activity.Text}";
            }
            else
            {
                reply.Text = $"activity type: {activity.Type}";
            }

            await client.Conversations.ReplyToActivityAsync(reply);
            return Ok();
        }
    }
}
