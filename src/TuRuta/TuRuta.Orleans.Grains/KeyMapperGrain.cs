using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains
{
    public class KeyMapperGrain : Grain<Dictionary<string, Guid>>, IKeyMapperGrain
    {
        public Task<Guid> GetId(string name)
            => Task.FromResult(State[name]);

        public Task SetName(string name, Guid Id)
        {
            State.Add(name, Id);
            return WriteStateAsync();
        }
    }
}
