using Capstone.Contracts;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Data
{
    public class RuleSetRepository : RepositoryBase<RuleSet>, IRuleSetRepository
    {
        public RuleSetRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}
