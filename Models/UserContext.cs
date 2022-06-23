using Microsoft.EntityFrameworkCore;

// Links the model and controller to tell the database how to use information
//    sent to and from it.
namespace OutdoorsmanBackend.Models
{

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}