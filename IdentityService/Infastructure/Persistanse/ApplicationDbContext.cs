using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistanse
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {}

        public DbSet<User> users { get; set; }
        public DbSet<RefreshTokenModel> refreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd(); 
            });

            modelBuilder.Entity<RefreshTokenModel>(entity =>
            {
                entity.HasKey(e => e.id); 
                entity.Property(e => e.id).ValueGeneratedOnAdd(); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}
