namespace FoodQualityAnalysis.Common;

public interface IMessageProducer
{
    public Task SendMessage<T>(string queueName,T message);
}