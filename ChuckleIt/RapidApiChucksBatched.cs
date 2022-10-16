using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ChuckleIt;

//this makes some sense, but doesn't allow to control amount of pulled data, so doesn't meed requirements
class RapidApiChucksBatched : IChucksTrace
{
    string[] IChucksTrace.MoreChucks() {
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

    string IChucksTrace.Identifier() => nameof(RapidApiChucksBatched);

    readonly RapidApiClient source = new();
    string[] categories;
    ushort actualCategory;

    bool EnsureCategories() {
        if(categories is not null
            && categories.Length > 0) {
            return true;
        }
        try {
            var response = source.GetArray("jokes/categories");
            categories = response.Select(dzajson => dzajson.Value<string>()).ToArray();
            return categories.Length > 0;
        } catch(Exception error) {
            Log.Error("Failed to get categories", error);
            return false;
        }
    }

    string[] GetJokesAbout(string category) {
        try {
            var response = source.GetObject($"jokes/search?query={category}");
            var jokesObjects = response.Value<JArray>("result");
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
}
