using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace relayr_csharp_sdk
{
    public class Device
    {
        private string _deviceId;
        private string _topic;
        private QualityOfService _qosLevel;
        
        public string DeviceId
        {
            get
            {
                return _deviceId;
            }
        }

        public string Topic
        {
            get
            {
                return _topic;
            }
        }

        public QualityOfService QosLevel
        {
            get
            {
                return _qosLevel;
            }
        }

        public event PublishedDataReceivedEventHandler PublishedDataReceived;

        public Device(string deviceId, string topic, QualityOfService qosLevel)
        {
            _deviceId = deviceId;
            _topic = topic;
            _qosLevel = qosLevel;
        }

        public virtual void NewData(byte[] rawData, bool retain, bool dupFlag, QualityOfService qosLevel)
        {
            if (PublishedDataReceived != null)
            {
                string json = System.Text.Encoding.UTF8.GetString(rawData, 0, rawData.Length);

                dynamic dataObject;

                // If the Json data is an array, parse it as one
                if (json[0] == '[')
                {
                    JArray responseElements = JArray.Parse(json);
                    dataObject = responseElements.ToArray<dynamic>();
                }
                // Otherise, parse it as a single object
                else
                {
                    dataObject = JObject.Parse(json);
                }

                PublishedDataReceivedEventArgs args = new PublishedDataReceivedEventArgs(dataObject, dupFlag, retain, qosLevel);
                PublishedDataReceived(this, args);
            }
        }
    }
}
