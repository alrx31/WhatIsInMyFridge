using BLL.DI;
using DAL.DI;
using DAL.Persistanse;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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



JobScheduler.ConfigureJobs(app.Services.GetRequiredService<IServiceScopeFactory>());


app.MapHangfireDashboard();
app.MapControllers();

app.Run();
