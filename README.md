# EventMonitor
The purpose of the code is to set up a network of dapr microservices which monitor values/objects via a pub/sub data stream and trigger events based off the pre-set conditions on those values/objects. Once data is sent to the publisher via a *http* request it published the data it recieves. The eventmonitor is subscribed to the publisher so recieves the data. Depending on what the data is it triggers an event to change the *unitEnd* which then invokes the unit convertor and converts the value.

# Prerequisites
#### Installation of Docker 
![Docker](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DockerLogo.jfif)
> <https://docs.docker.com/engine/install/>

#### Installation of Dapr 
![Dapr](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DaprLogo.jfif)
> <https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/getting-started>

# Redis PubSub
When setting up a redis store you need to create a .yaml file 

    apiVersion: dapr.io/v1alpha1
    kind: Component
    metadata:
      name: pubsub
    spec:
      type: pubsub.redis
      metadata:
      - name: redisHost
        value: localhost:6379
      - name: redisPassword
        value: ""

# Build and Run
To run each of the microservices you need to run the following commands in cli:

publisher

> dapr run --app-id publisher --app-port 5003 --dapr-http-port 5030 -d ..\components\ -- dotnet run --urls http://+:5003

eventmonitor

> dapr run --app-id eventmonitor --app-port 5002 -d ..\components\ -- dotnet run --urls http://*:5002 

unitconvertor

> dapr run --app-id unit-conversion --app-port 5000 --dapr-grpc-port 50010 --dapr-http-port 5010 -- dotnet run

# Commands
To run these commands you need the REST client extension installed.

The *subscriber* part of the *Json* file hold the information regarding the topics it need to publish to. In this case it is *mytopic*. In the *data* section of the *Json* file it contains the data which is being published.

    POST http://localhost:5030/v1.0/invoke/publisher/method/publish HTTP/1.1
    content-type: application/json

    {
        "subscriber" :
        {
            "pubsubname": "pubsub",
            "topic" : "mytopic"
        },
        "data" :
        {
            "value": 195.0,
            "unitType": "Length",
            "unitStart": "m"
        }
    }


