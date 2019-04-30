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
    public class OrderServiceItemsController : ControllerBase
    {
        private readonly UnitWork _unit;

        public OrderServiceItemsController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOrderItem(int id)
        {
            var orderItem = _unit.OrderItems.GetById(id);
            if (orderItem == null)
                return NotFound();

            return Ok(orderItem);
        }

        [HttpGet("")]
        public IActionResult GetOrderItems([FromQuery]FilterParams filterParams)
        {
            var orderItems = _unit.OrderItems.Get(filterParams);

            return Ok(orderItems);
        }

        [HttpPost("")]
        public IActionResult CreateOrderItem([FromBody] OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.OrderItems.Create(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpPut("")]
        public IActionResult UpdateOrderItem([FromBody] OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.OrderItems.Update(model);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOrderItem(int id)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var order = _unit.OrderItems.Delete(id);
                _unit.CommitTransaction();

                return Ok(order);
            }
            else
                return ValidationProblem();
        }
    }
}
