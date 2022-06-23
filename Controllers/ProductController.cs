using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutdoorsmanBackend.Models;

namespace OutdoorsmanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //links the following functions to a context file
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }
        // Http function that retrieves all products from a category
        // GET: api/Product/category/"Kayaking"
         [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategory(string category)
        {
            // filters the list for the category needed
            return await _context.Products.Where(c => c.category == category).ToListAsync();
        }



        // Http function that retrieves all products
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //returns all products
            return await _context.Products.ToListAsync();
        }

        //Http function that retrieves a specific product, by id
        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //loads the product into a var
            var product = await _context.Products.FindAsync(id);

            // returns a 404 "not found" error if the specified product is not found
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        //Http function to edit products in the data base
        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            //returns a "Bad Request" error message if produt id# doesn't exist
            if (id != product.id)
            {
                return BadRequest();
            }
            // Sets the targeted product state to "modified"
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                //saves changes to the database
                await _context.SaveChangesAsync();
            }
            // checks for concurrency issues
            catch (DbUpdateConcurrencyException)
            {
                //checks if product exists, if not returns 404 "Not Found" error
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                //if some other error is causing the concurrency issue, forwards the error message to the client
                else
                {
                    throw;
                }
            }
            //throws a 204 No Content error
            return NoContent();
        }

        //Http function to create a new product
        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //creates a new product
            _context.Products.Add(product);
            //saves created product into the database
            await _context.SaveChangesAsync();
            //returns 201 code with product info using the get product function
            return CreatedAtAction("GetProduct", new { id = product.id }, product);
        }

        //http function that deletes a product from the database
        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //searches for specified product
            var product = await _context.Products.FindAsync(id);
            //returns 404 "not found" error if no produt exists
            if (product == null)
            {
                return NotFound();
            }
            //changes state of the item to "deleted"
            _context.Products.Remove(product);
            //updates database, removing items with deleted state
            await _context.SaveChangesAsync();

            //returns 204 "No Content" error
            return NoContent();
        }

        //function that checks if a product with this order id exists
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
