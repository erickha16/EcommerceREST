using Microsoft.EntityFrameworkCore;

namespace EcommerceREST.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //DbSet para las entidades del modelo
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Brand> Brands { get; set; }

    }
}
