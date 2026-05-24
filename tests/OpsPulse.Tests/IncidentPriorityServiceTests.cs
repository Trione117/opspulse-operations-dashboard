using OpsPulse.Web.Services;

namespace OpsPulse.Tests;

public class IncidentPriorityServiceTests
{
    [Fact]
    public void GetPriorityScore_ReturnsHigherScore_ForCriticalIncident()
    {
        var result = IncidentPriorityService.GetPriorityScore("Critical", 1);
        Assert.True(result >= 100);
    }

    [Fact]
    public void GetPriorityScore_Increases_WhenIncidentIsOlder()
    {
        var newer = IncidentPriorityService.GetPriorityScore("Medium", 1);
        var older = IncidentPriorityService.GetPriorityScore("Medium", 25);
        Assert.True(older > newer);
    }

    [Fact]
    public void GetBadgeCss_ReturnsDanger_ForHighSeverity()
    {
        var result = IncidentPriorityService.GetBadgeCss("High");
        Assert.Equal("danger", result);
    }
}
