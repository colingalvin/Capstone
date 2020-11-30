using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Contracts
{
    public interface IRepositoryWrapper
    {
        IAddressRepository Address { get; }
        IAppointmentBlockRepository AppointmentBlock { get; }
        IAppointmentRepository Appointment { get; }
        IClientRepository Client { get; }
        IDefaultTimeRepository DefaultTime { get; }
        IPendingAppointmentRepository PendingAppointment { get; }
        IPianoRepository Piano { get; }
        IRuleSetRepository RuleSet { get; }
        void Save();
    }
}
