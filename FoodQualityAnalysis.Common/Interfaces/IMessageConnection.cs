using RabbitMQ.Client;

namespace FoodQualityAnalysis.Common;

public interface IMessageConnection
{
    IConnection Connection { get; }
    Task InitializeConnection();
}