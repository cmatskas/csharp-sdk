using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using relayr_csharp_sdk;
using System.Threading.Tasks;

namespace csharp_sdk_unit_tests
{
    [TestClass]
    public class HttpManagerTests
    {
        /*
         * PerformHttpOperation functionality:
         *      - Overall: should perform the Http Operation specified. Arguments should be placed in 
         *          URI, content should be converted to a Json string and passed with the operation.
         *          Returns the result of the operation (containing Json response, etc)
         *      - should throw an InvalidArgument exception if the passed operation is null
         *      - should throw an InvalidArgument exception if passed the incorrect number of arguments
         *      - content will always be constructed normally and sent. throw no exceptions here.
         *      - should return an HttpResponseMessage so that the result of the operation can be inspected
         *          - this value can be passed to a conversion method to create a dynamic object to access values
         *          - always return a response message no matter what it is
         *      - Operations should have correct URIs
         *          - Argument Tokens should be in the correct places
         *          - Type of the operation should be correct
         */

        [TestMethod]
        public async Task PerformHttpOperation_NullOperation_ThrowException()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_ZeroArguments_ThrowException()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_TooManyArguments_ThrowException()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_NullArguments_ThrowException()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_CorrectNumArguments_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_NullContent_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_TooLittleContent_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_TooMuchContent_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_CorrectContent_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task PerformHttpOperation_IncorrectContent_ExecuteNormally()
        {

        }

        //***********************************
            /*
             * TODO: Add tests to make sure that each operation had the correct:
             *      - URI
             *      - Argument Position
             *      - Operation Type
             */
        //***********************************

        /*
         * ConvertResponseContentToObject functionality
         *      - Overall: should convert the Json string contained in the Content of the passed 
         *          HttpResponseMessage to a dynamic object.
         *      - Should throw an InvalidOperation exception if the response type is anything but a 200
         *      - If Content is empty, return a null dynamic object
         *      - If there is an issue converting the Content, return a null dynamic object
         *      - Throw an InvalidArgument exception if the parameter passed is null
         */

        [TestMethod]
        public async Task ConvertResponseContentToObject_Non200ResponseCode_ThrowException()
        {

        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_200ResponseCode_ExecuteNormally()
        {

        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_EmptyContent_ReturnNull()
        {

        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_InvalidContentJson_ReturnNull()
        {

        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_NullPassedAsArgument_ThrowInvalidArgumentException()
        {

        }
    }
}
