using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PubNubMessaging;
using PubNubMessaging.Core;
using Newtonsoft.Json;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class PubNubClientUpdate : IClientUpdate
    {
        
        Pubnub pubnub = new Pubnub("","");

        private bool Sent(object update)
        {
            var json = JsonConvert.SerializeObject(update);
            return pubnub.Publish<object>(
                channel: "client",
                message: update,
                userCallback: null,
				errorCallback: null);
        }

		public Task<bool> SentUpdate(object update)
			=> Task.FromResult(Sent(update));
    }
}
