using System;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TerraformingFromScratch.BddTests.Drivers;

namespace TerraformingFromScratch.BddTests.StepDefinitions
{
    [ Binding ]
    [ Scope(Tag = GetNameTag) ]
    public class GetNameSteps
    {
        private const string GetNameTag = "getName";
        private const string GetNameRequestUrlKey = "getNameRequestUrl";
        private const string ResponseKey = "response";
        private readonly ScenarioContext _scenarioContext;
        private readonly GetNameDriver _driver;

        public GetNameSteps(ScenarioContext scenarioContext, GetNameDriver driver)
        {
            _scenarioContext = scenarioContext;
            _driver = driver;
        }

        [ Given(@"I have assigned '(.*)' to the query string parameter '(.*)'") ]
        public void GivenIHaveAssignedLukeTheQueryStringParameterName(string paramValue, string paramName)
        {
            _scenarioContext[GetNameRequestUrlKey] = _driver.GetApiUrl(paramName, paramValue);
        }

        [Given(@"I send an incorrect '(.*)' in the query string")]
        public void GivenISendAnIncorrectInTheQueryString(string paramName)
        {
            _scenarioContext[GetNameRequestUrlKey] = _driver.GetApiUrl(paramName);
        }

        [ When(@"I request the GetName API") ]
        public async Task WhenIRequestTheGetNameApi()
        {
            Console.WriteLine($"Sending request to {_scenarioContext[GetNameRequestUrlKey]}");

            _scenarioContext[ResponseKey] = await _driver.RequestGetNameApi(_scenarioContext[GetNameRequestUrlKey].ToString());
        }

        [ Then(@"the response should be '(.*)'") ]
        public void ThenTheResponseShouldBe(string expectedResponse)
        {
            Console.WriteLine("Result comparison");
            var actualResponse = _scenarioContext[ResponseKey].ToString();

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [ BeforeScenario ]
        public void BeforeScenario()
        {
            Console.WriteLine($"Starting {_scenarioContext.ScenarioInfo.Title} scenario");
        }

        [ AfterScenario("getName") ]
        public void AfterScenario()
        {
            Console.WriteLine($"Ending {_scenarioContext.ScenarioInfo.Title} scenario");
        }
    }
}