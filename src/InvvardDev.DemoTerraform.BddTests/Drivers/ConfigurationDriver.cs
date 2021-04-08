using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace InvvardDev.DemoTerraform.BddTests.Drivers
{
    public class ConfigurationDriver
    {
        private const string ConfigurationBasePath = @"Settings";
        private const string AppSettingsTargetFileKey = "appSettingsTargetFile";
        private const string AzureFunctionBaseUrlKey = "azureFunctionBaseUrl";

        private readonly Lazy<IConfiguration> _configurationLazy;
        private readonly Lazy<IConfiguration> _fileConfigurationLazy;

        public ConfigurationDriver()
        {
            _fileConfigurationLazy = new Lazy<IConfiguration>(GetFileConfiguration);
            _configurationLazy = new Lazy<IConfiguration>(GetTestConfiguration);
        }

        public IConfiguration Configuration => _configurationLazy.Value;

        public string AzureFunctionBaseUrl => Configuration[AzureFunctionBaseUrlKey];

        private IConfiguration GetFileConfiguration()
        {
            return GetConfiguration($"{ConfigurationBasePath}/base-appsettings.json");
        }

        private IConfiguration GetTestConfiguration()
        {
            var targetFileName = _fileConfigurationLazy.Value[AppSettingsTargetFileKey];

            Console.WriteLine($"Loading {targetFileName} configuration file");

            return GetConfiguration($"{ConfigurationBasePath}/{targetFileName}");
        }

        private IConfiguration GetConfiguration(string filePath)
        {
            var configurationBuilder = new ConfigurationBuilder();

            string directoryName = Path.GetDirectoryName(typeof(ConfigurationDriver).Assembly.Location);
            configurationBuilder.AddJsonFile(Path.Combine(directoryName, filePath));

            return configurationBuilder.Build();
        }
    }
}