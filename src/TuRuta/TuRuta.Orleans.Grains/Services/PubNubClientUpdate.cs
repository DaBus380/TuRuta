using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PubNubMessaging.Core;
using TuRuta.Common.StreamObjects;
using TuRuta.Orleans.Grains.Services.Interfaces;

namespace TuRuta.Orleans.Grains.Services
{
    public class PubNubClientUpdate : IClientUpdate
    {
        private Pubnub pubnub;

        public PubNubClientUpdate()
        {
            pubnub = new Pubnub("","");
        }

        private bool Sent(ClientBusUpdate update)
        {
            return pubnub.Publish(
                "client",
                update,
                (obj) => {},
                (error) => {});
        }

        public Task<bool> SentUpdate(ClientBusUpdate update)
            => Task.FromResult(Sent(update));
    }
}
