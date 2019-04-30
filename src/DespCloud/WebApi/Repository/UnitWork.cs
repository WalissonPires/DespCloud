using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database;

namespace WebApi.Repository
{
    public class UnitWork : IDisposable
    {
        internal readonly UnitWorkInfo Info;
        internal readonly AppDbContext Context;
        internal readonly IMapper Mapper;

        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        private EFClientsRepository _clientsRepository;
        private EFServicesRepository _servicesRepository;
        private EFVehiclesRepository _vehiclesRepository;
        private EFServiceOrdersRepository _ordersRepository;        
        private EFOrderServiceItemsRepository _orderItemsRepository;
        private EFCompaniesRepository _companyRepository;
        private EFAddressRepository _addressRepository;

        public EFClientsRepository Clients => _clientsRepository != null ? _clientsRepository : _clientsRepository = new EFClientsRepository(this);
        public EFServicesRepository Services => _servicesRepository != null ? _servicesRepository : _servicesRepository = new EFServicesRepository(this);
        public EFVehiclesRepository Vehicles => _vehiclesRepository != null ? _vehiclesRepository : _vehiclesRepository = new EFVehiclesRepository(this);
        public EFServiceOrdersRepository Orders => _ordersRepository != null ? _ordersRepository : _ordersRepository = new EFServiceOrdersRepository(this);
        public EFOrderServiceItemsRepository OrderItems => _orderItemsRepository != null ? _orderItemsRepository : _orderItemsRepository = new EFOrderServiceItemsRepository(this);
        public EFCompaniesRepository Companies => _companyRepository != null ? _companyRepository : _companyRepository = new EFCompaniesRepository(this);
        public EFAddressRepository Address => _addressRepository != null ? _addressRepository : _addressRepository = new EFAddressRepository(this);

        public UnitWork(UnitWorkInfo unitWorkInfo, AppDbContext appDbContext, IMapper mapper)
        {
            Info = unitWorkInfo;
            Context = appDbContext;
            Mapper = mapper;
            _transaction = null;
        }

        ~UnitWork()
        {
            Dispose(false);
        }


        internal int GenereteId(string context)
        {
            var sequence = Context.ContextSequence.Where(x => x.CompanyId == Info.CompanyId && x.Context == context).SingleOrDefault();            

            if (sequence != null)
            {
                sequence.Value++;
                Context.SaveChanges();
            }
            else
            {
                sequence = new Database.Entities.ContextSequenceEty
                {
                    CompanyId = Info.CompanyId,
                    Context = context,
                    Value = 1
                };
                Context.ContextSequence.Add(sequence);
                Context.SaveChanges();
            }

            return sequence.Value;
        }

        public void BeginTransaction()
        {
            _transaction = Context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public bool HasTransaction()
        {
            return _transaction != null;
        }


        public void Dispose()
        {            
            Dispose(true);           
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_transaction != null)
                    _transaction.Dispose();
            }

            _disposed = true;
        }
    }
}
