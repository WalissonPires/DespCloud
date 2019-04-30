using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository;
using WebApi.Services.AddressSearch;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly UnitWork _unit;

        public AddressController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("Zipcode/{zipcode}")]
        public async Task<IActionResult> GetAddress(string zipcode, [FromServices]IServiceZipCodeQuery zipcodeService)
        {
            var address = await zipcodeService.Search(zipcode);

            if (address == null)
                return NotFound();

            _unit.Address.CheckAddress(address);

            return Ok(address);
        }

        [HttpGet("Countys")]
        public IActionResult GetCountys()
        {
            var countys = _unit.Address.GetCountys();
            return Ok(countys);
        }

        [HttpGet("County/{countyId:int}/Citys")]
        public IActionResult GetCitys(int countyId)
        {
            var citys = _unit.Address.GetCitys(countyId);
            return Ok(citys);
        }

        [HttpGet("City/{cityId:int}/districts")]
        public IActionResult GetDistricts(int cityId)
        {
            var districts = _unit.Address.GetDistricts(cityId);
            return Ok(districts);
        }        
    }
}
