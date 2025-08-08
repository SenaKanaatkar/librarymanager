
using LibraryApp.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Serilog;
 

namespace LibraryApp.Consumers
{
    public class LogConsumerService : BackgroundService
    {
       
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            {
                channel.QueueDeclare(queue: "logQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);
                    var logDto = JsonSerializer.Deserialize<LogDto>(messageJson);

                  if (logDto != null)
    {
        Log.Information("{Message} | Level: {Level} | Action: {Action} | Details: {Details} | Timestamp: {Timestamp}",
            logDto.Message, logDto.Level, logDto.Action, logDto.Details, logDto.Timestamp);

        Console.WriteLine($"Log saved to file: {logDto.Message} at {logDto.Timestamp}");
    }
                };
                channel.BasicConsume(queue: "logQueue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Log consumer started and waiting for messages...");
            }
            return Task.CompletedTask;
        }
        
    }

   
}



