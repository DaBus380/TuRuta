using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Grains.Services.Interfaces
{
    interface IClientUpdate
    {
        Task<bool> SentUpdate(object update);
    }
}
