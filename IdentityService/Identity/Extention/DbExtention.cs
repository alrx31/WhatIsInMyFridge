using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;

namespace Identity.Extention
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var assemblyName = configuration.GetSection("MigrationsAssembly").Get<string>();
            services.AddDbContext<ApplicationDbContext>(
                opt => opt.UseNpgsql(connectionString,
                                     npgsqlOpt => npgsqlOpt.MigrationsAssembly(assemblyName)));
        }

        public static void ApplyMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
