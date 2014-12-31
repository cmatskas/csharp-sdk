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
                PublishedDataReceivedEventArgs args = new PublishedDataReceivedEventArgs(rawData, dupFlag, retain, qosLevel);
                PublishedDataReceived(this, args);
            }
        }
    }
}
