using Microsoft.Playwright;
namespace TrelloAutomation.Pages;

/// <summary>
/// Base class for all page objects in the automation framework. 
/// </summary>
public abstract class BasePage
{
    protected readonly IPage _page;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class with the specified Playwright page.
    /// </summary>
    /// <param name="page"></param>
    public BasePage(IPage page) => _page = page ?? throw new ArgumentNullException(nameof(page));

    /// <summary>
    /// Navigate to the specific URL.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public virtual async Task GotoAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("URL cannot be null or empty.", nameof(url));
        await _page.GotoAsync(url);
    }
    /// <summary>
    /// Get the title of the current page.
    /// </summary>
    /// <returns></returns>
    public virtual async Task<string> PageTitleAsync() => await _page.TitleAsync();
}