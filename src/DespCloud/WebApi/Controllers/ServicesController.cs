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
    public class ServicesController : ControllerBase
    {
        private readonly UnitWork _unit;

        public ServicesController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetService(int id)
        {
            var service = _unit.Services.GetById(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpGet("")]
        public IActionResult GetServices([FromQuery]FilterParams filterParams)
        {
            var services = _unit.Services.Get(filterParams);

            return Ok(services);
        }

        [HttpPost("")]
        public IActionResult CreateService([FromBody] Service model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Services.Create(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpPut("")]
        public IActionResult UpdateService([FromBody] Service model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Services.Update(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteService(int id)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                _unit.Services.Delete(id);
                _unit.CommitTransaction();

                return Ok(null);
            }
            else
                return ValidationProblem();
        }
    }
}
