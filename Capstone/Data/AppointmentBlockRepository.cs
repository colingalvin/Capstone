using Capstone.Contracts;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Data
{
    public class AppointmentBlockRepository : RepositoryBase<AppointmentBlock>, IAppointmentBlockRepository
    {
        public AppointmentBlockRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}
