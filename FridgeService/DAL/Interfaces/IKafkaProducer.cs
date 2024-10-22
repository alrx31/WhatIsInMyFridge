namespace DAL.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(string topic, string message);

        Task ProduceAsync<T>(string topic, T Object);
    }
}
