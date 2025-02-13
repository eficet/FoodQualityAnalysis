using System.Net;
using AutoMapper;
using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.DTOs;
using FoodQualityAnalysis.Common.Utilities;
using FoodQualityAnalysis.Common.Utilities.Error;
using Microsoft.EntityFrameworkCore;
using QualityManager.Data;
using QualityManager.Data.Entities;
using QualityManager.Enums;
using QualityManager.Interfaces;

namespace QualityManager.Services;

public class FoodQualityService : IFoodQualityService
{
    private QualityManagerContext _context;
    private IMessageProducer _producer;
    private ILogger<FoodQualityService> _logger;
    private IMapper _mapper;

    public FoodQualityService(QualityManagerContext context, IMessageProducer producer, ILogger<FoodQualityService> logger, IMapper mapper)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
        _mapper = mapper;
    }
        
    public async Task<FoodBatchResponse> ProcessFoodBatch(FoodBatchRequest foodBatch)
    {
        if (!Enum.TryParse(foodBatch.AnalysisType, out AnalysisType analysisType))
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Invalid analysis type");
        }
        
        var foodBatchEntity = new FoodBatch
        {
            FoodName = foodBatch.FoodName,
            Status = FoodBatchStatus.Pending,
            SerialNumber = foodBatch.SerialNumber,
            SubmittedAt = DateTime.Now,
            AnalysisType = analysisType
        };

        await _context.AddAsync(foodBatchEntity);
        await _context.SaveChangesAsync();
        await _producer.SendMessage(ConfigurationHelper.GetQueueName("food_analysis_queue"), foodBatch);
        return _mapper.Map<FoodBatchResponse>(foodBatchEntity);
    }

    public async Task<string> GetStatusBySerialNumber(string serialNumber)
    {
        var foodBatchEntity =await _context.FoodBatches.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber);
        if (foodBatchEntity == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest,"Invalid serial number");
        }

        return $"Processing {foodBatchEntity.Status} : {foodBatchEntity.Result}";
    }
}