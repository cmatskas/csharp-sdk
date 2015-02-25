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
using uPLibrary.Networking.M2Mqtt;

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

            testClient();
        }

        public async void testClient()
        {
            Relayr.OauthToken = "TEAM TOKEN HERE";
            List<dynamic> Transmitters = await Relayr.GetTransmittersAsync();

            List<dynamic> devices = null;
            Transmitter t = null;
            foreach (dynamic transmitter in Transmitters)
            {
                string id = (string)transmitter["id"];
                if (id.Equals("IN THE NEIGHBORHOOD ID HERE"))
                {
                    t = new Transmitter(id);
                    break;
                }
            }

            devices = await t.GetDevicesAsync();

            Device d = await MqttChannelManager.SubscribeToDeviceData((string)devices[0]["id"]);
            d.PublishedDataReceived += d_PublishedDataReceived;

        }

        void d_PublishedDataReceived(object sender, PublishedDataReceivedEventArgs args)
        {
            //throw new NotImplementedException();
        }
    }
}
