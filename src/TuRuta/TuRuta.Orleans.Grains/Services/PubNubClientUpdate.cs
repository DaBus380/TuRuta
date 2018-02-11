using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PubNubMessaging.Core;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class PubNubClientUpdate : IClientUpdate
    {
        private Pubnub pubnub;

        public PubNubClientUpdate()
        {
            pubnub = new Pubnub("pub-c-188fed9a-a6bc-4630-9150-055fd7b5ba07", "sub-c-9e8bf2f0-0166-11e8-b09a-5e0f113aa1d7");
        }

        private bool Sent(object update)
        {
            return pubnub.Publish<object>(
                "client",
                update, 
                (obj) => 
                {
                }, 
                (error) => 
                {
                });
        }

		public Task<bool> SentUpdate(object update)
			=> Task.Run(() => Sent(update));
    }
}
