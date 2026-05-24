using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Controllers;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Tests;

public class OpsPulseApiControllerTests
{
    [Fact]
    public async Task GetHealthSummary_ReturnsOkResult()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await using var db = await CreateSeededDbAsync(connection);
        var controller = new OpsPulseApiController(db);

        var result = await controller.GetHealthSummary();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task ExportFailedJobs_ReturnsCsvFile()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await using var db = await CreateSeededDbAsync(connection);
        var controller = new OpsPulseApiController(db);

        var result = await controller.ExportFailedJobs();

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);

        var csv = Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("Site,JobName,JobType,LastRunUtc,LastStatus,FailureMessage", csv);
    }

    [Fact]
    public async Task ExportFailedJobs_PrefixesFormulaLikeCsvValues()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await using var db = await CreateSeededDbAsync(connection);

        db.MaintenanceJobs.Add(new MaintenanceJob
        {
            SiteId = 1,
            JobName = "Injected Formula Test",
            JobType = "Backup",
            LastRunUtc = DateTime.UtcNow,
            LastStatus = "Failed",
            FailureMessage = "=HYPERLINK(\"http://example.local\")"
        });
        await db.SaveChangesAsync();

        var controller = new OpsPulseApiController(db);
        var result = await controller.ExportFailedJobs();

        var fileResult = Assert.IsType<FileContentResult>(result);
        var csv = Encoding.UTF8.GetString(fileResult.FileContents);

        Assert.Contains("\"'=HYPERLINK(\"\"http://example.local\"\")\"", csv);
    }

    private static async Task<AppDbContext> CreateSeededDbAsync(SqliteConnection connection)
    {
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        SeedData.Initialize(db);

        return db;
    }
}
