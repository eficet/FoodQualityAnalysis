using System.Net.Http.Json;
using FoodQualityAnalysis.Common.DTOs;
using Newtonsoft.Json;
using QualityManager.Enums;

namespace FoodQualityAnalysis.Integration.Tests;

public class FoodQualityIntegrationTest : IClassFixture<TestEnvironmentFixture>
{
    private readonly HttpClient _clientQualityManager;
    
    public FoodQualityIntegrationTest()
    {
        _clientQualityManager = new HttpClient { BaseAddress = new Uri("http://localhost:5004") };
    }

    [Fact]
    public async Task TestFoodQualityAnalysisFlow()
    {
        var foodBatchRequest = new FoodBatchRequest
        {
            FoodName = "Apple",
            SerialNumber = TestHelper.GenerateRandomAlphanumeric(10),
            AnalysisType = "GMO"
        };
        _clientQualityManager.DefaultRequestHeaders.Add("Accept", "application/json");
        var response = await _clientQualityManager.PostAsJsonAsync("/api/food", foodBatchRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<FoodBatchResponse>(responseString);
        
        //test FoodQuality service response
        Assert.Equal(foodBatchRequest.FoodName, result.FoodName);
        Assert.Equal(foodBatchRequest.SerialNumber, result.SerialNumber);
        Assert.Equal(foodBatchRequest.AnalysisType, result.AnalysisType);
        Assert.Equal(FoodBatchStatus.Pending.ToString(), result.Status);
        
        //simulate time for Analysis engine and Food Quality service to process the messages
        await Task.Delay(4000);
        
        var statusResponse = await _clientQualityManager.GetAsync($"/api/food/status/{foodBatchRequest.SerialNumber}");
        statusResponse.EnsureSuccessStatusCode();
        var statusString = await statusResponse.Content.ReadAsStringAsync();
        Assert.Contains(FoodBatchStatus.Completed.ToString(), statusString);
    }
}