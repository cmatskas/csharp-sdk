﻿using Newtonsoft.Json.Linq;
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
        private HttpRequestMessage httpRequest;

        // Initialize the HttpClient with the base URI of the relayr.api
        private HttpManager() 
        {
            httpClient = new HttpClient();
            Uri baseUri = new Uri("https://api.relayr.io/");
            httpClient.BaseAddress = baseUri;

            httpRequest = new HttpRequestMessage();
        }

        public enum httpOperation
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
            [Uri("users/!/devices?")]
            [OperationType("GET")]
            DevicesUnderASpecificUser,
            [Uri("users/!/devices?meaning=!")]
            [OperationType("GET")]
            DevicesUnderASpecificUserWithMeaning,
            [Uri("devices/!")]
            [OperationType("PATCH")]
            DevicesUpdateAttributes
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

        public async Task<dynamic> PerformHttpOperation(httpOperation operation, string[] arguments, Dictionary<string, string> content) {
            
            // Get the URI extension and opration type from the attributes of the operation
            Type type = operation.GetType();
            FieldInfo field = type.GetRuntimeField(operation.ToString());
            string uriExtension = ((UriAttribute) field.GetCustomAttribute(typeof(UriAttribute), false)).Value;
            string operationType = ((OperationType) field.GetCustomAttribute(typeof(OperationType), false)).Value;

            // Add the arguments to the URI, if required
            if (arguments != null) 
            {
                uriExtension = processArguments(uriExtension, arguments);
            }

            // Create the content json, if required
            StringContent contentJson = null;
            if (content != null)
            {
                contentJson = new StringContent(createContentJson(content));
            }

            // Set up the http request
            httpRequest.Method = new HttpMethod(operationType);
            httpRequest.RequestUri = new Uri(uriExtension, UriKind.Relative);
            httpRequest.Content = contentJson;

            // Send the http request
            HttpResponseMessage rawJson = await httpClient.SendAsync(httpRequest);
            string returnedJson = await rawJson.Content.ReadAsStringAsync();

            // Check if the returned json is an array of objects. If so, parse as one.
            if (returnedJson[0] == '[')
            {
                JArray responseElements = JArray.Parse(returnedJson);
                return responseElements.ToArray<dynamic>();
            }

            // Otherwise, parse it as a single json object
            return JObject.Parse(returnedJson);
        }

        #region Helpers

        // Take a uri and arguments, insert the arguments into the URI, return the URI
        private string processArguments(string uri, string[] args)
        {
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
                try
                {
                    Boolean.Parse(pair.Value);
                    jsonString.Append(pair.Value);
                }
                catch
                {
                    try
                    {
                        Double.Parse(pair.Value);
                        jsonString.Append(pair.Value);
                    }
                    catch
                    {
                        jsonString.Append("\"").Append(pair.Value).Append("\"");
                    }
                }
                // #####################################################################
            }
            jsonString.Append(" }");

            return jsonString.ToString();
        }

        #endregion
    }
}
