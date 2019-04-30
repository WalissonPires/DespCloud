using Base.Domain.Entities;
using Base.WebApp.Contracts.Network;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly UnitWork _unit;

        public VehiclesController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _unit.Vehicles.GetById(id);
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        [HttpGet("")]
        public IActionResult GetVehicles([FromQuery]int? clientId, [FromQuery]FilterParams filterParams)
        {
            var vehicles = _unit.Vehicles.Get(filterParams);

            if (clientId.HasValue)
                vehicles = vehicles.Where(x => x.ClientId == clientId).ToList();

            return Ok(vehicles);
        }

        [HttpPost("")]
        public IActionResult CreateVehicle([FromBody] Vehicle model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Vehicles.Create(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpPut("")]
        public IActionResult UpdateVehicle([FromBody] Vehicle model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Vehicles.Update(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteVehicle(int id)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                _unit.Vehicles.Delete(id);
                _unit.CommitTransaction();

                return Ok(null);
            }
            else
                return ValidationProblem();
        }
    }
}
