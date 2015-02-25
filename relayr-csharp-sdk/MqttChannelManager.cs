using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace relayr_csharp_sdk
{
    public static class MqttChannelManager
    {
        private static MqttClient _client;
        private static Dictionary<string, Device> _subscribedDevices;
        private static Dictionary<string, string> _deviceIdToTopic;     // for unsubscribing

        private static Dictionary<string, string> _arguments;

        public static bool IsConnected {
            get 
            {
                return _client.IsConnected;
            }
        }

        public static string ClientId;
      
        private static QualityOfService DefaultQualityOfService;

        private static void Initialize() {
            _client = new MqttClient("mqtt.relayr.io");
            _client.MqttMsgPublishReceived += _client_MqttMsgPublishReceived;

            _subscribedDevices = new Dictionary<string, Device>();
            _deviceIdToTopic = new Dictionary<string, string>();

            _arguments = new Dictionary<string, string>();
            _arguments.Add("transport", "mqtt");
            _arguments.Add("deviceId", "");

            // Assign a random value to the clientId if one hasn't been set yet
            Random r = new Random();
            if (ClientId == null) 
            { 
                ClientId = r.Next(0, Int16.MaxValue) + "";
            }

            if (DefaultQualityOfService == null)
            {
                DefaultQualityOfService = QualityOfService.AtLeastOnce;
            }
        }

        private static void ConnectToBroker(string userId, string password)
        {
            _client.Connect(ClientId, userId, password);
        }

        // Does the meat of the work in subscribing to a device (as well as initalization and connecting
        // if need be)
        public static async Task<Device> SubscribeToDeviceData(string deviceId)
        {
            // Initialize, if required
            if (_client == null)
            {
                Initialize();
            }

            // Get the connection information for this device
            _arguments["deviceId"] = deviceId;
            HttpResponseMessage response = await HttpManager.Manager.PerformHttpOperation(ApiCall.CreateChannel, null, _arguments);
            dynamic connectionInfo = await HttpManager.Manager.ConvertResponseContentToObject(response);

            // Use that connection information to attempt to connect to the mqtt server, if necessary
            // Return null if a connection couldn't be created
            if (!_client.IsConnected)
            {
                string userId = (string)connectionInfo["credentials"]["user"];
                string password = (string)connectionInfo["credentials"]["password"];

                ConnectToBroker(userId, password);
                if (!_client.IsConnected)
                {
                    return null;
                }
            }

            // Get the quality of service byte
            byte serviceLevel = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
            switch (DefaultQualityOfService)
            {
                case QualityOfService.AtMostOnce:
                    serviceLevel = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE;
                    break;
                case QualityOfService.ExactlyOnce:
                    serviceLevel = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
                    break;
                case QualityOfService.GrantedFailure:
                    serviceLevel = MqttMsgBase.QOS_LEVEL_GRANTED_FAILURE;
                    break;
            }

            // Subscribe to data from the device
            string topic = (string)connectionInfo["credentials"]["topic"];
            _client.Subscribe(new string[] { topic }, new byte[] { serviceLevel });

            Device device = new Device(deviceId, topic, DefaultQualityOfService);
            _subscribedDevices.Add(topic, device);
            _deviceIdToTopic.Add(deviceId, topic);

            return device;
        }

        // Usubscribe from data coming from the device specified by the deviceId
        public static bool UnsubscribeFromDeviceData(string deviceId)
        {
            if (_deviceIdToTopic.ContainsKey(deviceId))
            {
                // Attempt to unsubscribe from the device
                string topic = _deviceIdToTopic[deviceId];
                _client.Unsubscribe(new string[] { topic });

                // Delete the device from records
                _subscribedDevices.Remove(topic);
                _deviceIdToTopic.Remove(deviceId);
                return true;
            }

            return false;
        }

        // Raise the new data event in the appropriate device object
        private static void _client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(_subscribedDevices.ContainsKey(e.Topic)) 
            {
                Device device = _subscribedDevices[e.Topic];

                QualityOfService qosLevel = QualityOfService.AtLeastOnce;
                switch (e.QosLevel)
                {
                    case MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE:
                        qosLevel = QualityOfService.AtMostOnce;
                        break;
                    case MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE:
                        qosLevel = QualityOfService.ExactlyOnce;
                        break;
                    case MqttMsgBase.QOS_LEVEL_GRANTED_FAILURE:
                        qosLevel = QualityOfService.GrantedFailure;
                        break;
                }

                device.NewData(e.Message, e.Retain, e.DupFlag, qosLevel);
            }
        }
    }
}
