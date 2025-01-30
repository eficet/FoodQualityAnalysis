namespace FoodQualityAnalysis.Common.Utilities.CustomAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class QueueNameAttribute : Attribute
{
    public string QueueName { get; }

    public QueueNameAttribute(string queueName)
    {
        QueueName = queueName;
    }
}