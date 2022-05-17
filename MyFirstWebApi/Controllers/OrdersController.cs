using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyFirstWebApi.Models;

namespace MyFirstWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly AppDbContext _context;
        private const string APPROVED = "Approved";
        private const string REJECTED = "Rejected";
        private const string REVIEW = "Review";
        private const string NEW = "New";

        public OrdersController(AppDbContext context) {
            _context = context;
        }

        [HttpGet("ordersgt100")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderGt100() {
            if (_context.Orders == null) {
                return NotFound();
            }
            return await _context.Orders
                                    .Where(x => x.Total > 100)
                                    .Include(x => x.Customer)
                                    .Include(x => x.Orderlines)
                                    .ToListAsync();
            //var orders = from o in _context.Orders
            //             join ol in _context.Orderlines
            //                on o.Id equals ol.OrderId
            //             join c in _context.Customers
            //                on o.CustomerId equals c.Id
            //             where o.Total > 100
            //             select o;
            //return await orders.ToListAsync();
        }

        [HttpGet("reviews/{customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderInReview(int customerId) {
            if (_context.Orders == null) {
                return NotFound();
            }
            return await _context.Orders
                                    .Include(x => x.Customer)
                                    .Where(x => x.Status == REVIEW && x.CustomerId != customerId)
                                    .ToListAsync();
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
            if (_context.Orders == null) {
                return NotFound();
            }
            return await _context.Orders.Include(x => x.Customer).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id) {
            if (_context.Orders == null) {
                return NotFound();
            }
            var order = await _context.Orders.Include(x => x.Customer)
                                            .Include(x => x.Orderlines)
                                            .SingleOrDefaultAsync(x => x.Id == id);

            if (order == null) {
                return NotFound();
            }

            return order;
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveOrder(int id, Order order) {
            order.Status = APPROVED;
            return await PutOrder(id, order);
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectOrder(int id, Order order) {
            order.Status = REJECTED;
            return await PutOrder(id, order);
        }

        [HttpPut("review/{id}")]
        public async Task<IActionResult> ReviewOrder(int id, Order order) {
            order.Status = (order.Total <= 100) ? APPROVED : REVIEW;
            return await PutOrder(id, order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order) {
            if (id != order.Id) {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order) {
            if (_context.Orders == null) {
                return Problem("Entity set 'AppDbContext.Orders'  is null.");
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id) {
            if (_context.Orders == null) {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id) {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
