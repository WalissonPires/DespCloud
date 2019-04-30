using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    public class EFAddressRepository : EFRepositoryBase
    {
        public EFAddressRepository(UnitWork unitWork) : base(unitWork)
        {
        }

        public List<County> GetCountys()
        {
            var countys = _context.Countys.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => _unitWork.Mapper.Map<County>(x)).ToList();

            return countys;
        }

        public List<City> GetCitys(int countyId)
        {
            var citys = _context.Citys.AsNoTracking().Where(x => x.CountyId == countyId)
                .OrderBy(x => x.Name).Select(x => _unitWork.Mapper.Map<City>(x))
                .ToList();

            return citys;
        }

        public void CheckAddress(Address address)
        {
            if (!_context.Countys.Any(x => x.Id == address.CountyId))
            {
                _context.Countys.Add(new Database.Entities.AddressCountyEty
                {
                    Id = address.CountyId,
                    Initials = address.CountyInitials,
                    Name = address.CountyName
                });
                _context.SaveChanges();
            }

            if (!_context.Citys.Any(x => x.Id == address.CityId))
            {
                _context.Citys.Add(new Database.Entities.AddressCityEty
                {
                    Id = address.CityId,
                    CountyId = address.CountyId,
                    Name = address.CityName,
                });
                _context.SaveChanges();
            }

            var dbDistrict = _context.Districts.Where(x => x.CityId == address.CityId && x.Name == address.DistrictName).FirstOrDefault();
            if (dbDistrict == null)
            {
                dbDistrict = new Database.Entities.AddressDistrictEty
                {
                    CityId = address.CityId,
                    Name = String.IsNullOrEmpty(address.DistrictName) ? "Centro" : address.DistrictName
                };
                _context.Districts.Add(dbDistrict);
                _context.SaveChanges();

                address.DistrictName = dbDistrict.Name;
            }

            address.DistrictId = dbDistrict.Id;
        }

        public List<District> GetDistricts(int cityId)
        {
            var districts = _context.Districts.AsNoTracking().Where(x => x.CityId == cityId)
                .OrderBy(x => x.Name).Select(x => _unitWork.Mapper.Map<District>(x))
                .ToList();

            return districts;
        }
    }
}
