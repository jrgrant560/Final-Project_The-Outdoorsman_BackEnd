using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutdoorsmanBackend.Models;

namespace OutdoorsmanBackend.Controllers
{
    //this sets the parent route, in this case "/User"
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: User
        //HTTP function that retrieves the entire list of users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            //returns a list of all the users
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        //HTTP function that retrieves a specific user, by id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            //searches for a user with this id
            var user = await _context.Users.FindAsync(id);

            //if the id the user searched for isn't available, returns a NotFound error
            if (user == null)
            {
                return NotFound();
            }
            
            //returns the data for the specified user
            return user;
        }

        // GET: api/User/test@test.com
        //HTTP function that retrieves a specific user, by emailAddress
        [Authorize]
        [HttpGet("email/{emailAddress}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByEmail(string emailAddress)
        {
            //searches for a user with this emailAddress
            var user = await _context.Users.Where(e => e.emailAddress == emailAddress).ToListAsync();

            //if the id the user searched for isn't available, returns a NotFound error
            if (user == null)
            {
                return NotFound();
            }
            
            //returns the data for the specified user
            return user;
        }

        //POST: api/User
        //HTTP function that creates a new User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //adds a new user
            _context.Users.Add(user);
            //saves the changes to the database and waits until completed before proceeding
            await _context.SaveChangesAsync();
            //returns the user data that was just created, using the GetUser(int id) method, referencing the new id
            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user );
        }

        //PUT: api/User/5
        //HTTP function that updates data for an existing user, by id
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int? id, User user)
        {
            //returns a "Bad Request" Error message if the entered id doesn't exist
            if (id != user.id)
            {
                return BadRequest();
            }

            //sets the targeted user state to "Modified"
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //detects if a concurreny exception exists, and performs the following code
            {
                //if a user with this id doesn't exist, throws a NotFound error
                if (!UserExists(id))
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

        // DELETE: api/User/5
        //HTTP function that deletes a target User, by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            //searches for a user with this id
            var user = await _context.Users.FindAsync(id);
            //if the target specified user is unavailable, returns a NotFound code
            if (user == null)
            {
                return NotFound();
            }

            //removes the specified user
            _context.Users.Remove(user);
            //waits for the changes to be completed
            await _context.SaveChangesAsync();

            //returns a NoContent error
            return NoContent();
        }

        //function that checks if a User with this id exists
        private bool UserExists(int? id)
        {
            return _context.Users.Any(e => e.id == id);
        }

    }
}
