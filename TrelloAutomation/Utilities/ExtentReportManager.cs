using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TrelloAutomation.Utilities;

public class ExtentReportManager
{
    private static ExtentReports? _extent;
    private static ExtentSparkReporter? _htmlReporter;

    /// <summary>
    /// Sets up the report file, configurations, and adds system information.
    /// </summary>
    public static ExtentReports GetExtent()
    {
        if (_extent == null)
        {
            // Set the project directory path and define the reports folder
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string reportsFolder = Path.Combine(projectDir, "Reports");

            // Ensure the Reports directory exists
            Directory.CreateDirectory(reportsFolder);

            // Define the report path with a timestamp
            string reportPath = Path.Combine(reportsFolder, $"ExtentReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

            _htmlReporter = new ExtentSparkReporter(reportPath);
            _htmlReporter.Config.DocumentTitle = "Trello Test Report";
            _htmlReporter.Config.ReportName = "Trello Test Execution Report";

            _extent = new ExtentReports();
            _extent.AttachReporter(_htmlReporter);
            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("User", "Tester");
        }
        return _extent;
    }

    /// <summary>
    /// Flushes the ExtentReports instance to write the report data to the output file.
    /// </summary>
    public static void FlushReport()
    {
        _extent?.Flush();
    }
}
