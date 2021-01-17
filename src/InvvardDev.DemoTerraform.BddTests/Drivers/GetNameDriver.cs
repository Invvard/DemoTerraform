using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TerraformingFromScratch.BddTests.Drivers
{
    public class GetNameDriver
    {
        private readonly ConfigurationDriver _configurationDriver;
        private readonly string _getNameBaseUrl;

        public GetNameDriver(ConfigurationDriver configurationDriver)
        {
            _configurationDriver = configurationDriver;
            _getNameBaseUrl = $"{_configurationDriver.AzureFunctionBaseUrl}/api/getName";

            Console.WriteLine($"GetName API URL : {_getNameBaseUrl}");
        }

        public string GetApiUrl(string paramName, string paramValue = "")
        {
            if (!string.IsNullOrWhiteSpace(paramName))
            {
                return $"{_getNameBaseUrl}?{paramName}={paramValue}";
            }

            return _getNameBaseUrl;
        }

        public async Task<string> RequestGetNameApi(string apiUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(apiUrl);

            return response;
        }
    }
}