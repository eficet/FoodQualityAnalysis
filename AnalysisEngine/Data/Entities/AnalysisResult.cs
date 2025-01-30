namespace AnalysisEngine.Data.Entities;

public class AnalysisResult
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Result { get; set; }
    public required DateTime CreatedAt { get; set; }
}