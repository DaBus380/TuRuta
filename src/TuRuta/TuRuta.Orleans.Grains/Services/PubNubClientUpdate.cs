using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TuRuta.Orleans.Grains.Services.Interfaces;
using PubnubApi;

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

        private void Sent(object update)
        {
            pubnub.Publish()
                .Channel("client")
                .Message(update)
                .Sync();
        }

		public Task SentUpdate(object update)
			=> Task.Run(() => Sent(update));
    }
}
