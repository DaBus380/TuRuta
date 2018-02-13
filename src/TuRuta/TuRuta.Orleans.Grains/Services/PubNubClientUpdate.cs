using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PubnubApi;

using TuRuta.Common.StreamObjects;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class PubNubClientUpdate : IClientUpdate
    {
        private Pubnub pubnub;

        public PubNubClientUpdate()
        {
            pubnub = new Pubnub(new PNConfiguration
            {
                SubscribeKey = "",
                PublishKey = ""
            });
        }

        private void Sent(ClientBusUpdate update)
        {
            pubnub.Publish()
                .Channel("client")
                .Message(update)
                .Sync();
        }

		public Task SentUpdate(ClientBusUpdate update)
			=> Task.Run(() => Sent(update));
    }
}
