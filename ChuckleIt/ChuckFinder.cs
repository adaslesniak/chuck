using F23.StringSimilarity;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ChuckleIt;
class ChuckFinder
{
    readonly int amountToCheck = 3;

    private ChuckFinder() { }

    internal ChuckFinder(int howManyToCheckPerRun) {
        amountToCheck = howManyToCheckPerRun;
    }

    internal void Run(IChucksKeeper storage, IChuckTrace[] searchArea) {
        foreach(var source in searchArea) {
            if(source == null) {
                Log.Error("Chuck is nowhere but we can't chase him there.");
            }
            UpdateFrom(source, storage);
        }
    }

    void UpdateFrom(IChuckTrace source, IChucksKeeper target) {
        var sourceId = string.Empty;
        try {
            sourceId = source.Identifier();
            var added = 0;
            for(int i = 0; i < amountToCheck; i++) {
                var found = source.GetSomeChuck();
                added += CheckNext(found, sourceId, target) ? 1 : 0;
            }
            Log.Info($"Found {added} interesting chucks from {sourceId}");
        } catch(Exception error) {
            Log.Error($"Obscure source of chucks: {sourceId}. Unmesurable amount of available chucks.", error);
        }
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
