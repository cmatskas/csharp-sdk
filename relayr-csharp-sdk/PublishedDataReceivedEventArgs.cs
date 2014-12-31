using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace relayr_csharp_sdk
{
    public class PublishedDataReceivedEventArgs
    {
        public byte[] RawData;
        public bool DupFlag;
        public bool Retain;
        public QualityOfService QosLevel;

        public PublishedDataReceivedEventArgs(byte[] rawData, bool dupFlag, bool retain, QualityOfService qosLevel)
        {
            RawData = rawData;
            DupFlag = dupFlag;
            Retain = retain;
            QosLevel = qosLevel;
        }
    }
}
