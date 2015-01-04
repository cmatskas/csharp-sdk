using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using relayr_csharp_sdk;
using System.Net.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private int testCounter = 0;

        public MainPage()
        {
            this.InitializeComponent();

            testClient();
        }

        public async void testClient()
        {
            HttpManager.Instance.OauthToken = "c7UI1voiTqbqu1zbz2HROvR6kDaYtzZn";

            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("name", "Living room thermometer");
            content.Add("description", "This is a special humidity sensor");
            content.Add("public", "135");

            HttpResponseMessage userInfoResponse = await HttpManager.Instance.PerformHttpOperation(ApiCall.UserGetInfo, null, null);
            string userId = (string) (await HttpManager.Instance.ConvertResponseContentToObject(userInfoResponse))["id"];

            HttpResponseMessage transmittersResponse = await HttpManager.Instance.PerformHttpOperation(ApiCall.TransmittersListByUser, new string[] { userId }, null);
            dynamic transmitters = await HttpManager.Instance.ConvertResponseContentToObject(transmittersResponse);

            string transmitterId = (string) transmitters[1]["id"];
            string transmitterSecret = (string) transmitters[1]["secret"];

            bool connectSuccess = MqttManager.Subscriber.ConnectToBroker("peter", transmitterId, transmitterSecret);

            bool disconnectSuccess = await MqttManager.Subscriber.DisconnectFromBrokerAsync();

            //Device sound = MqttManager.Subscriber.SubscribeToDeviceData("a0dde2dc-bce6-4df3-8f6c-2b773ce6fb90", QualityOfService.AtLeastOnce);
            //sound.PublishedDataReceived += sound_PublishedDataReceived;
        }

        void sound_PublishedDataReceived(object sender, PublishedDataReceivedEventArgs args)
        {
            
        }
    }
}
