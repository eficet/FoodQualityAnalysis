using AnalysisEngine.Data;
using AnalysisEngine.Data.Entities;
using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.DTOs;
using FoodQualityAnalysis.Common.MessageBroker;
using FoodQualityAnalysis.Common.Utilities;
using FoodQualityAnalysis.Common.Utilities.CustomAttributes;

namespace AnalysisEngine.MessageHandlers;

[QueueName("food_analysis_queue")]
public class FoodBatchAnalysisConsumer: MessageConsumer<FoodBatchRequest>
{
    private readonly IMessageProducer _producer;
    private readonly ILogger<FoodBatchAnalysisConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    public FoodBatchAnalysisConsumer(IMessageConnection messageConnection, IMessageProducer messageProducer, IServiceProvider serviceProvider, ILogger<FoodBatchAnalysisConsumer> logger) : base(messageConnection,logger)
    {
        _producer = messageProducer;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    

    public override async Task HandleMessage(FoodBatchRequest message)
    {
        try
        {
            await Task.Delay(2000);
            var result = $"{message.AnalysisType} is within limits.";
            var analysisResult = new AnalysisResult
            {
                SerialNumber = message.SerialNumber,
                Result = result,
                CreatedAt = DateTime.Now
            };
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.AddAsync(analysisResult);
            await context.SaveChangesAsync();
            await _producer.SendMessage(ConfigurationHelper.GetQueueName("food_analysis_response"), analysisResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " food_batch_analysis_queue -Error processing "+ex.Message);
        }
        
    }
}