using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TuRuta.Common.StreamObjects;

namespace TuRuta.Orleans.Grains.Services.Interfaces
{
    public interface IClientUpdate
    {
        Task<bool> SentUpdate(ClientBusUpdate update);
    }
}
