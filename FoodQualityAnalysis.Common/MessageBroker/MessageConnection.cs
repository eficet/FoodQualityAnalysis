using RabbitMQ.Client;

namespace FoodQualityAnalysis.Common.MessageBroker;

public class MessageConnection : IMessageConnection, IDisposable
{
    private IConnection? _connection;
    public IConnection Connection => _connection ?? throw new InvalidOperationException("Connection is not initialized");
    

    public async Task InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            UserName = "admin",
            Password = "admin"
        };
        _connection = await factory.CreateConnectionAsync();

    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}