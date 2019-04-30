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
    public class EFClientsRepository : EFRepositoryBase
    {
        private const string CONTEXT_GENERETOR_ID = "Clients";

        private IQueryable<ClientEty> ClientsNoTracking => _context.Clients.Include(x => x.Address).AsNoTracking();

        public EFClientsRepository(UnitWork unitWork) : base(unitWork)
        {
        }

        public List<Client> Get(FilterParams filterParams)
        {
            var clients = ClientsNoTracking.Where(x => x.CompanyId == _info.CompanyId).Select(x => _unitWork.Mapper.Map<Client>(x)).ToList();

            return clients;
        }

        public Client GetById(int id)
        {            
            var client = ClientsNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            return _unitWork.Mapper.Map<Client>(client);            
        }

        public Client Create(Client client)
        {
            var dbClient = _unitWork.Mapper.Map<ClientEty>(client);
            dbClient.Id = _unitWork.GenereteId(CONTEXT_GENERETOR_ID);
            dbClient.CompanyId = _info.CompanyId;
            dbClient.CreateAt = DateTimeOffset.Now;          

            if (dbClient.Address != null) {

                if (!String.IsNullOrEmpty(dbClient.Address.Street) || !String.IsNullOrEmpty(dbClient.Address.Street))
                {
                    validateAddress(dbClient.Address);

                    _context.Address.Add(dbClient.Address);
                    _context.SaveChanges();

                    dbClient.AddressId = dbClient.Address.Id;
                }
                else
                {
                    dbClient.AddressId = null;
                    dbClient.Address = null;
                }
            }

            _context.Clients.Add(dbClient);
            _context.SaveChanges();

            dbClient = ClientsNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == dbClient.Id).Single();

            return _unitWork.Mapper.Map<Client>(dbClient);
        }

        public Client Update(Client model)
        {
            var dbClient = _context.Clients.Include(x => x.Address).Where(x => x.CompanyId == _info.CompanyId && x.Id == model.Id).SingleOrDefault();

            if (dbClient == null)
                throw new AppValidationException("Cliente não encontrado");

            dbClient.Name = model.Name;
            dbClient.Phone = model.Phone;
            dbClient.Email = model.Email;
            dbClient.ContactName = model.ContactName;
            dbClient.CpfCnpj = model.CpfCnpj;
            dbClient.RgIE = model.RgIE;
            dbClient.Org = model.Org;

            //if (model.Address == null)
            //    throw new AppValidationException("Informe os dados de endereço");

            _unitWork.Mapper.Map(model.Address, dbClient.Address);

            if (dbClient.AddressId.HasValue)
                dbClient.Address.Id = dbClient.AddressId.Value;

            if (dbClient.Address != null)
            {
                validateAddress(dbClient.Address);
            }

            _context.SaveChanges();

            return _unitWork.Mapper.Map<Client>(dbClient);
        }

        public void Delete(int id)
        {
            var dbClient = _context.Clients.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            if (dbClient == null)
                throw new AppValidationException("Cliente não encontrado");

            try
            {
                _context.Clients.Remove(dbClient);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new AppValidationException("Não é possível excluir o cliente porque existe registros vinculados ao mesmo");
            }
        }

        private bool validateAddress(AddressEty address)
        {
            if (address.CountyId == 0)
                throw new AppValidationException("Informe um estado(UF) valido");

            if (address.CityId == 0)
                throw new AppValidationException("Informe uma cidade valida");

            if (address.DistrictId == 0)
                throw new AppValidationException("Informe um bairro valido");

            if (String.IsNullOrEmpty(address.Number))
                throw new AppValidationException("Informe um número da residência");

            if (String.IsNullOrEmpty(address.Street))
                throw new AppValidationException("Informe o logradouro da residência");

            return true;
        }
    }
}
