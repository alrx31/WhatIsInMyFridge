using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;
using Presentation.ExceptionsHandlingMiddleware;
using Application.DI;
using Infastructure.DI;
using Presentation.DI;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});


// logs

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()  
    .WriteTo.File("/var/log/identityservice/logs.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder);
builder.Services.AddPresentationServices();




// Authentication and Authorization
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseCors("AllowLocalhost3000");

// Apply migrations at application startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
// TODO: delete true for production
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // This should be in UseEndpoints to map HTTP controllers
    endpoints.MapGrpcService<GreeterService>(); // Map your gRPC services
});

app.Run();
