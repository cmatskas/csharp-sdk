using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using relayr_csharp_sdk;

namespace csharp_sdk_unit_tests
{
    [TestClass]
    class MqttManager_tests
    {
        #region ConnectToBroker

        // ConnectToBroker should return true if the connection was successful, and false if it wasn't
        // The values of the arguments shouldn't matter, but they shouldn't be null.
        //      If any value is null, throw an InvalidArgumentException

        [TestMethod]
        public void ConnectToBroker_ValidCredentials_Success()
        {
            //if(MqttManager.Subscriber.ConnectToBroker("relayr"))
        }

        #endregion

    }
}
