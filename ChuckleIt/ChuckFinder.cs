using F23.StringSimilarity;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ChuckleIt;
class ChuckFinder
{
    internal void Run(IChucksKeeper storage, IChucksTrace[] searchArea) {
        foreach(var source in searchArea) {
            if(source == null) {
                Log.Error("Chuck is nowhere but we can't chase him there.");
            }
            UpdateFrom(source, storage);
        }
    }

    void UpdateFrom(IChucksTrace source, IChucksKeeper target) {
        var sourceId = string.Empty;
        try {
            sourceId = source.Identifier();
            var @checked = 0; //should asks target how many are there from this source
            var added = 0;
            var found = new string[0];
            do {
                found = source.GetMoreChucks();
                @checked += found.Length;
                added += CheckNextBatch(found, sourceId, target);
            } while(found != null && found.Length > 0);
            Log.Info($"Found {added} interesting chucks from {sourceId}");
        } catch(Exception error) {
            Log.Error($"Obscure source of chucks: {sourceId}. Unmesurable amount of available chucks.", error);
        }
    }

    int CheckNextBatch(string[] jokes, string fromSource, IChucksKeeper jail) {
        var validatedAsNew = 0;
        foreach(var joke in jokes) {
            validatedAsNew += CheckNext(joke, fromSource, jail) ? 1 : 0;
        }
        return validatedAsNew;
    }

    bool CheckNext(string joke, string fromSource, IChucksKeeper jail) {
        if(joke == null || joke.Length == 0
                || false == IsGoodChuck(joke)
                || jail.IsAlreadyThere(joke, IsBasicallyTheSame)) {
            return false; //TODO: optimize it, it's wasted bandwidth, some cache would be good to not look at the same things again and again, using checksums to compare and other stuff... lot of optimalization possible
        }
        jail.KeepHim(joke, fromSource);
        Log.Info($"Found new Chuck: {joke}");
        return true;
    }

    bool IsGoodChuck(string suspect) =>
        suspect != null
        && suspect.Length > "Chuck Borris".Length
        && suspect.Length <= 200;
        //&& dearAiLord.IsFunny(suspect); //that may be little to advanced for tonight :p

    bool IsBasicallyTheSame(string someJoke, string otherJoke) =>
        new NormalizedLevenshtein().Distance(someJoke, otherJoke) > 0.9;
        //FIXME: I have no idea about comparing texts, just pulled something out of my... little bit of research required
}
