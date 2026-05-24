namespace OpsPulse.Web.Services;

public static class IncidentPriorityService
{
    public static int GetPriorityScore(string severity, int ageHours)
    {
        var baseScore = severity switch
        {
            "Critical" => 100,
            "High" => 75,
            "Medium" => 50,
            "Low" => 25,
            _ => 10
        };

        var ageBoost = ageHours switch
        {
            >= 24 => 20,
            >= 8 => 10,
            >= 2 => 5,
            _ => 0
        };

        return baseScore + ageBoost;
    }

    public static string GetBadgeCss(string severity)
    {
        return severity switch
        {
            "Critical" => "danger",
            "High" => "danger",
            "Medium" => "warning",
            "Low" => "secondary",
            _ => "secondary"
        };
    }
}
