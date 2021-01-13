using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TerraformingFromScratch
{
    public static class HttpTriggeredFunction
    {
        [ FunctionName("GetName") ]
        public static IActionResult GetName([ HttpTrigger(AuthorizationLevel.Anonymous, "get") ]
                                            HttpRequest req,
                                            ILogger log)
        {
            log.LogInformation("HTTP Trigger - getName function");

            string name = req.Query["name"];

            string responseMessage = string.IsNullOrEmpty(name)
                                         ? "This HTTP triggered function executed successfully. Pass a name in the query string for a personalized response."
                                         : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("PostName")]
        public static async Task<IActionResult> PostName([ HttpTrigger(AuthorizationLevel.Anonymous, "post") ]
                                                         HttpRequest req,
                                                         ILogger log)
        {
            log.LogInformation("HTTP Trigger - postName function");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                                         ? "This HTTP triggered function executed successfully. Pass a name in the body for a personalized response."
                                         : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}