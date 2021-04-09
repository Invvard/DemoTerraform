using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvvardDev.DemoTerraform.BddTests.Drivers
{
    public class RequestDriver
    {
        private readonly ConfigurationDriver _configurationDriver;
        private readonly string _doWePushBaseUrl;

        public RequestDriver(ConfigurationDriver configurationDriver)
        {
            _configurationDriver = configurationDriver;
            _doWePushBaseUrl = $"{_configurationDriver.AzureFunctionBaseUrl}/api/dowepush";

            Console.WriteLine($"DoWePush API URL : {_doWePushBaseUrl}");
        }

        public string GetApiUrl(string paramName, string paramValue = "")
        {
            if (!string.IsNullOrWhiteSpace(paramName))
            {
                return $"{_doWePushBaseUrl}?{paramName}={paramValue}";
            }

            return _doWePushBaseUrl;
        }

        public async Task<string> RequestGetDoWePushApi(string apiUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(apiUrl);

            return response;
        }

        public string GetBody(string paramName, string paramValue = "")
        {
            if (!string.IsNullOrWhiteSpace(paramName))
            {
                return $"{{\"{paramName}\":\"{paramValue}\"}}";
            }

            return "{}";
        }

        public async Task<HttpResponseMessage> RequestPostDoWePushApi(string jsonBody)
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_doWePushBaseUrl, new StringContent(jsonBody));

            return response;
        }
    }
}