using Capstone.Contracts;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Data
{
    public class PendingAppointmentRepository : RepositoryBase<PendingAppointment>, IPendingAppointmentRepository
    {
        public PendingAppointmentRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}
