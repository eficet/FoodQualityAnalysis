using AnalysisEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnalysisEngine.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<AnalysisResult> AnalysisResults { get; set; }
}