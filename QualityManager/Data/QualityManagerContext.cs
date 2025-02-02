using Microsoft.EntityFrameworkCore;
using QualityManager.Data.Entities;

namespace QualityManager.Data;

public class QualityManagerContext : DbContext
{
    public QualityManagerContext(DbContextOptions<QualityManagerContext> options) : base(options) { }

    public DbSet<FoodBatch> FoodBatches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodBatch>()
            .HasIndex(s=>s.SerialNumber)
            .IsUnique();
    }
}