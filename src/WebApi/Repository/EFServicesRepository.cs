using Base.Domain.Contracts;
using Base.Domain.Entities;
using Base.WebApp.Contracts.Network;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Repository
{
    public class EFServicesRepository : EFRepositoryBase
    {
        private const string CONTEXT_GENERETOR_ID = "Services";

        private IQueryable<ServiceEty> ServicesNoTracking => _context.Services.AsNoTracking();

        public EFServicesRepository(UnitWork unitWork) : base(unitWork)
        {
        }

        public List<Service> Get(FilterParams filterParams)
        {
            var services = ServicesNoTracking.Where(x => x.CompanyId == _info.CompanyId).Select(x => _unitWork.Mapper.Map<Service>(x)).ToList();

            return services;
        }

        public Service GetById(int id)
        {
            var service = ServicesNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            return _unitWork.Mapper.Map<Service>(service);
        }

        public Service Create(Service service)
        {
            var _service = _unitWork.Mapper.Map<ServiceEty>(service);
            _service.Id = _unitWork.GenereteId(CONTEXT_GENERETOR_ID);
            _service.CompanyId = _info.CompanyId;

            _context.Services.Add(_service);
            _context.SaveChanges();

            var dbService = ServicesNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == _service.Id).Single();
            return _unitWork.Mapper.Map<Service>(dbService);
        }

        public Service Update(Service service)
        {
            var dbService = _context.Services.Where(x => x.CompanyId == _info.CompanyId && x.Id == service.Id).SingleOrDefault();
            if (dbService == null)
                throw new AppValidationException("Serviço não encontrado");

            dbService.Name = service.Name;
            dbService.Honorary = service.Honorary;
            dbService.Rate = service.Rate;
            dbService.PlateCard = service.PlateCard;
            dbService.Other = service.Other;

            _context.SaveChanges();

            return _unitWork.Mapper.Map<Service>(dbService);
        }

        public void Delete(int id)
        {
            var dbService = _context.Services.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();
            if (dbService == null)
                throw new AppValidationException("Serviço não encontrado");

            try
            {
                _context.Services.Remove(dbService);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new AppValidationException("Não é possível excluir o serviço porque existe registros vinculados ao mesmo");
            }
        }
    }
}
