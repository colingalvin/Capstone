using Capstone.Contracts;
using Capstone.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        private IAddressRepository _address;
        private IAppointmentBlockRepository _appointmentBlock;
        private IAppointmentRepository _appointment;
        private IClientRepository _client;
        private IDefaultTimeRepository _defaultTime;
        private IPendingAppointmentRepository _pendingAppointment;
        private IPianoRepository _piano;
        private IRuleSetRepository _ruleSet;
        public IAddressRepository Address
        {
            get
            {
                if(_address == null)
                {
                    _address = new AddressRepository(_context);
                }
                return _address;
            }
        }
        public IAppointmentBlockRepository AppointmentBlock
        {
            get
            {
                if (_appointmentBlock == null)
                {
                    _appointmentBlock = new AppointmentBlockRepository(_context);
                }
                return _appointmentBlock;
            }
        }
        public IAppointmentRepository Appointment
        {
            get
            {
                if (_appointment == null)
                {
                    _appointment = new AppointmentRepository(_context);
                }
                return _appointment;
            }
        }
        public IClientRepository Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new ClientRepository(_context);
                }
                return _client;
            }
        }
        public IDefaultTimeRepository DefaultTime
        {
            get
            {
                if (_defaultTime == null)
                {
                    _defaultTime = new DefaultTimeRepository(_context);
                }
                return _defaultTime;
            }
        }
        public IPendingAppointmentRepository PendingAppointment
        {
            get
            {
                if (_pendingAppointment == null)
                {
                    _pendingAppointment = new PendingAppointmentRepository(_context);
                }
                return _pendingAppointment;
            }
        }
        public IPianoRepository Piano
        {
            get
            {
                if (_piano == null)
                {
                    _piano = new PianoRepository(_context);
                }
                return _piano;
            }
        }
        public IRuleSetRepository RuleSet
        {
            get
            {
                if (_ruleSet == null)
                {
                    _ruleSet = new RuleSetRepository(_context);
                }
                return _ruleSet;
            }
        }
        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
