using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QualityManager.Enums;

namespace QualityManager.Data.Entities;

public class FoodBatch
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string FoodName { get; set; }
    [Required]
    public string SerialNumber { get; set; }
    [Required]
    public AnalysisType AnalysisType { get; set; }
    public FoodBatchStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Result { get; set; }
    
    
}