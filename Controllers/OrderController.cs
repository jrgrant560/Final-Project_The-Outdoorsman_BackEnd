using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutdoorsmanBackend.Models;

namespace OutdoorsmanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        //links the following functions to a context file
    public class OrderController : ControllerBase
    {
        
        private readonly OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        // Http function that retrieves all products from a category
        // GET: api/Order/user/"userId"
         [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> Getid(int userId)
        {
            // filters the list for the category needed       
            return await _context.Orders.Where(u => u.userId == userId).ToListAsync();
        }



        // Http function that retrieves all orders (Admin only)
        // GET: api/Order     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {

            //returns all products
            return await _context.Orders.ToListAsync();
        }

        //Http function that retrieves a specific order, by id
        // GET: api/Order/"5"
        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(int orderId)
        {
            //loads the order into a var
            var order = await _context.Orders.FindAsync(orderId);

            // returns a 404 "not found" error if the specified order is not found
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //Http function to create a new order
        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            //creates a new order
            _context.Orders.Add(order);
            //saves created order into database
            await _context.SaveChangesAsync();

                //sets new order to var order
                 order = new Order {
                    orderId = order.orderId,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    userId = order.userId,
                    firstName = order.firstName,
                    lastName = order.lastName,
                    streetAddress = order.streetAddress,
                    city = order.city,
                    state = order.state,
                    zipcode = order.zipcode,
                    orderProducts = order.orderProducts,
                    creditNumber = order.creditNumber,
                    expDate = order.expDate,
                    cvv = order.cvv,
                    totalPrice = order.totalPrice
                };
        
            // returns new order 
             return order;
        }

        //Http function that deletes an order from the database
        // function is for developer testing only,
        // will not be included in final version
        // DELETE: api/Order/"5"
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            // searches for specified order
            var order = await _context.Orders.FindAsync(orderId);
            //returns 404 "not found" error if no order exists
            if (order == null)
            {
                return NotFound();
            }
            //changes state of the item to "deleted"
            _context.Orders.Remove(order);
            //updates database, removing items with deleted state
            await _context.SaveChangesAsync();
            //returns 204 "No Content" error
            return NoContent();
        }

        //function that checks if an order with this order id exists

        private bool OrderExists(int orderId)
        {
            return _context.Orders.Any(e => e.orderId == orderId);
        }
    
    }
}
