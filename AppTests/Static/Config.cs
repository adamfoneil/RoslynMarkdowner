using Microsoft.Extensions.Configuration;

namespace AppTests.Extensions
{
    internal static class Config
    {
        internal static IConfigurationRoot GetConfig()
        {            
            return new ConfigurationBuilder()
                .AddJsonFile("Config/config.json")
                .Build();            
        }
    }
}
