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

        public Task<string> GetId(string Id)
        {
            if (State.TryGetValue(Id.ToLowerInvariant(), out var name))
            {
                return Task.FromResult(name);
            }

            return Task.FromResult(string.Empty);
        }

        public Task<List<string>> FindByValue(string id)
            => Task.FromResult(
                State
                .Where(pair => pair.Value.Contains(id.ToLowerInvariant()))
                .Select(pair => pair.Key)
                .ToList());

        public Task SetName(string Id, string name)
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(name))
            {
                return Task.CompletedTask;
            }

            if (State.ContainsValue(name.ToLowerInvariant()) || State.ContainsKey(Id.ToLowerInvariant()))
            {
                return Task.CompletedTask;
            }

            State.Add(Id.ToLowerInvariant(), name.ToLowerInvariant());
            return WriteStateAsync();
        }

        public Task<List<string>> FindByKey(string id)
            => Task.FromResult(
                State
                .Where(pair => pair.Key.Contains(id.ToLowerInvariant()))
                .Select(pair => pair.Value)
                .ToList());

        public Task RemoveKey(string key)
        {
            State.Remove(key.ToLowerInvariant());
            return Task.CompletedTask;
        }

        public Task UpdateKey(string key, string value)
        {
            if (State.ContainsKey(key.ToLowerInvariant()))
            {
                State[key] = value.ToLowerInvariant();
                return Task.CompletedTask;
            }

            return SetName(key.ToLowerInvariant(), value.ToLowerInvariant());
        }

        public Task<List<string>> FindByValueGetValues(string id) 
            => Task.FromResult(
                State
                .Where(pair => pair.Value.Contains(id.ToLowerInvariant()))
                .Select(pair => pair.Value)
                .ToList());
    }
}
