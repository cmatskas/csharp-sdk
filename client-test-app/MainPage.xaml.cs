using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using relayr_csharp_sdk;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // (Un)comment the method you need to test

            //TestClient();
            TestClientDevices();
        }

        public async void TestClient()
        {
            Relayr.OauthToken = "YOUR OAUTH TOKEN";
            var Transmitters = await Relayr.GetTransmittersAsync();
            
            var transmitterInfo = Transmitters[0];
            var transmitter = Relayr.ConnectToBroker(transmitterInfo, "YOUR OAUTH CLIENT ID");
            var devices = await transmitter.GetDevicesAsync();

            Device d = await transmitter.SubscribeToDeviceDataAsync(devices[3]);
            d.PublishedDataReceived += d_PublishedDataReceived;

        }

        public async void TestClientDevices()
        {
            Relayr.OauthToken = "YOUR OAUTH TOKEN";
            var devices = await Relayr.GetDevicesAsync();
        }

        void d_PublishedDataReceived(object sender, PublishedDataReceivedEventArgs args)
        {
            // Do something with the data. The data is part of 'args'
        }
    }
}
