using Microsoft.Extensions.Configuration;

namespace TrelloAutomation.Utilities;

public class ConfigManager
{
    public static IConfiguration GetConfigs()
    {
        // Load configuration
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }
}
