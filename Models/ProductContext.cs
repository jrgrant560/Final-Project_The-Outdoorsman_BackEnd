 using Microsoft.EntityFrameworkCore;

// Links the model and controller to tell the database how to use information
//    sent to and from it.
 namespace OutdoorsmanBackend.Models
 {
     public class ProductContext : DbContext
     {
         public ProductContext(DbContextOptions<ProductContext> options)
             : base(options)
         {
         }

         public DbSet<Product> Products { get; set; }
     }
 }