using FoodQualityAnalysis.Common.DTOs;
using QualityManager.Data.Entities;

namespace QualityManager.Interfaces;

public interface IFoodQualityService
{
    Task<FoodBatchResponse> ProcessFoodBatch(FoodBatchRequest foodBatch);
    Task<string> GetStatusBySerialNumber(string serialNumber);
}