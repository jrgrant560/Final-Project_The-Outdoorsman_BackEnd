 using Microsoft.EntityFrameworkCore;

// Links the model and controller to tell the database how to use information
//    sent to and from it.
 namespace OutdoorsmanBackend.Models
 {
     public class OrderContext : DbContext
     {
         public OrderContext(DbContextOptions<OrderContext> options)
             : base(options)
         {
         }

         public DbSet<Order> Orders { get; set; }
     }
 }