using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InvvardDev.DemoTerraform.Api;
using InvvardDev.DemoTerraform.Tests.utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace InvvardDev.DemoTerraform.Tests
{
    public class HttpTriggeredFunctionTest
    {
        private const string OkResultMessage = "This is the way";
        private const string NotOkResultMessage = "This is madness !";

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
        public async Task DoWePush_WhenNoQueryStringOrBody_ReturnCorrectResponse()
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
            response.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult) response).Value.Should().BeEquivalentTo(
                DateTime.Today.DayOfWeek == DayOfWeek.Friday
                    ? NotOkResultMessage
                    : OkResultMessage);
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("other", "", "")]
        [InlineData("date", "", "")]
        [InlineData("date", "2021-01-01", NotOkResultMessage)]
        [InlineData("date", "2021-01-04", OkResultMessage)]
        public async Task DoWePush_WhenBody_ReturnCorrectAnswer(string bodyParamName, string bodyParamValue,
            string expectedMessage)
        {
            // Arrange
            var logger = TestFactory.CreateLogger();
            var request = CreateRequest(bodyParamName, bodyParamValue);
            if (string.IsNullOrWhiteSpace(expectedMessage))
            {
                expectedMessage = DateTime.Today.DayOfWeek == DayOfWeek.Friday
                    ? NotOkResultMessage
                    : OkResultMessage;
            }

            // Act
            var response = await HttpTriggeredFunction.Run(request, logger);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedMessage, ((OkObjectResult) response).Value);
        }
    }
}