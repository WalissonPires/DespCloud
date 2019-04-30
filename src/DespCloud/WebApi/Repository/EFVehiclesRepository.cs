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
    public class EFVehiclesRepository : EFRepositoryBase
    {
        private const string CONTEXT_GENERETOR_ID = "Vehicles";

        private IQueryable<VehicleEty> VehiclesNoTracking => _context.Vehicles.Include(x => x.Client).AsNoTracking();

        public EFVehiclesRepository(UnitWork unitWork) : base(unitWork)
        {         
        }

        public List<Vehicle> Get(FilterParams filterParams)
        {
            var vehicles = VehiclesNoTracking.Where(x => x.CompanyId == _info.CompanyId).Select(x => _unitWork.Mapper.Map<Vehicle>(x)).ToList();

            return vehicles;
        }

        public Vehicle GetById(int id)
        {
            var vehicle = VehiclesNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            return _unitWork.Mapper.Map<Vehicle>(vehicle);
        }

        public Vehicle Create(Vehicle vehicle)
        {
            checkDuplicateVehicle(vehicle);

            if (vehicle.CityId.HasValue && vehicle.CityId <= 0)
                throw new AppValidationException("Informe uma cidade válida");

            if (vehicle.CountyId.HasValue && vehicle.CountyId <= 0)
                throw new AppValidationException("Informe uma cidade válida");

            var client = _context.Clients.Where(x => x.CompanyId == _info.CompanyId && x.Id == vehicle.ClientId).SingleOrDefault();
            if (client == null)
                throw new AppValidationException("Cliente vinculado ao veículo não encontrado");

            var _vehicle = _unitWork.Mapper.Map<VehicleEty>(vehicle);
            _vehicle.CompanyId = _info.CompanyId;
            _vehicle.Id = _unitWork.GenereteId(CONTEXT_GENERETOR_ID);

            _context.Vehicles.Add(_vehicle);
            _context.SaveChanges();

            var dbVehicle = VehiclesNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == _vehicle.Id).Single();
            return _unitWork.Mapper.Map<Vehicle>(dbVehicle);
        }

        public Vehicle Update(Vehicle vehicle)
        {            
            var dbVehicle = _context.Vehicles.Include(x => x.Client).Where(x => x.CompanyId == _info.CompanyId && x.Id == vehicle.Id).Single();
            if (dbVehicle == null)
                throw new AppValidationException("Veículo não encontrado");

            if (vehicle.CityId.HasValue && vehicle.CityId <= 0)
                throw new AppValidationException("Informe uma cidade válida");

            if (vehicle.CountyId.HasValue && vehicle.CountyId <= 0)
                throw new AppValidationException("Informe uma cidade válida");

            checkDuplicateVehicle(vehicle);

            dbVehicle.Model = vehicle.Model;
            dbVehicle.Plate = vehicle.Plate;
            dbVehicle.Manufacturer = vehicle.Manufacturer;
            dbVehicle.YearManufacture = vehicle.YearManufacture;
            dbVehicle.ModelYear = vehicle.ModelYear;
            dbVehicle.Chassis = vehicle.Chassis;
            dbVehicle.Renavam = vehicle.Renavam;
            dbVehicle.Color = vehicle.Color;
            dbVehicle.CityId = vehicle.CityId;
            dbVehicle.CityName = vehicle.CityName;
            dbVehicle.CountyId = vehicle.CountyId;
            dbVehicle.CountyName = vehicle.CountyName;
            dbVehicle.CountyInitials = vehicle.CountyInitials;
            dbVehicle.Type = (VehicleTypeEty)vehicle.Type;
            _context.SaveChanges();

            return _unitWork.Mapper.Map<Vehicle>(dbVehicle);
        }

        public void Delete(int id)
        {
            var dbVehicle = _context.Vehicles.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            if (dbVehicle == null)
                throw new AppValidationException("Veículo não encontrado");

            try
            {
                _context.Vehicles.Remove(dbVehicle);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new AppValidationException("Não é possível excluir o veículo porque existe registros vinculados ao mesmo");
            }
        }


        private void checkDuplicateVehicle(Vehicle vehicle)
        {
            var duplicate = _context.Vehicles.Where(x => x.CompanyId == _info.CompanyId && x.Id != vehicle.Id && x.Plate == vehicle.Plate).Any();
            if (duplicate)
                throw new AppValidationException("Já existe um veículo cadastrado com essa placa");

            if (vehicle.Chassis != null)
            {
                duplicate = _context.Vehicles.Where(x => x.CompanyId == _info.CompanyId && x.Id != vehicle.Id && x.Chassis == vehicle.Chassis).Any();
                if (duplicate)
                    throw new AppValidationException("Já existe um veículo cadastrado com esse chassi");
            }

            if (vehicle.Renavam != null)
            {
                duplicate = _context.Vehicles.Where(x => x.CompanyId == _info.CompanyId && x.Id != vehicle.Id && x.Renavam == vehicle.Renavam).Any();
                if (duplicate)
                    throw new AppValidationException("Já existe um veículo cadastrado com esse renavam");
            }
        }
    }
}
