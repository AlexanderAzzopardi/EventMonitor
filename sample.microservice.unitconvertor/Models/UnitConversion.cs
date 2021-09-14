using System;
using System.Collections.Generic;

namespace sample.microservice.unitconvertor.Models
{
    public class ConvertObject
    {
        public double value { get; set; }
        public string unitType { get; set; }
        public string unitStart { get; set; }
        public string unitEnd { get; set; }
    }
}
