
using Application.DTO;
using Application.Services;
using Domain.Repository;
using EventManagement.Middlewares;
using Identity.Infrastructure;
using Infastructure.Persistanse;
using Infastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Controllers;
using System.Text;
using Application.Validators;
using FluentValidation.AspNetCore;
using StackExchange.Redis;
using Application.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// autoMapper

builder.Services.AddAutoMapper(typeof(UserProfile));

//Redis

builder.Services.AddSingleton<IConnectionMultiplexer>(s =>
{

    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));

});


builder.Services.AddDbContext<ApplicationDbContext>(op =>
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Identity")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:issuer"],
        ValidAudience = builder.Configuration["Jwt:audience"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});


// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheRepository, CacheRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJWTService, JWTService>();


// validators

builder.Services.AddControllers();
builder.Services.AddFluentValidation(fv=>{
    fv.RegisterValidatorsFromAssemblyContaining<LoginDTOValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<RegisterDTOValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<LogoutDTOValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<RefreshTokenDTOValidator>();
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
