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
        public Task<List<string>> GetAllKeys()
            => Task.FromResult(State.Keys.ToList());

        public Task<List<string>> GetAllValues()
            => Task.FromResult(State.Values.ToList());

        public Task<string> GetId(string name)
        {
            if(State.TryGetValue(name, out var Id))
            {
                return Task.FromResult(Id);
            }

            return Task.FromResult(string.Empty);
        }

        public Task SetName(string Id, string name)
        {
            if(string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(name))
            {
                return Task.CompletedTask;
            }

            if (State.ContainsValue(name))
            {
                return Task.CompletedTask;
            }

            State.Add(Id, name);
            return WriteStateAsync();
        }
    }
}
