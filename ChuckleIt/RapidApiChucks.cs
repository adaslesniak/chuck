using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace ChuckleIt;
class RapidApiChucks : IChucksTrace
{
    //TODO FIXME: implementation required
    string[] IChucksTrace.GetMoreChucks() {
        if(actualCategory++ > 1) {
            return new string[0]; 
        }
        if(false == EnsureCategories()
            || actualCategory > categories.Length) {
            return new string[0];
        }
        var nextCategory = categories[actualCategory++];
        return GetJokesAbout(nextCategory);
    }

    string IChucksTrace.Identifier() => nameof(RapidApiChucks);

    readonly HttpClient http;
    string[] categories;
    ushort actualCategory = 0;

    bool EnsureCategories() {
        if(categories is not null
            && categories.Length > 0) {
            return true;
        }
        try {
            var responseData = Get("jokes/categories");
            categories = JsonConvert.DeserializeObject<string[]>(responseData);
            return categories.Length > 0;
        } catch(Exception error) {
            Log.Error("Failed to get categories", error);
            return false;
        }
    }

    string[] GetJokesAbout(string category) {
        try {
            var responseString = Get($"jokes/search?query={category}");
            var wholeResponse = JsonConvert.DeserializeObject<JObject>(responseString);
            var jokesObjects = wholeResponse.Value<JArray>("result");
            var jokes = new List<string>();
            foreach(var model in jokesObjects) {
                jokes.Add(model.Value<string>("value"));
            }
            return jokes.ToArray();

        } catch(Exception error) {
            Log.Error($"failed to grab jokes about {category}", error);
            return new string[0];
        }
    }

    public RapidApiChucks() {
        http = new();
        http.DefaultRequestHeaders.Accept.Clear();
        http.DefaultRequestHeaders.Add("accept", "application/json");
        http.DefaultRequestHeaders.Add("X-RapidAPI-Key", "723a9f2409msh881f2edf57a11ffp10120fjsn39958d9c7a2a");
        http.DefaultRequestHeaders.Add("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");
    }

    string Get(string endpoint) {
        try {
            var fullUrl = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/" + endpoint;
            var message = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            var response = http.Send(message);
            return response.Content.ReadAsStringAsync().Result;
        } catch(Exception error) {
            Log.Error("failed web request to RapidApiChucks", error);
            return String.Empty;
        }
    }

}
