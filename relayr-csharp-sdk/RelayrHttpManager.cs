using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;
using System.Collections.Generic;

namespace relayr_csharp_sdk
{
    class User
    {
        public string Name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

     public class RelayrHttpManager
    {
        private static string _token;
        private const string relayrAPI = "https://api.relayr.io";
        private const string URL = relayrAPI;
        private static string urlParameters;
        private static HttpClient client;
        private static HttpResponseMessage response;

       public RelayrHttpManager(string token)
        {
            _token = token;
            client = new HttpClient();
            response = new HttpResponseMessage();
            urlParameters = "";
            client.BaseAddress = new Uri(URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task GetUserInfo()
        {
            urlParameters = "oauth2/user-info";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer "+_token);
            client.DefaultRequestHeaders.Add("User-Agent", "io.relayr.postman/0.1.0");
             response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                //User user = await response.Content.ReadAsAsync<User>();
                //Debug.WriteLine("{0}\t${1}\t{2}", user.Name, user.id, user.email);
                Debug.WriteLine(response);
                Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
                // Parse the response body. Blocking!
                //var user = response.Content.ReadAsAsync<User>().Result;
                //Debug.WriteLine(user.Name);
            }
            else
            {
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            } 
        }

        public static async Task PutRequest()
        {
                    // HTTP PUT
                    var user = new User();
                    user.id = "80";   // Update id
                    response = await client.PutAsJsonAsync(URL, user);
        }

        public static async Task PostRequest()
        {
            // HTTP POST
                var user = new User() { Name = "user", id = "100", email = "Widget" };
                response = await client.PostAsJsonAsync(URL, user);
                if (response.IsSuccessStatusCode)
                {
                    Uri userUrl = response.Headers.Location;
                }
        }

        static async Task DeleteRequest()
        {
                    response = await client.DeleteAsync(URL);
        }
    }
}