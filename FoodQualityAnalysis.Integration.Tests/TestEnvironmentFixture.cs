using Microsoft.Data.SqlClient;

namespace FoodQualityAnalysis.Integration.Tests;

public class TestEnvironmentFixture : IAsyncLifetime
{
    private readonly string connectionString =
        "Server=localhost,1433;Database=FoodQualityAnalysisTestDb;User Id=sa;Password=Test123!;TrustServerCertificate=True";
    public async Task InitializeAsync()
    {
        await using var sql = new SqlConnection(connectionString);
        await sql.OpenAsync();
        const string command = @"
            ALTER TABLE FoodBatches NOCHECK CONSTRAINT ALL;
            ALTER TABLE AnalysisResults NOCHECK CONSTRAINT ALL;
            DELETE FROM FoodBatches;
            DELETE FROM AnalysisResults;
            ALTER TABLE FoodBatches CHECK CONSTRAINT ALL;
            ALTER TABLE AnalysisResults CHECK CONSTRAINT ALL;";

        await using var sqlCommand =  new SqlCommand(command, sql);
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}