using API.ExceptionsHandlingMiddleware;
using Infrastructure.DI;
using Application.DI;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// add port
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8083, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    options.ListenAnyIP(8084, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()  
    .WriteTo.File("/var/log/productsservice/logs.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // ��������� ������� URL ��� ��������
    c.AddServer(new OpenApiServer
    {
        Url = "/products"
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

app.UseCors("AllowLocalhost3000");


// Configure the HTTP request pipeline.
// delete true for production
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<ProductgRPCService>();

app.UseMiddleware <ExceptionHandlingMiddleware>();

app.Run();
