using System.Text.Json;
using FoodQualityAnalysis.Common.Utilities.CustomAttributes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FoodQualityAnalysis.Common.MessageBroker;

public abstract class MessageConsumer<T> : BackgroundService
{
    private readonly IMessageConnection _messageConnection;
    private IChannel _channel;
    private readonly string _queueName;
    private readonly ILogger<MessageConsumer<T>> _logger;

    protected MessageConsumer(IMessageConnection messageConnection, ILogger<MessageConsumer<T>> logger)
    {
        _messageConnection = messageConnection;
        _logger = logger;

        // Get the queue name from the QueueName attribute
        if (Attribute.GetCustomAttribute(GetType(), typeof(QueueNameAttribute)) is not QueueNameAttribute queueNameAttribute)
        {
            throw new InvalidOperationException($"Queue name is not specified for {GetType().Name}. Use the [QueueName] attribute.");
        }

        _queueName = queueNameAttribute.QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _messageConnection.Connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(body);
                if (message != null)
                {
                    _logger.LogInformation($"Started processing - {_queueName}");
                    await HandleMessage(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing {_queueName}: {ex.Message}");
            }
        };

        // Start consuming messages
        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        // Wait until cancellation is requested
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public abstract Task HandleMessage(T message);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);
            _channel.Dispose();
        }

        await base.StopAsync(cancellationToken);
    }
}