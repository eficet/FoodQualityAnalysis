using System.ComponentModel.DataAnnotations;

namespace FoodQualityAnalysis.Common.DTOs;

public class FoodBatchRequest
{
    [Required(ErrorMessage = "Food name is required.")]
    [StringLength(100, ErrorMessage = "Food name cannot exceed 100 characters.")]
    public string FoodName { get; set; }

    [Required(ErrorMessage = "Serial number is required.")]
    [RegularExpression(@"^[A-Z0-9]{10}$", ErrorMessage = "Serial number must be 10 alphanumeric characters.")]
    public string SerialNumber { get; set; }

    [Required(ErrorMessage = "Analysis type is required.")]
    public string AnalysisType { get; set; }
}