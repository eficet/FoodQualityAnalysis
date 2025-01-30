using AutoMapper;
using FoodQualityAnalysis.Common.DTOs;
using QualityManager.Data.Entities;

namespace QualityManager.AutoMapper;

public class FoodBatchProfile : Profile
{
    public FoodBatchProfile()
    {
        CreateMap<FoodBatchRequest, FoodBatch>();
        CreateMap<FoodBatch, FoodBatchResponse>();
    }
}