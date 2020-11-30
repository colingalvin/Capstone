using Capstone.Contracts;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Data
{
    public class DefaultTimeRepository : RepositoryBase<DefaultTime>, IDefaultTimeRepository
    {
        public DefaultTimeRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}
