using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using relayr_csharp_sdk;
using System.Threading.Tasks;

namespace csharp_sdk_unit_tests
{
    [TestClass]
    public class UnitTest1
    {
        /*
         * PerformHttpOperation functionality:
         *      - should throw an InvalidArgument exception if passed the incorrect number of arguments
         *      - content will always be constructed normally and sent. throw no exceptions here.
         *      - should return an HttpResponseMessage so that the result of the operation can be inspected
         *          - this value can be passed to a conversion method to create a dynamic object to access values
         * 
         */


        //[TestMethod]
        //public async Task ()
        //{

        //}

    }
}
