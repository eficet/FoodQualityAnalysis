using FoodQualityAnalysis.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using QualityManager.Interfaces;

namespace QualityManager.Controllers;

[ApiController]
[Route("api/food")]
public class FoodQualityController : ControllerBase
{
    private readonly IFoodQualityService _foodQualityService;

    public FoodQualityController(IFoodQualityService foodQualityService)
    {
        _foodQualityService = foodQualityService;
    }

    [HttpGet("status/{serialnumber}")]
    public async Task<IActionResult> GetStatusBySerialNumber(string serialnumber)
    {
        var result = await _foodQualityService.GetStatusBySerialNumber(serialnumber);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> ProcessFoodBatch([FromBody]FoodBatchRequest foodBatch)
    {
        var response = await _foodQualityService.ProcessFoodBatch(foodBatch);
        return Ok(response);
    }
    
}