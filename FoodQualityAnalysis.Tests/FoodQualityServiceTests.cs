using AutoMapper;
using FoodQualityAnalysis.Common;
using FoodQualityAnalysis.Common.DTOs;
using FoodQualityAnalysis.Common.Utilities.Error;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using QualityManager.Data;
using QualityManager.Data.Entities;
using QualityManager.Services;

namespace FoodQualityAnalysis.Tests;

public class FoodQualityServiceTests
{
    private  FoodQualityService _foodQualityService;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILogger<FoodQualityService>> mockLogger;
    private readonly Mock<IMessageProducer> mockMessageProducer;
    private readonly DbContextOptions<QualityManagerContext> options;
    private readonly QualityManagerContext context;

    public FoodQualityServiceTests()
    {
        mockLogger = new Mock<ILogger<FoodQualityService>>();
        mockMessageProducer = new Mock<IMessageProducer>();
        mockMapper = new Mock<IMapper>();
        options = new DbContextOptionsBuilder<QualityManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        context = new QualityManagerContext(options);
        _foodQualityService = new FoodQualityService(context, mockMessageProducer.Object, mockLogger.Object, mockMapper.Object);
    }
    [Fact]
    public async Task FoodBatchInsertTest()
    {
        var foodbatch = new FoodBatchRequest()
        {
            SerialNumber = "UDY2VQMJ12",
            AnalysisType = "Allergenic",
            FoodName = "Apple"
        };
        var foodBatchResponse = new FoodBatchResponse()
        {
            SerialNumber = "UDY2VQMJ12",
            AnalysisType = "Allergenic",
            Status = "Pending"
        };
        mockMapper.Setup(m => m.Map<FoodBatchResponse>(It.IsAny<FoodBatch>()))
            .Returns(foodBatchResponse);
        var result = await _foodQualityService.ProcessFoodBatch(foodbatch);
        Assert.Equal(foodBatchResponse.SerialNumber, result.SerialNumber);
        Assert.Equal(foodBatchResponse.AnalysisType, result.AnalysisType);
        Assert.Equal(foodBatchResponse.Status, result.Status);

    }
    
    [Fact]
    public async Task ProcessFoodBatchAsync_InvalidInput_ThrowsApiException()
    {
        var request = new FoodBatchRequest
        {
            FoodName = "food",
            SerialNumber = "f12300f212",
            AnalysisType = "Wrong Input" // Invalid input
        };

        await Assert.ThrowsAsync<ApiException>(() => _foodQualityService.ProcessFoodBatch(request));
    }
}