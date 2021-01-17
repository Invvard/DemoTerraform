using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvvardDev.DemoTerraform.Api
{
    public static class HttpTriggeredFunction
    {
        [FunctionName("dowepush")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HTTP Trigger - DoWePush function");

            var weekDay = await GetWeekDay(req);
            
            var message = weekDay == DayOfWeek.Friday
                ? "This is madness !"
                : "This is the way";

            return new OkObjectResult(message);
        }

        private static async Task<DayOfWeek> GetWeekDay(HttpRequest req)
        {
            string dateString = req.Query["date"];

            if (string.IsNullOrWhiteSpace(dateString))
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                dateString ??= data?.date;
            }

            DayOfWeek weekDay = DateTime.Today.DayOfWeek;
            if (DateTime.TryParse(dateString, out var date))
            {
                weekDay = date.DayOfWeek;
            }

            return weekDay;
        }
    }
}