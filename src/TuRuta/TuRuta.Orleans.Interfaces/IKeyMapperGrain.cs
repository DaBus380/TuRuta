using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Interfaces
{
    public interface IKeyMapperGrain
    {
        Task<Guid> GetId(string name);

        Task SetName(string name, Guid Id);
    }
}
