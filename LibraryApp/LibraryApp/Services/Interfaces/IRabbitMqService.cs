public interface IRabbitMqService
{
    void Publish<T>(T message, string queueName);
}