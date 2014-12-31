using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace relayr_csharp_sdk
{
    public class MqttManager
    {

        // create a singleton MqttClient
        // provide this client with the users credentials
        // have the client subscribe to whichever topics (devices) that the user wants
        //      the manager will need to make an API call to connect the device to this application 
        //      ^^ DON'T THINK THAT I ACTUALLY NEED TO DO THIS. THIS IS PROBABLY JUST FOR PUBNUB
        // the client will then create an object (decide on the structure and type) for this topic (device)
        // the client maintains a list of these devices 
        // when the client receives a value, it calls the update event in the specific function

        private MqttClient _client;

        private MqttManager _subscriber;
        public MqttManager Subscriber
        {
            get
            {
                if (_subscriber == null)
                {
                    _subscriber = new MqttManager();
                }

                return _subscriber;
            }
        }

        private MqttManager()
        {
            _client = new MqttClient("mqtt.relayr.io");
            _client.MqttMsgPublishReceived += _client_MqttMsgPublishReceived;
        }

        public void SubscribeToDeviceData()
        {

        }

        public void UnsubscribeFromDeviceData()
        {

        }

        // Called whenever a new item of data is published by any topic the manager is subscribed to
        private void _client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            
        }


        //private MqttManager()
        //{
        //    MqttClient client = new MqttClient("mqtt.relayr.io");
        //    client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        //    //client.Connect("relayr", "relayr", "");

        //    //client.Subscribe(new string[] { "/v1/e2fd5e51-5561-4e0b-a680-e785e69ed828/data" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        //}

    }
}
