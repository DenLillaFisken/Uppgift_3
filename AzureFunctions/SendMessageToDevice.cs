using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using SharedLibraries.Services;
using SharedLibraries.Models;

namespace AzureFunctions
{
    public static class SendMessageToDevice
    {
        private static readonly ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubConnection"));
        [FunctionName("SendMessageToDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string message = req.Query["message"];
            string deviceid = req.Query["deviceid"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<SendMessages>(requestBody);

            message = message ?? data?.Message;
            deviceid = deviceid ?? data?.DeviceId;

            await DeviceService.SendMessageToDeviceAsync(serviceClient, deviceid, message);

            return new OkResult();
        }
    }
}
