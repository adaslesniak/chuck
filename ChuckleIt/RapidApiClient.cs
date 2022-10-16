using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    internal class RapidApiClient
    {
        readonly HttpClient http;

        internal RapidApiClient() {
            http = new();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Add("accept", "application/json");
            http.DefaultRequestHeaders.Add("X-RapidAPI-Key", "723a9f2409msh881f2edf57a11ffp10120fjsn39958d9c7a2a");
            http.DefaultRequestHeaders.Add("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");
        }

        internal JObject GetObject(string endpoint) {
            var responseString = GetRawString(endpoint);
            return JsonConvert.DeserializeObject<JObject>(responseString);
        }

        internal JArray GetArray(string endpoint) {
            var responseString = GetRawString(endpoint);
            return JArray.Parse(responseString);
        }

        internal string GetRawString(string endpoint) {
            try {
                var fullUrl = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/" + endpoint;
                var message = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                var response = http.Send(message);
                return response.Content.ReadAsStringAsync().Result;
                //var responseString = response.Content.ReadAsStringAsync().Result;
                //return JsonConvert.DeserializeObject<JObject>(responseString);
            } catch(Exception error) {
                Log.Error("failed web request to RapidApiChucks", error);
                return string.Empty;// JObject.Parse(@"{}");
            }
        }
    }
}
