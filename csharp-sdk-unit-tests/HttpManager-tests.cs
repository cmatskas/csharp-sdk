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
            await ThrowsAsync<ArgumentException>(() => HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, new string[2], null)); 


            //Assert.ThrowsException<ArgumentException>(async() => await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, new string[2], null));

            //try
            //{
            //    await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, new string[2], null);
            //    Assert.Fail();
            //}
            //catch(ArgumentException e) 
            //{
            //    // Pass the test
            //}
        }

        [TestMethod]
        public async Task PerformHttpOperation_TooManyArguments_ThrowException()
        {
            try
            {
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
            //try
            //{
                await HttpManager.Instance.PerformHttpOperation(HttpManager.ApiCall.UserGetInfo, null, null);
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail();
            //}
        }

        [TestMethod]
        public async Task PerformHttpOperation_CorrectNumArguments_ExecuteNormally()
        {
            try
            {
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



        // Found on stack overflow
        public static async Task ThrowsAsync<TException>(Func<Task> action, bool allowDerivedTypes = true)
        {
            try
            {
                await action();
                Assert.Fail("Delegate did not throw expected exception " + typeof(TException).Name + ".");
            }
            catch (Exception ex)
            {
                if (allowDerivedTypes && !(ex is TException))
                    Assert.Fail("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " or a derived type was expected.");
                if (!allowDerivedTypes && ex.GetType() != typeof(TException))
                    Assert.Fail("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " was expected.");
            }
        }
    }
}
