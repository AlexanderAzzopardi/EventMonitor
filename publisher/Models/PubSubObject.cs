using System;
using System.Collections.Generic;

namespace publisher.Models.PubSub
{
    public class PubSubObject
    {
        public SubscriberObject subscriber {get; set;}
        public DataObject data {get; set;}
    }

    public class SubscriberObject
    {
        public string pubsubname {get; set;}
        public string topic {get; set;}
    }

    public class DataObject
    {
        public double value {get; set;}
        public string unitType {get; set;}
        public string unitStart {get; set;}
        public string unitEnd {get; set;}
    }

}
