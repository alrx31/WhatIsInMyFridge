using BLL.DI;
using DAL.DI;
using DAL.Persistanse;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// IP configuration
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8082); // Listen on port 8082
});

// Add services to the container.
builder.Services.AddBLLDependencies(builder.Configuration.GetConnectionString("HangFire"));
builder.Services.AddDALDependencies();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHangfireDashboard();


// Hangfire configuration
JobScheduler.ConfigureJobs(app.Services.GetRequiredService<IServiceScopeFactory>());


app.MapHangfireDashboard();
app.MapControllers();

app.Run();
