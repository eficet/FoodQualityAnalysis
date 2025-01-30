namespace FoodQualityAnalysis.Common;

public interface IMessageConsumer
{
    public Task StartConsuming(string queueName, Func<string, Task> handler);
}