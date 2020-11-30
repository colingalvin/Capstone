using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Capstone.Contracts.IRepositoryBase;

namespace Capstone.Contracts
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
    }
}
