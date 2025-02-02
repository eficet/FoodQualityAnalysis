using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.DTOs;
using FoodQualityAnalysis.Common.MessageBroker;
using FoodQualityAnalysis.Common.Utilities.CustomAttributes;
using Microsoft.EntityFrameworkCore;
using QualityManager.Data;
using QualityManager.Enums;

namespace QualityManager.MessageHandlers;

[QueueName("food_analysis_response")]
public class FoodQualityAnalysisResponseConsumer: MessageConsumer<AnalysisResultResponse>
{
    private readonly ILogger<FoodQualityAnalysisResponseConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    public FoodQualityAnalysisResponseConsumer(IMessageConnection connection,IServiceProvider serviceProvider, ILogger<FoodQualityAnalysisResponseConsumer> logger) : base(connection, logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task HandleMessage(AnalysisResultResponse message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var datacontext = scope.ServiceProvider.GetRequiredService<QualityManagerContext>();
            var foodBatch = await datacontext.FoodBatches.FirstOrDefaultAsync(s => s.SerialNumber == message.SerialNumber);
            if (foodBatch != null)
            {
                foodBatch.Result = message.Result;
                foodBatch.Status = FoodBatchStatus.Completed;
                foodBatch.ApprovedAt = DateTime.Now;
                await datacontext.SaveChangesAsync();
            }
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}