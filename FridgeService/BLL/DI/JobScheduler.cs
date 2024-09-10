using BLL.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
namespace BLL.DI;
public static class JobScheduler
{
    public static void ConfigureJobs(IServiceScopeFactory scopeFactory)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            var fridgeService = scope.ServiceProvider.GetRequiredService<IFridgeService>();

            recurringJobManager.AddOrUpdate(
                "check-products",                         
                () => fridgeService.CheckProducts(),      
                Cron.Daily                                
            );
        }
    }
}
