namespace FoodQualityAnalysis.Common.DTOs;

public class AnalysisResultResponse
{
    public string SerialNumber { get; set; }
    public string Result { get; set; }
    public DateTime CreatedAt { get; set; }
}