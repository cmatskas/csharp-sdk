﻿/*
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

        public readonly Dictionary<string, Device> Devices;
        public QualityOfService DefaultQualityOfService;

        private string _clientId;
        public string ClientId
        {
            get
            {
                return _clientId;
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

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

        #region Connect and Disconnect

        // Connect to the MQTT broker using the provided credentials. Returns a bool representing
        // whether the connection was successful or not
        public bool ConnectToBroker(string clientId, string username, string password)
        {
            _clientId = clientId;
            _userName = username;

            _client.Connect(clientId, username, password);
            return _client.IsConnected;
        }

        // Disconnect from the MQTT broker. Returns a bool representing whether the operation was
        // successful or not
        public Task<bool> DisconnectFromBrokerAsync()
        {
            if (!_client.IsConnected)
            {
                
            }

            return Task.Run(() =>
            {
                try
                {
                    _client.Disconnect();
                    while (_client.IsConnected)
                    {
                        Task.Delay(100);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        #endregion

        #region Subscribe and Unsubscribe from Device

        // Subscribe to new data coming from the device specified by the deviceId, with the
        // default MQTT quality of service
        public Device SubscribeToDeviceData(string deviceId)
        {
            return SubscribeToDeviceData(deviceId, DefaultQualityOfService);
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

        #endregion

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
