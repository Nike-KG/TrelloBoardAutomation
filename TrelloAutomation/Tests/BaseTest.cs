using AventStack.ExtentReports;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using TrelloAutomation.Utilities;

namespace TrelloAutomation.Tests;

[TestFixture]
public class BaseTest
{
    
    private IPlaywright _playwright;
    private IBrowser _browser;

    protected static ExtentReports extent;
    protected static ExtentTest test;
    protected IPage page;
    protected IConfiguration configuration;

    /// <summary>
    /// One-time setup to initialize reporting, browser, and navigate to the start URL.
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        extent = ExtentReportManager.GetExtent();
        configuration = ConfigManager.GetConfigs();
        try
        {
            var browserType = configuration.GetValue<string>("BrowserSettings:BrowserType");
            var headless = configuration.GetValue<bool>("BrowserSettings:Headless");
            var viewportWidth = configuration.GetValue<int>("BrowserSettings:ViewportWidth");
            var viewportHeight = configuration.GetValue<int>("BrowserSettings:ViewportHeight");
            var url = configuration.GetValue<string>("Trello:BaseUrl") ?? throw new ArgumentNullException();

            _playwright = await Playwright.CreateAsync();
            var browserOption = new BrowserTypeLaunchOptions
            {
                Headless = headless,
                SlowMo = 50,
            };

            _browser = browserType switch
            {
                "Chromium" => await _playwright.Chromium.LaunchAsync(browserOption),
                "Firefox" => await _playwright.Firefox.LaunchAsync(browserOption),
                "Webkit" => await _playwright.Webkit.LaunchAsync(browserOption),
                _ => throw new ArgumentException("Invalid browser type specified in configuration.")
            };

            var contextOptions = new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = viewportWidth, Height = viewportHeight }
            };
             
            var context = await _browser.NewContextAsync(contextOptions);
            page = await context.NewPageAsync();

            // Create a new Extent Test
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            test.Log(Status.Info, $"Navigating to URL: {url}");
            await page.GotoAsync(url);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Failed to complete setup. Aborting tests. Due to: {ex.Message}");
            throw;
        }
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var errorMessage = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            test.Fail("Test failed");
            test.Fail(errorMessage);
        }
        else
        {
            test.Pass("Test passed");
        }
    }
    /// <summary>
    /// Ensures all resources are released, including browser, reports and Playwright, after all tests are completed.
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        try
        {
            await _browser.CloseAsync();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            _playwright.Dispose();
            ExtentReportManager.FlushReport();
        }
        
    }
}
