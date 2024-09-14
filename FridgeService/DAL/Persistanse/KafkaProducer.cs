using Confluent.Kafka;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Persistanse
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:9092",

            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync(string topic, string message)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }

        public async Task ProduceAsync<T>(string topic, T Object)
        {
            var message = JsonSerializer.Serialize(Object);

            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }
}
