using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Interfaces
{
    public interface IKeyMapperGrain : IGrainWithStringKey
    {
        Task<string> GetId(string name);

        Task SetName(string name, string Id);

        Task<IEnumerable<string>> GetAllKeys();

        Task<IEnumerable<string>> GetAllValues();
    }
}
