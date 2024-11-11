using BLL.DI;
using DAL.DI;
using DAL.Persistanse;
using DAL.Persistanse.Hubs;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Presentation.Middlewares;
using System.Net;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// IP configuration
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8082);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()  
    .WriteTo.File("/var/log/fridgeservice/logs.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddBLLDependencies(builder.Configuration.GetConnectionString("HangFire"));
builder.Services.AddDALDependencies(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // ��������� ������� URL ��� ��������
    c.AddServer(new OpenApiServer
    {
        Url = "/fridge"
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!dbContext.Database.CanConnect())
    {
        dbContext.Database.Migrate();
    }
}

app.UseCors("AllowLocalhost3000");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Hangfire configuration
// Configure Authorization fro production
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = Array.Empty<IDashboardAuthorizationFilter>()
});



// Hangfire configuration
JobScheduler.ConfigureJobs(app.Services.GetRequiredService<IServiceScopeFactory>());

app.Use((context, next) =>
{
    var pathBase = context.Request.Headers["X-Forwarded-PathBase"];
    if (!StringValues.IsNullOrEmpty(pathBase))
    {
        context.Request.PathBase = new PathString(pathBase);
    }
    return next();
});

app.UseHangfireDashboard();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/NotificationHub");

app.MapControllers();

app.Run();

public partial class Program { }