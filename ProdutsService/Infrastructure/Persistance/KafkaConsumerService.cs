﻿using Confluent.Kafka;
using DAL.Entities.MessageBrokerEntities;
using Domain.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class KafkaConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILogger<KafkaConsumerService> logger
        )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "getProductsInfo-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new[] { _configuration["Kafka:Topic1"], _configuration["Kafka:Topic2"] });
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(stoppingToken);
                        _logger.LogInformation($"Consumed message '{cr.Value}' from '{cr.Topic}' at: '{cr.Offset}'.");

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var listManageRepository = scope.ServiceProvider.GetRequiredService<IListManageRepository>();
                            var listRepository = scope.ServiceProvider.GetRequiredService<IListRepository>();

                            if (cr.Topic == _configuration["Kafka:Topic1"])
                            {
                                var data = JsonSerializer.Deserialize<Product>(cr.Value);

                                var list = await listRepository.GetListbyFridgeId(data.FridgeId, stoppingToken);

                                if (list is not null)
                                {
                                    for (int i = 0; i < data.ProductId.Count; i++)
                                    {
                                        var product = await listManageRepository.GetProductInLlist(list.Id, data.ProductId[i].ProductId, stoppingToken);

                                        if (product is not null)
                                        {
                                            if (product.Count - data.ProductId[i].Count <= 0)
                                            {
                                                await listManageRepository.DeleteProductInList(list.Id, data.ProductId[i].ProductId, stoppingToken);
                                            }
                                            else
                                            {
                                                product.Count -= data.ProductId[i].Count;

                                                await listManageRepository.UpdateAsync(product, stoppingToken);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (cr.Topic == _configuration["Kafka:Topic2"])
                            {
                                var data = JsonSerializer.Deserialize<ProductRemove>(cr.Value);

                                var list = await listRepository.GetListbyFridgeId(data.FridgeId, stoppingToken);

                                if (list is null)
                                {
                                    list = new Domain.Entities.ProductsList
                                    {
                                        FridgeId = data.FridgeId,
                                        Name = $"list {data.FridgeId}",
                                        BoxNumber = 0,
                                        Price = 0,
                                        Weight = 0,
                                        HowPackeges = 0,
                                        CreateData = DateTime.Now
                                    };

                                    await listRepository.AddAsync(list, stoppingToken);
                                }

                                var productInList = await listManageRepository.GetProductInLlist(list.Id, data.ProductId, stoppingToken);

                                if (productInList is not null)
                                {
                                    productInList.Count += data.Count;
                                    
                                    await listManageRepository.UpdateAsync(productInList, stoppingToken);
                                }
                                else
                                {
                                    var newProduct = new Domain.Entities.ProductInList
                                    {
                                        ListId = list.Id,
                                        Count = data.Count,
                                        ProductId = data.ProductId
                                    };

                                    await listManageRepository.AddAsync(newProduct, stoppingToken);
                                }
                            }
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error occurred: {e.Error.Reason}");
                }
                finally
                {
                    consumer.Close();
                }
            }
        }, stoppingToken);
    }
}
