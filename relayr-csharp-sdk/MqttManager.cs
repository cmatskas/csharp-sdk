/*
 * Copyright (c) 2014 iThings4U Gmbh
 * 
 * Author:
 *      Peter Dwersteg      
 */

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
        #region fields and properties

        private MqttClient _client;
        private Dictionary<string, Device> _devices;

        private static MqttManager _subscriber;
        public static MqttManager Subscriber
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

        #endregion

        private MqttManager()
        {
            _client = new MqttClient("mqtt.relayr.io");
            _client.MqttMsgPublishReceived += _client_MqttMsgPublishReceived;

            _devices = new Dictionary<string, Device>();
        }

        // Connect to the MQTT broker using the provided credentials
        public void ConnectToBroker(string clientId, string username, string password)
        {
            _client.Connect(clientId, username, password);
        }

        // Subscribe to new data coming from the device specified by the deviceId, with the
        // specified MQTT quality of service
        public Device SubscribeToDeviceData(string deviceId, QualityOfService qualityOfService)
        {
            byte serviceLevel = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
            switch (qualityOfService)
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

            string topic = "/v1/" + deviceId + "/data";

            _client.Subscribe(new string[] { topic }, new byte[] { serviceLevel });

            // Create a device and add it to the list
            Device device = new Device(deviceId, topic, qualityOfService);
            _devices.Add(topic, device);
            
            // Return a reference to the device
            return device;
        }

        // Unsubscribe from the device specified by the deviceId
        public void UnsubscribeFromDeviceData(string deviceId)
        {
            string topic = "/v1/" + deviceId + "/data";
            _client.Unsubscribe(new string[] { topic });

            // Remove the device from the set of devices
            if (_devices.ContainsKey(topic))
            {
                _devices.Remove(topic);
            }
        }

        // Called whenever a new item of data is published by any topic the manager is subscribed to
        private void _client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (_devices.ContainsKey(e.Topic)) 
            { 
                Device device = _devices[e.Topic];

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
