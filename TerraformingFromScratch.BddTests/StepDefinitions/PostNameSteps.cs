using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TerraformingFromScratch.BddTests.Drivers;
using Xunit.Abstractions;

namespace TerraformingFromScratch.BddTests.StepDefinitions
{
    [ Binding ]
    [ Scope(Tag = PostNameTag) ]
    public class PostNameSteps
    {
        private const string PostNameTag = "postName";
        private const string PostNameRequestBodyKey = "postNameRequestBody";
        private const string ResponseKey = "response";
        private readonly ScenarioContext _scenarioContext;
        private readonly PostNameDriver _driver;

        public PostNameSteps(ScenarioContext scenarioContext, PostNameDriver driver)
        {
            _scenarioContext = scenarioContext;
            _driver = driver;
        }

        [Given(@"I have assigned '(.*)' to the body parameter '(.*)'")]
        public void GivenIHaveAssignedToTheBodyParameter(string paramValue, string paramName)
        {
            _scenarioContext[PostNameRequestBodyKey] = _driver.GetBody(paramName, paramValue);
        }

        [Given(@"I send an incorrect '(.*)' in the body")]
        public void GivenISendAnIncorrectInTheBody(string paramName)
        {
            _scenarioContext[PostNameRequestBodyKey] = _driver.GetBody(paramName);
        }

        [When(@"I request the PostName API")]
        public async Task WhenIRequestThePostNameApi()
        {
            _scenarioContext[ResponseKey] = await _driver.RequestPostNameApi(_scenarioContext[PostNameRequestBodyKey].ToString());
        }

        [Then(@"the response should be '(.*)'")]
        public void ThenTheResponseShouldBe(string expectedResponse)
        {
            var response = (HttpResponseMessage) _scenarioContext[ResponseKey];

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.ReadAsStringAsync().Result.Should().BeEquivalentTo(expectedResponse);
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