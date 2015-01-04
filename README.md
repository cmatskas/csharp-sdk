# The relayr C# SDK

The relayr C# SDK allows you to access all of that sweet relayr sensor data and [cloud platform](https://developer.relayr.io/documents/Welcome/Platform) functionality from, you guessed it, apps built in C#!

Because it's built as a portable class library, you'll be able to use the same SDK across Windows 8.1, Windows Phone 8.1, and Windows Desktop Apps.

# Getting Started

The C# SDK has two main functions: 

1. Programatically exposing the relayr HTTP API. This is done through the HttpManager class.
2. Allowing applications to subscribe to the streams of data published by sensor modules. The Transmitter and Device classes handle this functionality. You will need security strings obtained through the HttpManager to subscribe to data topics.

## Before you Begin

The HttpManager requires a OAuth token to authorize your HTTP requests. You can obtain a token in two ways:

1. Through a proper OAuth authentication flow. Due to the platform-specific nature of performing this task, the C# SDK does not have the ability to execute this process for you. If you choose to get a token using this method, you will need to implement it yourself. Documentation on how the relayr OAuth system works can be found in [our documentation center](https://developer.relayr.io/documents/Welcome/OAuthReference).
2. By generating one through the relayr Dashboard, on the [API Keys](https://developer.relayr.io/dashboard/apps/myApps) page. Create an app (or use an existing one), and you will see a Generate Token button. Clicking this will create the required OAuth token. Once the token is generated, you can revoke it by clicking the button again. This value can be hard-coded into your app, and allows you access to any information and data from sensors associated with YOUR relayr account. This is extremely useful for prototyping and personal-use apps, but prevents you from wide-spread distribution of your app. You will need to use Method One if you wish to distribute your application.

