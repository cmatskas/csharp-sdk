using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using relayr_csharp_sdk;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace csharp_sdk_unit_tests
{
    [TestClass]
    public class HttpManagerTests
    {
        [ClassInitialize]
        public static void InitializeHttpManager(TestContext context)
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
        }

        #region PerformHttpOperation

        /*
         * PerformHttpOperation functionality:
         *      - Overall: should perform the Http Operation specified. Arguments should be placed in 
         *          URI, content should be converted to a Json string and passed with the operation.
         *          Returns the result of the operation (containing Json response, etc)
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
        public async Task PerformHttpOperation_EmptyArguments_ThrowException()
        {
            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, new string[2], null);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                // Pass the test
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_TooManyArguments_ThrowException()
        {
            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, new string[3] {"brr", "skrr", "hur"} , null);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                // Pass the test
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_NullArgumentsWhenArgumentsRequired_ThrowException()
        {
            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserCheckEmail, null, null);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                // Pass the test
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_NullArgumentsNoArgumentsRequired_ExecuteNormally()
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
            await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
        }

        [TestMethod]
        public async Task PerformHttpOperation_CorrectNumArguments_ExecuteNormally()
        {
            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserCheckEmail,
                    new string[] { "134-236-3213-1135" }, null);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_NullContent_ExecuteNormally()
        {
            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserUpdateDetails,
                    new string[] { "asdf-asdf-asdf-asdf"}, null);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_TooLittleContent_ExecuteNormally()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("name", "skkkrrrrrr");

            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserUpdateDetails,
                    new string[] { "asdf-asdf-asdf-asdf" }, content);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_TooMuchContent_ExecuteNormally()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("name", "skkkrrrrrr");
            content.Add("email", "skkkrrrrrr@relayr.io");
            content.Add("age", "born millenia before the dawn of man");

            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserUpdateDetails,
                    new string[] { "asdf-asdf-asdf-asdf" }, content);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_CorrectContent_ExecuteNormally()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("name", "skkkrrrrrr");
            content.Add("email", "skkkrrrrrr@relayr.io");

            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserUpdateDetails,
                    new string[] { "asdf-asdf-asdf-asdf" }, content);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PerformHttpOperation_IncorrectContent_ExecuteNormally()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("gender", "incomprehensible");
            content.Add("age", "born millenia before the dawn of man");

            try
            {
                HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserUpdateDetails,
                    new string[] { "asdf-asdf-asdf-asdf" }, content);
            }
            catch
            {
                Assert.Fail();
            }
        }

        //***********************************
            /*
             * TODO: Add tests to make sure that each operation had the correct:
             *      - URI
             *      - Argument Position
             *      - Operation Type
             */
        //***********************************

        #endregion

        /*
         * ConvertResponseContentToObject functionality
         *      - Overall: should convert the Json string contained in the Content of the passed 
         *          HttpResponseMessage to a dynamic object.
         *      - Should throw an InvalidOperation exception if the response type is anything but a 200
         *      - If Content is empty, return a null dynamic object
         *      - If there is an issue converting the Content, bubble up the exception from the Json converter
         *      - Throw an InvalidArgument exception if the parameter passed is null
         */

        [TestMethod]
        public async Task ConvertResponseContentToObject_Non200ResponseCode_ThrowException()
        {
            HttpManager.Instance.OauthToken = "asdfasdfasdfasdffaaaaaakkkkkkkkkkkeeee";
            HttpResponseMessage message = await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);

            try
            {
                await HttpManager.Instance.ConvertResponseContentToObject(message);
                Assert.Fail();
            }
            catch(InvalidOperationException e)
            {
                // Passed test
            }
        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_200ResponseCode_ExecuteNormally()
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
            HttpResponseMessage message = await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
            await HttpManager.Instance.ConvertResponseContentToObject(message);
        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_EmptyContent_ReturnNull()
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
            HttpResponseMessage message = await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
            message.Content = new StringContent("");
            
            dynamic result = await HttpManager.Instance.ConvertResponseContentToObject(message);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_NullContent_ReturnNull()
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
            HttpResponseMessage message = await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
            message.Content = null;

            dynamic result = await HttpManager.Instance.ConvertResponseContentToObject(message);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_InvalidContentJson_ThrowInvalidOperationException()
        {
            HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";
            HttpResponseMessage message = await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
            message.Content = new StringContent("{\"waka\" : \"brrrr\", \"wut\" : }");

            try
            {
                dynamic result = await HttpManager.Instance.ConvertResponseContentToObject(message);
                Assert.Fail();
            }
            catch (InvalidOperationException ex) 
            {
                // Test passed   
            }
        }

        [TestMethod]
        public async Task ConvertResponseContentToObject_NullPassedAsArgument_ThrowInvalidArgumentException()
        {
            try
            {
                await HttpManager.Instance.ConvertResponseContentToObject(null);
                Assert.Fail();
            }
            catch
            {
                // Passed test
            }
        }
    }
}
