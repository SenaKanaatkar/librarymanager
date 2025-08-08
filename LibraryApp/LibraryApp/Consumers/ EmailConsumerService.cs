using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using LibraryApp.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Net;


namespace LibraryApp.Consumers
{
    public class EmailConsumerService : BackgroundService
    {
        

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "emailQueue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var emailDto = JsonSerializer.Deserialize<EmailDto>(messageJson);
                if (emailDto != null)
                {
                     await SendEmailAsync(emailDto);
                    Console.WriteLine($"Email sent to: {emailDto.To} with subject: {emailDto.Subject}");
                }
            };
            channel.BasicConsume(queue: "emailQueue",
                                 autoAck: true,
                                 consumer: consumer);
            Console.WriteLine("Email consumer started and waiting for messages...");

             return Task.CompletedTask;
        }

        
            

private async Task SendEmailAsync(EmailDto emailDto)
{
    using var smtpClient = new SmtpClient("smtp.gmail.com", 587); 

    smtpClient.Credentials = new NetworkCredential("******@gmail.com", "uygulamaparolası**"); 
    smtpClient.EnableSsl = true;

    var mailMessage = new MailMessage
    {
        From = new MailAddress("*****2004@gmail.com"),
        Subject = emailDto.Subject,
        Body = emailDto.Body,
        IsBodyHtml = false
    };

    mailMessage.To.Add(emailDto.To);

    await smtpClient.SendMailAsync(mailMessage);
}

            };
        }
    
        
