using OpsPulse.Web.Services;

namespace OpsPulse.Tests;

public class JobHealthServiceTests
{
    [Fact]
    public void Classify_ReturnsCritical_WhenJobFailed()
    {
        var now = new DateTime(2026, 5, 21, 12, 0, 0, DateTimeKind.Utc);

        var result = JobHealthService.Classify(now.AddHours(-1), "Failed", now);

        Assert.Equal("Critical", result);
    }

    [Fact]
    public void Classify_ReturnsWarning_WhenJobOlderThan24Hours()
    {
        var now = new DateTime(2026, 5, 21, 12, 0, 0, DateTimeKind.Utc);

        var result = JobHealthService.Classify(now.AddHours(-25), "Succeeded", now);

        Assert.Equal("Warning", result);
    }

    [Fact]
    public void Classify_ReturnsHealthy_WhenJobSucceededRecently()
    {
        var now = new DateTime(2026, 5, 21, 12, 0, 0, DateTimeKind.Utc);

        var result = JobHealthService.Classify(now.AddHours(-2), "Succeeded", now);

        Assert.Equal("Healthy", result);
    }
}
