using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FoodQualityAnalysis.Common.MessageBroker;

public class MessageProducer : IMessageProducer
{
    private readonly IMessageConnection _connection;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IMessageConnection connection, ILogger<MessageProducer> logger)
    {
        _connection = connection;
        _logger = logger;
    }
    public async Task SendMessage<T>(string queueName, T message)
    {
        try
        {
            var channel = await _connection.Connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, false,false,false,null);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            await channel.BasicPublishAsync("", queueName, body);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            Console.WriteLine(e);
        }
    }
}