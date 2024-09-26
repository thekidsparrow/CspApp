using Microsoft.ApplicationInsights; // Import Application Insights for telemetry
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC components
using System.Collections.Generic; // Import for using Dictionary
using System.Threading.Tasks; // Import tasks for asynchronous programming

// Define the API controller for handling CSP violation reports
[ApiController]
[Route("csp-violation-report-endpoint")] // Set the base route for this controller
public class CspController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient; // Declare a TelemetryClient to send data to Application Insights

    // Constructor that accepts a TelemetryClient instance, enabling logging to Application Insights
    public CspController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient; // Assign the injected TelemetryClient to the class-level variable
    }

    // HTTP POST endpoint for reporting CSP violations
    [HttpPost]
    public async Task<IActionResult> Report([FromBody] CspReport report) // Takes a CspReport object from the request body
    {
        if (report == null)
        {
            return BadRequest("Report cannot be null."); // Return a bad request if the report is null
        }

        // Log the CSP violation details to Application Insights
        _telemetryClient.TrackEvent("CspViolation", new Dictionary<string, string>
        {
            { "DocumentUri", report.DocumentUri ?? "Unknown" }, // Log the document URI or "Unknown" if null
            { "Referrer", report.Referrer ?? "Unknown" }, // Log the referrer or "Unknown" if null
            { "BlockedUri", report.BlockedUri ?? "Unknown" }, // Log the blocked URI or "Unknown" if null
            { "ViolatingDirective", report.ViolatingDirective ?? "Unknown" }, // Log the violating directive or "Unknown" if null
        });

        // Return an Ok response to indicate success
        return Ok(); // No need to wrap in Task.FromResult, just return Ok directly
    }
}

// Class representing the structure of a CSP report
public class CspReport
{
    public string? DocumentUri { get; set; } // The URI of the document that triggered the CSP violation, nullable
    public string? Referrer { get; set; } // The referrer URI, nullable
    public string? BlockedUri { get; set; } // The URI that was blocked, nullable
    public string? ViolatingDirective { get; set; } // The directive that was violated, nullable
}
