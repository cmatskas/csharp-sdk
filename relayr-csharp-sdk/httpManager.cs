using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace relayr_csharp_sdk
{

    public class HttpManager
    {
        static private HttpManager manager;
        
        private string oauthToken;
        private HttpClient httpClient;

        private bool conversionDummyBool;
        private double conversionDummyDouble;

        // Initialize the HttpClient with the base URI of the relayr.api
        private HttpManager() 
        {
            httpClient = new HttpClient();
            Uri baseUri = new Uri("https://api.relayr.io/");
            httpClient.BaseAddress = baseUri;
        }

        public enum ApiCall
        {
            [Uri("users/validate?email=!")]
            [OperationType("GET")]
            UserCheckEmail,
            [Uri("oauth2/user-info")]
            [OperationType("GET")]
            UserGetInfo,
            [Uri("users/!")]
            [OperationType("PATCH")]
            UserUpdateDetails,
            [Uri("publishers")]
            [OperationType("GET")]
            PublishersReturnList,
            [Uri("publishers")]
            [OperationType("POST")]
            PublishersRegister,
            [Uri("publishers/!")]
            [OperationType("PATCH")]
            PublisherUpdateAttributes,
            [Uri("publishers/!")]
            [OperationType("DELETE")]
            PublisherDelete,
            [Uri("users/!/publishers")]
            [OperationType("GET")]
            PublishersListByUser,
            [Uri("apps")]
            [OperationType("GET")]
            ApplicationsListAll,
            [Uri("apps")]
            [OperationType("POST")]
            ApplicationsAddNew,
            [Uri("oauth2/app-info")]
            [OperationType("GET")]
            ApplicationsGetInfo,
            [Uri("oauth2/appdev-token/!")]
            [OperationType("GET")]
            ApplicationsReturnToken,
            [Uri("oauth2/appdev-token/!")]
            [OperationType("POST")]
            ApplicationsReplaceToken,
            [Uri("oauth2/appdev-token/!")]
            [OperationType("DELETE")]
            ApplicationDeleteToken,
            [Uri("apps/!")]
            [OperationType("GET")]
            ApplicationsRetrievePublisherApp,
            [Uri("apps/!")]
            [OperationType("PATCH")]
            ApplicationsUpdateAttributes,
            [Uri("apps/!")]
            [OperationType("DELETE")]
            ApplicationsDeleteApp,
            [Uri("users/!/apps/!")]
            [OperationType("POST")]
            ApplicationInstallAppUnderUser,
            [Uri("users/!/apps/!")]
            [OperationType("DELETE")]
            ApplicationUninstallAppUnderUser,
            [Uri("users/!/apps/!")]
            [OperationType("GET")]
            ApplicationInstalledAppInfo,
            [Uri("users/!/apps")]
            [OperationType("GET")]
            ApplicationListUserApps,
            [Uri("devices/!/apps")]
            [OperationType("GET")]
            ApplicationsListConnectedToDevice,
            [Uri("publishers/!/apps")]
            [OperationType("GET")]
            ApplicationsListByPublisher,
            [Uri("publishers/!/apps/extended")]
            [OperationType("GET")]
            ApplicationsListByPublisherExtended,
            [Uri("apps/!/devices/!")]
            [OperationType("POST")]
            ApplicationsConnectAppToDevice,
            [Uri("apps/!/devices/!")]
            [OperationType("DELETE")]
            ApplicationsDisconnectAppFromDevice,
            [Uri("devices")]
            [OperationType("POST")]
            DevicesRegisterNew,
            [Uri("users/!/devices?")]
            [OperationType("GET")]
            DevicesListUnderSpecificUser,
            [Uri("users/!/devices?meaning=!")]
            [OperationType("GET")]
            DevicesListUnderSpecificUserWithMeaning,
            [Uri("devices/!")]
            [OperationType("GET")]
            DeviceInformation,
            [Uri("devices/!")]
            [OperationType("PATCH")]
            DevicesUpdateAttributes,
            [Uri("devices/!")]
            [OperationType("DELETE")]
            DevicesDelete,
            [Uri("devices/!/apps/!")]
            [OperationType("POST")]
            DevicesConnectDeviceToApp,
            [Uri("devices/!/apps/!")]
            [OperationType("DELETE")]
            DevicesDisconnectDeviceFromApp,
            [Uri("users/!/devices/public?")]
            [OperationType("GET")]
            DevicesListPublic,
            [Uri("users/!/devices/public?meaning=!")]
            [OperationType("GET")]
            DevicesListPublicWithMeaning,
            [Uri("devices/!/subscription")]
            [OperationType("POST")]
            DevicesSubscribeUserToPublic,
            [Uri("users/!/devices/!/bookmarks")]
            [OperationType("POST")]
            DevicesBookmarkPublic,
            [Uri("users/!/devices/!/bookmarks")]
            [OperationType("DELETE")]
            DevicesDeleteBookmarkPublic,
            [Uri("users/!/devices/bookmarks")]
            [OperationType("GET")]
            DevicesListBookmarkedByUser,
            [Uri("device-models")]
            [OperationType("GET")]
            DevicesListOfModels,
            [Uri("device-models/!")]
            [OperationType("GET")]
            DevicesGetInfoAboutModel,
            [Uri("device-models/!/firmware")]
            [OperationType("GET")]
            DevicesGetFirmwareListForModel,
            [Uri("device-models/!/firmware/!")]
            [OperationType("GET")]
            DevicesGetFirmwareVersionRecord,
            [Uri("device-models/meanings")]
            [OperationType("GET")]
            DevicesListPossibleReadingTypes,
            [Uri("wunderbars/!")]
            [OperationType("DELETE")]
            WunderBarDelete,
            [Uri("devices/!/firmware")]
            [OperationType("GET")]
            DeviceConfigurationInformation,
            [Uri("devices/!/configuration")]
            [OperationType("POST")]
            DeviceModifyConfiguration,
            [Uri("POST")]
            [OperationType("devices/!/cmd/!")]
            DeviceIssueCommand,
            [Uri("transmitters")]
            [OperationType("Post")]
            TransmittersRegister,
            [Uri("users/!/transmitters")]
            [OperationType("GET")]
            TransmittersListByUser,
            [Uri("GET")]
            [OperationType("transmitters/!")]
            TransmittersGetSpecificInformation,
            [Uri("transmitters/!")]
            [OperationType("PATCH")]
            TransmittersUpdate,
            [Uri("transmitters/!")]
            [OperationType("DELETE")]
            TransmittersDelete,
            [Uri("transmitter/!/device/!")]
            [OperationType("POST")]
            TransmittersConnectToDevice,
            [Uri("transmitter/!/device/!")]
            [OperationType("DELETE")]
            TransmittersDisconnectFromDevice,
            [Uri("transmitters/!/devices")]
            [OperationType("GET")]
            TrasmittersListConnectedDevices
        }

        #region Fields

        // Create / return reference to singleton manager object
        public static HttpManager Instance
        {
            get
            {
                if (manager == null)
                {
                    manager = new HttpManager();
                }

                return manager;
            }
        }

        // Get or set the Oauth token. On set token will automatically be added to request headers
        public string OauthToken
        {
            get
            {
                return oauthToken;
            }
            set
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
                oauthToken = value;
            }
        }

        #endregion

        // Perform a relayr API operation. Takes the operation, URI arguments, and a dictionary
        // of key -> value pairs (which will be converted to a JSON string) for operation content
        public async Task<HttpResponseMessage> PerformHttpOperation(ApiCall operation, string[] arguments, Dictionary<string, string> content) {
            
            // Get the URI extension and opration type from the attributes of the operation
            Type type = operation.GetType();
            FieldInfo field = type.GetRuntimeField(operation.ToString());
            string uriExtension = ((UriAttribute) field.GetCustomAttribute(typeof(UriAttribute), false)).Value;
            string operationType = ((OperationType) field.GetCustomAttribute(typeof(OperationType), false)).Value;

            // Add the arguments to the URI, if required
            uriExtension = processArguments(uriExtension, arguments);

            // Create the content json, if required
            StringContent contentJson = null;
            if (content != null)
            {
                contentJson = new StringContent(createContentJson(content));
            }

            // Set up the http request
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = new HttpMethod(operationType);
            httpRequest.RequestUri = new Uri(uriExtension, UriKind.Relative);
            httpRequest.Content = contentJson;

            // Send the http request, return the response message
            return await httpClient.SendAsync(httpRequest);
        }
        
        // Convert the JSON content of the response message into an object whose values can be easily accessed
        public async Task<dynamic> ConvertResponseContentToObject(HttpResponseMessage message)
        {
            // Throw an InvalidOperation exception if the message isn't 200 OK
            if (!message.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Response code is: " 
                    + message.StatusCode.ToString()
                    + ". Cannot create a object from JSON response if the operation was not successful (200 OK).");
            }

            if (message.Content == null)
            {
                return null;
            }

            string returnedJson = await message.Content.ReadAsStringAsync();

            if (returnedJson.Equals(""))
            {
                return null;
            }

            try 
            { 
                // Check if the returned json is an array of objects. If so, parse as one.
                if (returnedJson[0] == '[')
                {
                    JArray responseElements = JArray.Parse(returnedJson);
                    return responseElements.ToArray<dynamic>();
                }

                // Otherwise, parse it as a single json object
                return JObject.Parse(returnedJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error when parsing JSON content", e);
            }
        }

        #region Helpers

        // Take a uri and arguments, insert the arguments into the URI, return the URI
        private string processArguments(string uri, string[] args)
        {
            if (args == null)
            {
                if (uri.Contains('!'))
                {
                    throw new ArgumentException("Expected arguments, received none");
                }
                else
                {
                    return uri;
                }
            }

            string[] uriPieces = uri.Split('!');

            // Throw an argumentexception if we didn't get the correct number of uri arguments
            if (uriPieces.Length - 1 != args.Length)
            {
                throw new ArgumentException("Expected " + (uriPieces.Length - 1) + " number of arguments. Got " + args.Length + ".");
            }

            StringBuilder uriWithArgs = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                uriWithArgs.Append(uriPieces[i]);
                uriWithArgs.Append(args[i]);
            }
            uriWithArgs.Append(uriPieces[uriPieces.Length - 1]);
            return uriWithArgs.ToString();
        }

        // Turn the dictionary of key -> value into json string of name : value
        // quotation marked bools and numbers will be inserted into json without quotation marks
        private string createContentJson(Dictionary<string, string> content)
        {
            StringBuilder jsonString = new StringBuilder();
            bool first = true;
            jsonString.Append("{ ");
            foreach (KeyValuePair<string, string> pair in content)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    jsonString.Append(", ");
                }

                jsonString.Append("\"").Append(pair.Key).Append("\" : ");

                // ############### GIANT HACK NOT FOR CHILDREN'S EYES ##################

                if (Boolean.TryParse(pair.Value, out conversionDummyBool) || 
                    Double.TryParse(pair.Value, out conversionDummyDouble))
                {
                    jsonString.Append(pair.Value);
                }
                else
                {
                    jsonString.Append("\"").Append(pair.Value).Append("\"");
                }

                // #####################################################################
            }
            jsonString.Append(" }");

            return jsonString.ToString();
        }

        #endregion
    }
}
