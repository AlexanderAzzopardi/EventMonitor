using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapr;
using Dapr.Client;
using eventmonitor.Models;

//dapr run --app-id eventmonitor --app-port 5020 -d ..\components\ -- dotnet run --urls http://*:5020

namespace eventmonitor.Controllers
{
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        [Topic("pubsub","mytopic")]
        [Route("/process")]
        [HttpPost]
        public void Run([FromBody] object body)
        {
            try
            {
                using JsonDocument dataJsonDoc = JsonDocument.Parse(body.ToString());
                JsonElement dataJsonElement = dataJsonDoc.RootElement;
                string dataJsonProp = dataJsonElement.GetProperty("data").ToString();
                string dataJsonString = dataJsonProp.Replace("\\u0022", "\"");
                JObject dataJsonObject = JObject.Parse(dataJsonString.Substring(1, dataJsonString.Length-2));
                
                System.Console.WriteLine($"Recieved: {dataJsonObject}");
                
                ConvertObject convertObject = ObjectBuilder(dataJsonObject);
                convertObject.unitEnd = StateTester(convertObject.value);
                ConvertInvoker(convertObject);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }  

        [Route("objectbuilder")]
        public ConvertObject ObjectBuilder(JObject dataJsonObject)
        {   
            ConvertObject convertObject = new ConvertObject();
            convertObject.value = dataJsonObject.GetValue("value").ToObject<double>();
            convertObject.unitType = dataJsonObject.GetValue("unitType").ToObject<string>();
            convertObject.unitStart = dataJsonObject.GetValue("unitStart").ToObject<string>();
            convertObject.unitEnd = dataJsonObject.GetValue("unitEnd").ToObject<string>();
            return convertObject;
        }

        [Route("statetester")]
        public string StateTester(double value)
        {
            if(value >= 1000)
            {
                return "km";
            }
            else if (value < 1)
            {
                return "mm";
            }
            return "m";
        }

        [Route("convertinvoker")]
        public async void ConvertInvoker(ConvertObject convertObject)
        {
            var client = new DaprClientBuilder().Build();
            await client.InvokeMethodAsync<ConvertObject>("unit-conversion", "convert", convertObject);
        }
    }
}
