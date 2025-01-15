using Microsoft.Playwright;

namespace TrelloAutomation.Pages;

/// <summary>
/// Login page elements and actions for authentication.
/// </summary>
public class LoginPage : BasePage
{ 
    private ILocator LoginLink => _page.Locator("//a[text()='Log in' and contains(@class, 'Buttonsstyles')]");
    private ILocator UserName => _page.Locator("#username");
    private ILocator Password => _page.Locator("#password");
    private ILocator LoginSubmitButton => _page.Locator("#login-submit");

    /// <summary>
    /// Contructor.
    /// </summary>
    /// <param name="page"></param>
    public LoginPage(IPage page) : base(page) { }

    /// <summary>
    /// Click the login link to start the process.
    /// </summary>
    /// <returns></returns>
    public async Task ClickLoginLinkAsync() => await LoginLink.ClickAsync();

    /// <summary>
    /// Enter email in the email field to login.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task EnterEmailAsync(string email) => await UserName.FillAsync(email);

    /// <summary>
    /// Enter user's password in the password field to login.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task EnterPasswordAsync(string password) => await Password.FillAsync(password);

    /// <summary>
    /// Click login submut button to login.
    /// </summary>
    /// <returns></returns>
    public async Task ClickLoginSubmitButtonAsync() => await LoginSubmitButton.ClickAsync();

    /// <summary>
    /// Logs in by performing a series of steps: clicking the login link, entering credentials, and submitting.
    /// </summary>
    /// <param name="email">User's email or username.</param>
    /// <param name="password">User's password.</param>
    public async Task LoginAsync(string username, string password)
    {
        await ClickLoginLinkAsync();
        await EnterEmailAsync(username);
        await ClickLoginSubmitButtonAsync();
        await EnterPasswordAsync(password);
        await ClickLoginSubmitButtonAsync();
    }
}
