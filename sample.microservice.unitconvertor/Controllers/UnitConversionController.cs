using UnitsNet;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using Dapr.Client;
using System.Collections.Generic;
using sample.microservice.unitconvertor.Models;

//dapr run --app-id unit-conversion --app-port 5000 --dapr-grpc-port 50010 --dapr-http-port 5010 -- dotnet run

namespace sample.microservice.unitconvertor.Controllers
{
    [ApiController]
    public class UnitConversionController : ControllerBase
    {
        [HttpPost("convert")]
        public void Convert(ConvertObject convertObject)
        {   
            Console.WriteLine(UnitConverter.ConvertByAbbreviation(convertObject.value, convertObject.unitType, convertObject.unitStart, convertObject.unitEnd).ToString() + convertObject.unitEnd);
        }
    }
}