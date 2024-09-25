using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("csp-violation-report-endpoint")]
public class CspController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public CspController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpPost]
    public async Task<IActionResult> Report([FromBody] CspReport report)
    {
        // Log the CSP violation to Application Insights
        _telemetryClient.TrackEvent("CspViolation", new Dictionary<string, string>
        {
            { "DocumentUri", report.DocumentUri },
            { "Referrer", report.Referrer },
            { "BlockedUri", report.BlockedUri },
            { "ViolatingDirective", report.ViolatingDirective },
        });

        return Ok();
    }
}

public class CspReport
{
    public string DocumentUri { get; set; }
    public string Referrer { get; set; }
    public string BlockedUri { get; set; }
    public string ViolatingDirective { get; set; }
}
