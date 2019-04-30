using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Domain.Entities;
using Base.WebApp.Contracts.Network;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly UnitWork _unit;

        public ServiceOrdersController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            var order = _unit.Orders.GetById(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("")]
        public IActionResult GetOrders([FromQuery]FilterParams filterParams)
        {
            var orders = _unit.Orders.Get(filterParams);

            return Ok(orders);
        }

        [HttpPost("")]
        public IActionResult CreateOrder([FromBody] OrderService model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Orders.Create(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpPut("")]
        public IActionResult UpdateOrder([FromBody] OrderService model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.Orders.Update(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }
    }
}
