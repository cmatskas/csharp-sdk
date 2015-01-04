# The relayr C# SDK

The relayr C# SDK allows you to access all of that sweet relayr sensor data and [cloud platform](https://developer.relayr.io/documents/Welcome/Platform) functionality from, you guessed it, apps built in C#!

Because it's built as a portable class library, you'll be able to use the same SDK across Windows 8.1, Windows Phone 8.1, and Windows Desktop Apps.

# Getting Started

The C# SDK has two main functions: 

1. Programatically exposing the relayr HTTP API. This is done through the HttpManager class.
2. Allowing applications to subscribe to the streams of data published by sensor modules. The Transmitter and Device classes handle this functionality. You will need security strings obtained through the HttpManager to subscribe to data topics.

### Before you Begin

The HttpManager requires a OAuth token to authorize your HTTP requests. You can obtain a token in two ways:

1. Through a proper OAuth authentication flow. Due to the platform-specific nature of performing this task, the C# SDK does not have the ability to execute this process for you. If you choose to get a token using this method, you will need to implement it yourself. Documentation on how the relayr OAuth system works can be found in [our documentation center](https://developer.relayr.io/documents/Welcome/OAuthReference).
2. By generating one through the relayr Dashboard, on the [API Keys](https://developer.relayr.io/dashboard/apps/myApps) page. Create an app (or use an existing one), and you will see a Generate Token button. Clicking this will create the required OAuth token. Once the token is generated, you can revoke it by clicking the button again. This value can be hard-coded into your app, and allows you access to any information and data from sensors associated with YOUR relayr account. This is extremely useful for prototyping and personal-use apps, but prevents you from wide-spread distribution of your app. You will need to use Method One if you wish to distribute your application.

### Subscribing to a Sensor

Subscribing to data published by a sensor is simple, and can be accomplished in just a few lines of code.

1. Set the Oauth Token, and obtain the ID of the user it specifies.

        // Set the OAuth token for the requests
        HttpManager.Manager.OauthToken = "YOUR OAUTH TOKEN HERE";
            
        // Obtain the userId
        HttpResponseMessage userInfoResponse = await HttpManager.Manager.PerformHttpOperation(
                                                    ApiCall.UserGetInfo, null, null);
        dynamic userInfo = await HttpManager.Manager.ConvertResponseContentToObject(userInfoResponse);
        string userId = (string) userInfo["id"];

2. Use the userId to get a list of Transmitters (WunderBars) that the user owns.

        // Get a list of Transmitters
        HttpResponseMessage transmittersResponse = await HttpManager.Manager.PerformHttpOperation(
                                                        ApiCall.TransmittersListByUser,
                                                        new string[] { userId }, null);
        dynamic transmitters = await HttpManager.Manager.ConvertResponseContentToObject(transmittersResponse);

3. Create a Transmitter object for one of the transmitters and connect to the relayr MQTT broker. In this case, we're using the first transmitter returned, although there could be many in the JSON response.

        // Get the ID and Secret for the transmitter. These will be your credentials for the MQTT Broker
        string transmitterId = (string) transmitters[0]["id"];
        string transmitterSecret = (string) transmitters[0]["secret"];
        
        // Connect to the broker
        Transmitter transmitter = new Transmitter(transmitterId);
        transmitter.ConnectToBroker("SOME ID FOR YOURSELF", transmitterSecret);

4. Subscribe to a data stream for a sensor

        // Subscribe to a sensor. Add an event handler for incoming data
        Device device = transmitter.SubscribeToDeviceData("SENSOR ID");
        device.PublishedDataReceived += device_PublishedDataReceived;

5. Implement an event handler for new data. Start reading values!

        // Event handler for incoming data
        void device_PublishedDataReceived(object sender, PublishedDataReceivedEventArgs args)
        {
            // Do something with the data here. Data is contained inside of args.
        }
