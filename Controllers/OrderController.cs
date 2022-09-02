using DeliveryAPI.Data;
using DeliveryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryAPI.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<Order>> AddOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new
                    {
                        status = HttpStatusCode.BadRequest,
                        Message = "model state order is invalid"
                    });
                }

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

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrder updateOrder)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(updateOrder.Description) || string.IsNullOrWhiteSpace(updateOrder.Address)){

                message = "Please provide description or address to update";
            }
            else if (!ModelState.IsValid)
            {
                message = "model state order is invalid";
            }

            if (!string.IsNullOrWhiteSpace(message)) {

                return Ok(new
                {
                    status = HttpStatusCode.BadRequest,
                    Message = message
                });
            }

            var orderDb = await _dbContext.Orders.FindAsync(id);

            if (orderDb == null)
            {
                return Ok(new { status = HttpStatusCode.NotFound, 
                                Message = string.Format("order id {0} not found", id) });
            }

            if (!string.IsNullOrWhiteSpace(updateOrder.Description))
                orderDb.Description = updateOrder.Description;


            if (!string.IsNullOrWhiteSpace(updateOrder.Address))
                orderDb.Address = updateOrder.Address;

            orderDb.UpdateBy = updateOrder.UpdateBy;
            orderDb.UpdatedOn = DateTime.Now;

            _dbContext.Entry(orderDb).State = EntityState.Modified;

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
