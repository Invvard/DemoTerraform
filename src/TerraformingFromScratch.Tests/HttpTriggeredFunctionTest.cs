using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InvvardDev.DemoTerraform.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;
using TerraformingFromScratch.Tests.utils;

namespace TerraformingFromScratch.Tests
{
    public class HttpTriggeredFunctionTest
    {
        private const string MissingQueryStringMessage =
            "This HTTP triggered function executed successfully. Pass a name in the query string for a personalized response.";

        private const string MissingBodyMessage =
            "This HTTP triggered function executed successfully. Pass a name in the body for a personalized response.";

        private HttpRequest CreateRequest(string bodyParamName, string paramValue)
        {
            HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = new MemoryStream();
            var bodyJson = $"{{\"{bodyParamName}\":\"{paramValue}\"}}";

            var writer = new StreamWriter(request.Body);
            writer.Write(bodyJson);
            writer.Flush();
            request.Body.Position = 0;

            return request;
        }


        [Fact]
        public async Task GetName_WhenNoQueryString_ReturnCorrectResponse()
        {
            // Arrange
            var logger = TestFactory.CreateLogger();
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>())
            };

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(MissingQueryStringMessage, ((OkObjectResult) response).Value);
        }

        [Theory]
        [InlineData("", "", MissingQueryStringMessage)]
        [InlineData("other", "", MissingQueryStringMessage)]
        [InlineData("name", "", MissingQueryStringMessage)]
        [InlineData("name", "Luke", "Hello, Luke. This HTTP triggered function executed successfully.")]
        public async Task GetName_ReturnCorrectResponse(string queryStringParamName, string queryStringValue,
            string expectedMessage)
        {
            // Arrange
            var logger = TestFactory.CreateLogger();
            var queryStrings = new Dictionary<string, StringValues>
            {
                {queryStringParamName, queryStringValue}
            };
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(queryStrings)
            };

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedMessage, ((OkObjectResult) response).Value);
        }

        [Fact]
        public void GetName_should_log_message()
        {
            // Arrange
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            // Act
            var response = HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.Equal(1, logger.Logs.Count);
            Assert.Contains("HTTP Trigger - getName function", logger.Logs[0]);
        }

        [Fact]
        public async void PostName_WhenNoQueryString_ReturnCorrectResponse()
        {
            // Arrange
            var logger = TestFactory.CreateLogger();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(MissingBodyMessage, ((OkObjectResult) response).Value);
        }

        [Theory]
        [InlineData("", "", MissingBodyMessage)]
        [InlineData("other", "", MissingBodyMessage)]
        [InlineData("name", "", MissingBodyMessage)]
        [InlineData("name", "Luke", "Hello, Luke. This HTTP triggered function executed successfully.")]
        public async void PostName_ReturnCorrectResponse(string bodyParamName, string bodyParamValue,
            string expectedMessage)
        {
            // Arrange
            var logger = TestFactory.CreateLogger();
            var request = CreateRequest(bodyParamName, bodyParamValue);

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedMessage, ((OkObjectResult) response).Value);
        }

        [Fact]
        public async void PostName_should_log_message()
        {
            // Arrange
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.Equal(1, logger.Logs.Count);
            Assert.Contains("HTTP Trigger - postName function", logger.Logs[0]);
        }
    }
}