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

        Task SetName(string Id, string Name);

        Task<List<string>> GetAllKeys();

        Task<List<string>> GetAllValues();
    }
}
