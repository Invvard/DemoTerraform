using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TerraformingFromScratch.BddTests.Drivers
{
    public class PostNameDriver
    {
        private readonly ConfigurationDriver _configurationDriver;
        private readonly string _postNameBaseUrl;

        public PostNameDriver(ConfigurationDriver configurationDriver)
        {
            _configurationDriver = configurationDriver;
            _postNameBaseUrl = $"{_configurationDriver.AzureFunctionBaseUrl}/api/postName";

            Console.WriteLine($"PostName API URL : {_postNameBaseUrl}");
        }

        public string GetBody(string paramName, string paramValue = "")
        {
            if (!string.IsNullOrWhiteSpace(paramName))
            {
                return $"{{\"{paramName}\":\"{paramValue}\"}}";
            }

            return "{}";
        }

        public async Task<HttpResponseMessage> RequestPostNameApi(string jsonBody)
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(_postNameBaseUrl, new StringContent(jsonBody));

            return response;
        }
    }
}