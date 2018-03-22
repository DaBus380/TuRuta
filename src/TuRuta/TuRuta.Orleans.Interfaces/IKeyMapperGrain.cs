using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Interfaces
{
    public interface IKeyMapperGrain : IGrainWithStringKey
    {
        Task<List<string>> FindByKey(string id);

        Task<string> GetId(string name);

        Task SetName(string Id, string Name);

        Task<List<string>> GetAllKeys();

        Task<List<string>> GetAllValues();

        Task<List<string>> FindByValue(string id);
        Task<List<string>> FindByValueGetValues(string id);

        Task RemoveKey(string key);

        Task UpdateKey(string key, string value);
    }
}
