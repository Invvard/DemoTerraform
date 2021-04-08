using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using InvvardDev.DemoTerraform.BddTests.Drivers;
using TechTalk.SpecFlow;

namespace InvvardDev.DemoTerraform.BddTests.StepDefinitions
{
    [Binding]
    [Scope(Tag = DoWePushTag)]
    public class DoWePushSteps
    {
        private const string DoWePushTag = "functionTest";
        private const string DateParameterNameKey = "dateParameterName";
        private const string DateParameterValueKey = "dateParameterValue";
        private const string RequestVerbKey = "requestVerb";
        private const string ResponseKey = "response";
        private readonly ScenarioContext _scenarioContext;
        private readonly RequestDriver _driver;

        public DoWePushSteps(ScenarioContext scenarioContext, RequestDriver driver)
        {
            _scenarioContext = scenarioContext;
            _driver = driver;
        }

        [Given(@"the query string parameter '(.*)'")]
        public void GivenTheQueryStringParameter(string queryStringParamName)
        {
            _scenarioContext[DateParameterNameKey] = queryStringParamName;
            _scenarioContext[RequestVerbKey] = "GET";
        }

        [Given(@"the body content parameter '(.*)'")]
        public void GivenTheBodyContentParameter(string bodyParamName)
        {
            _scenarioContext[DateParameterNameKey] = bodyParamName;
            _scenarioContext[RequestVerbKey] = "POST";
        }

        [Given(@"the date '(.*)'")]
        public void GivenTheDate(string parameterValue)
        {
            _scenarioContext[DateParameterValueKey] = parameterValue;
        }

        [When(@"I get the function response")]
        public async Task WhenIGetTheFunctionResponse()
        {
            string paramName = _scenarioContext[DateParameterNameKey].ToString();
            string paramValue = _scenarioContext[DateParameterValueKey].ToString();

            var getApiUrl = _driver.GetApiUrl(paramName, paramValue);
            Console.WriteLine($"Sending request to {getApiUrl}");

            var response = await _driver.RequestGetDoWePushApi(getApiUrl);
            _scenarioContext[ResponseKey] = response;
        }

        [When(@"I post the function response")]
        public async Task WhenIPostTheFunctionResponse()
        {
            string paramName = _scenarioContext[DateParameterNameKey].ToString();
            string paramValue = _scenarioContext[DateParameterValueKey].ToString();

            var postBody = _driver.GetBody(paramName, paramValue);
            var response = await _driver.RequestPostDoWePushApi(postBody);

            _scenarioContext[ResponseKey] = response;
        }

        [Then(@"the request result should be '(.*)'")]
        public void ThenTheRequestResultShouldBe(string expectedQueryResult)
        {
            var response = (HttpResponseMessage)_scenarioContext[ResponseKey];

            response.StatusCode.ToString().Should().BeEquivalentTo(expectedQueryResult);
            response.IsSuccessStatusCode.Should().BeTrue();
        }
        
        [Then(@"the response message should be '(.*)'")]
        public void ThenTheResultShouldBe(string expectedResponseMessage)
        {
            Console.WriteLine("Result comparison");
            var response = _scenarioContext[ResponseKey];
            string actualResponse = "No message received";
            switch (_scenarioContext[RequestVerbKey])
            {
                case "GET":
                    actualResponse = response.ToString();
                    break;
                case "POST":
                    actualResponse = ((HttpResponseMessage) response).Content.ReadAsStringAsync().Result;
                    break;
            }

            actualResponse.Should().BeEquivalentTo(expectedResponseMessage);
        }

        [BeforeScenario()]
        public void BeforeScenario()
        {
            Console.WriteLine($"Starting {_scenarioContext.ScenarioInfo.Title} scenario");
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            Console.WriteLine($"Ending {_scenarioContext.ScenarioInfo.Title} scenario");
        }
    }
}