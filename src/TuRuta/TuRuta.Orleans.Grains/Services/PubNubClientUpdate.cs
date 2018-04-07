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

        public PubNubClientUpdate(string subKey, string pubKey)
        {
            pubnub = new Pubnub(pubKey, subKey);
        }

        private bool Sent(ClientBusUpdate update, string busId)
        {
            return pubnub.Publish(
                busId,
                update,
                (obj) => {},
                (error) => {});
        }

        public Task<bool> SentUpdate(ClientBusUpdate update, Guid busId)
            => Task.FromResult(Sent(update, busId.ToString()));
    }
}
