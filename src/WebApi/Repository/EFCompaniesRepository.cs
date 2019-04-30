using Base.Domain.Contracts;
using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Repository
{
    public class EFCompaniesRepository : EFRepositoryBase
    {

        private IQueryable<CompanyEty> CompaniesNoTracking => _context.Companies.AsNoTracking().Include(x => x.Address);

        public EFCompaniesRepository(UnitWork unitWork) : base(unitWork)
        {
        }

        public Company GetUserCompany()
        {
            var company = CompaniesNoTracking.Where(x => x.Id == _info.CompanyId).SingleOrDefault();

            return _unitWork.Mapper.Map<Company>(company);
        }

        public Company Update(Company model)
        {
            var dbCompany = _context.Companies.Include(x => x.Address).Where(x => x.Id == _info.CompanyId).SingleOrDefault();
            if (dbCompany == null)
                throw new AppValidationException("Empresa não encontrada");

            if (!_unitWork.HasTransaction())
                throw new Exception("A transaction must be started");

            if (model.Address != null)
            {
                AddressEty dbAddress;
                if (dbCompany.Address != null)
                    dbAddress = dbCompany.Address;
                else                
                    dbAddress = new AddressEty();

                if (model.Address.CountyId == 0)
                    throw new AppValidationException("Informe um estado(UF) valido");

                if (model.Address.CityId == 0)
                    throw new AppValidationException("Informe uma cidade valida");

                if (model.Address.DistrictId == 0)
                    throw new AppValidationException("Informe um bairro valido");

                if (String.IsNullOrEmpty(model.Address.Number))
                    throw new AppValidationException("Informe um número da residência");

                if (String.IsNullOrEmpty(model.Address.Street))
                    throw new AppValidationException("Informe o logradouro da residência");

                _unitWork.Mapper.Map(model.Address, dbAddress);

                if (dbCompany.AddressId == null)
                {
                    _context.Address.Add(dbAddress);
                    _context.SaveChanges();

                    dbCompany.AddressId = dbAddress.Id;
                }
                else
                    dbAddress.Id = dbCompany.AddressId.Value;
            }

            dbCompany.Name = model.Name;
            dbCompany.CpfCnpj = model.CpfCnpj;
            dbCompany.Phone = model.Phone;
            dbCompany.Email = model.Email;

            _context.SaveChanges();

            return _unitWork.Mapper.Map<Company>(dbCompany);
        }        
    }
}
