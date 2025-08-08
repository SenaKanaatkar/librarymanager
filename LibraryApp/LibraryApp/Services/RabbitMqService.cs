using LibraryApp.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqService: IRabbitMqService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqService()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost", 
            Port = 5672,            
            UserName = "guest",     
            Password = "guest"      
        };
    }
    public void PublishToLogQueue(LogDto log)
    {
        Publish (log, "logQueue");
    }
public void PublishToEmailQueue(EmailDto email) 
{
        Publish(email, "emailQueue");
    }
    

    public void Publish<T>(T message, string queueName)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body
        );
    }
}

