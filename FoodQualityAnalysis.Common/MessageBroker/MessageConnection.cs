using FoodQualityAnalysis.Common.Utilities;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace FoodQualityAnalysis.Common.MessageBroker;

public class MessageConnection : IMessageConnection, IDisposable
{
    private IConnection? _connection;
    private readonly IConfiguration _configuration;
    public IConnection Connection => _connection ?? throw new InvalidOperationException("Connection is not initialized");

    public MessageConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "rabbitmq",
            UserName = _configuration["RabbitMQ:UserName"] ?? String.Empty,
            Password = _configuration["RabbitMQ:Password"] ?? String.Empty
        };
        _connection = await factory.CreateConnectionAsync();

    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}