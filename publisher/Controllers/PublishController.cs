using System;
using System.Net.Http;
using System.Text;
using publisher.Models.PubSub;
using Dapr.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//dapr run --app-id "publisher" --app-port "5003" --dapr-http-port "5030" -d ..\components\ -- dotnet run --urls="http://+:5003"

namespace publisher.Controllers
{
    [ApiController]
    public class pubsubClass : ControllerBase
    {
        [HttpPost("publish")]
        public async void Publish(PubSubObject pubsub)
        {
            var daprClient = new DaprClientBuilder().Build();
            string dataJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pubsub.data);
            await daprClient.PublishEventAsync<string>(pubsub.subscriber.pubsubname, pubsub.subscriber.topic, dataJsonString);
            JObject dataJsonObject = JObject.Parse(dataJsonString);
            System.Console.WriteLine($"Sending: {dataJsonObject}");       
        }

        
    }
}
