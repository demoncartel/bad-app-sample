using LegacyOrders.Api.Data;
using LegacyOrders.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LegacyOrders.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _orders;

        public OrdersController(OrderRepository orders)
        {
            _orders = orders;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Sync-over-async: blocks the request thread on an async database call.
            IReadOnlyList<Order> orders = _orders.GetAllAsync().Result;
            return Content(JsonConvert.SerializeObject(orders), "application/json");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Order order = _orders.GetByIdAsync(id).Result;
            if (order == null)
            {
                return NotFound();
            }

            return Content(JsonConvert.SerializeObject(order), "application/json");
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            Order created = _orders.CreateAsync(order).GetAwaiter().GetResult();
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
