using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DAL.Persistanse;
using Microsoft.EntityFrameworkCore;
using DAL.Persistanse.Protos;
using DAL.IRepositories;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using DAL.Interfaces;
using DAL.Persistanse.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace DAL.DI
{
    public static  class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services,IConfiguration configuration)
        {
            // redis
            services.AddScoped<IConnectionMultiplexer>(_ =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"])), // Use your secret key
                        ValidateIssuer = false,  // Set to true if you want to validate the issuer
                        ValidateAudience = false, // Set to true if you want to validate the audience
                        ValidIssuer = "WhatIsInMyFridge", // Set your issuer here
                        ValidAudience = "WhatIsInMyFridge" // Set your audience here
                    };

                // You can add events to log or handle authorization failures here
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed.");
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        // Here you can handle the token passed via the request header
                        // Usually done in client-side JavaScript
                        return Task.CompletedTask;
                    }
                };
            });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFridgeRepository,FridgeRepository>();
            services.AddScoped<IgRPCService, gRPCService>();
            services.AddScoped<IProductsgRPCService, ProductsgRPCService>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<NotificationHub>();

            services.AddScoped<IKafkaProducer, KafkaProducer>();

            services.AddSignalR();

            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                //o.Address = new Uri("http://nginx/identity.Greeter");
                o.Address = new Uri("http://identityservice:8081");
            });

            services.AddGrpcClient<Products.ProductsClient>(o =>
            {
                //o.Address = new Uri("http://nginx/products.Products");
                o.Address = new Uri("http://productsservice:8083");
            });

            return services;
        }
    }
}
