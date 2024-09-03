using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistanse
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {   
        }

        public DbSet<Fridge> fridges { get; set; }
        public DbSet<UserFridge> userFridges { get; set; }
        public DbSet<ProductFridgeModel> productFridgeModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configuration = new ApplicationDbContextConfiguration();

            modelBuilder.ApplyConfiguration<Fridge>(configuration);
            modelBuilder.ApplyConfiguration<UserFridge>(configuration);
            modelBuilder.ApplyConfiguration<ProductFridgeModel>(configuration);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
