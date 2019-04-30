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
    public class ClientsController : ControllerBase
    {
        private readonly UnitWork _unit;

        public ClientsController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetClient(int id)
        {
            var client = _unit.Clients.GetById(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpGet("")]
        public IActionResult GetClients([FromQuery]FilterParams filterParams)
        {
            var clients = _unit.Clients.Get(filterParams);

            return Ok(clients);
        }

        [HttpPost("")]
        public IActionResult CreateClient([FromBody]Client model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var client = _unit.Clients.Create(model);
                _unit.CommitTransaction();

                return Ok(client);
            }
            else
                return ValidationProblem();
        }

        [HttpPut("")]
        public IActionResult UpdateClient([FromBody] Client model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var client = _unit.Clients.Update(model);
                _unit.CommitTransaction();

                return Ok(client);
            }
            else
                return ValidationProblem();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteClient(int id)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                _unit.Clients.Delete(id);
                _unit.CommitTransaction();

                return Ok(null);
            }
            else
                return ValidationProblem();
        }
    }
}
