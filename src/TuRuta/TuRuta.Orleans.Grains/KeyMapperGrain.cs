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
    public class KeyMapperGrain : Grain<Dictionary<string, Guid>>, IKeyMapperGrain
    {
        public Task<IEnumerable<string>> GetAllKeys()
            => Task.FromResult(State.Keys.AsEnumerable());

        public Task<IEnumerable<Guid>> GetAllValues()
            => Task.FromResult(State.Values.AsEnumerable());

        public async Task<Guid> GetId(string name)
        {
            if(State.TryGetValue(name, out var Id))
            {
                return Id;
            }

            var newId = Guid.NewGuid();
            State.Add(name, newId);

            await WriteStateAsync();

            return newId;
        }

        public Task SetName(string name, Guid Id)
        {
            State.Add(name, Id);
            return WriteStateAsync();
        }
    }
}
