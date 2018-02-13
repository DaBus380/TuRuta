using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains
{
    [StorageProvider(ProviderName = "AzureTableStore")]
    public class KeyMapperGrain : Grain<Dictionary<string, string>>, IKeyMapperGrain
    {
        public Task<IEnumerable<string>> GetAllKeys()
            => Task.FromResult(State.Keys.AsEnumerable());

        public Task<IEnumerable<string>> GetAllValues()
            => Task.FromResult(State.Values.AsEnumerable());

        public Task<string> GetId(string name)
        {
            if(State.TryGetValue(name, out var Id))
            {
                return Task.FromResult(Id);
            }

            return Task.FromResult(default(string));
        }

        public Task SetName(string name, string Id)
        {
            State.Add(name, Id);
            return WriteStateAsync();
        }
    }
}
