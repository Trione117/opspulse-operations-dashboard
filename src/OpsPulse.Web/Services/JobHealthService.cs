namespace OpsPulse.Web.Services;

public static class JobHealthService
{
    public static string Classify(DateTime lastRunUtc, string lastStatus, DateTime referenceUtc)
    {
        if (string.Equals(lastStatus, "Failed", StringComparison.OrdinalIgnoreCase))
        {
            return "Critical";
        }

        if (lastRunUtc < referenceUtc.AddHours(-24))
        {
            return "Warning";
        }

        if (string.Equals(lastStatus, "Warning", StringComparison.OrdinalIgnoreCase))
        {
            return "Warning";
        }

        return "Healthy";
    }

    public static string GetBadgeCss(string health)
    {
        return health switch
        {
            "Critical" => "danger",
            "Warning" => "warning",
            "Healthy" => "success",
            _ => "secondary"
        };
    }
}
