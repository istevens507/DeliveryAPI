using DeliveryAPI.Data;
using DeliveryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderController(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await _dbContext.Orders.ToListAsync());
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return Ok(new { status = HttpStatusCode.NotFound });
            }

            return Ok(order);
        }

        // POST api/<OrderController> 
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(Order order)
        {
            try
            {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    orderId = order.Id,
                    status = HttpStatusCode.OK
                });
            }

            catch (Exception)
            {
                return Ok(new { status = HttpStatusCode.InternalServerError });
            }


        }

        //// PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return Ok(HttpStatusCode.BadRequest);
            }

            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dbContext.Orders.FindAsync(id) == null)
                {
                    return Ok(new { status = HttpStatusCode.NotFound });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = HttpStatusCode.NoContent });
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return Ok(new { status = HttpStatusCode.NotFound });
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok(new { status = HttpStatusCode.NoContent });
        }
    }
}
