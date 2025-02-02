using Microsoft.Extensions.Configuration;

namespace FoodQualityAnalysis.Common.Utilities;

public static class ConfigurationHelper
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetQueueName(string key)
    {
        return _configuration?.GetSection("Rabbitmq:Queues")[key] ?? string.Empty;
    }
}