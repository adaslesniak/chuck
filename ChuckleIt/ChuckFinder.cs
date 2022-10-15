using F23.StringSimilarity;
using System;

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
            int allThere = source.HowManyChucksAreThere();
            Log.Info($"Checking {allThere} chucks from {source.Identifier()}");
            int newOnes = 0;
            for(int i = 0; i < allThere; i++) {
                newOnes += UpdateNext(source, i, target) ? 1 : 0;
            }
            Log.Info($"Got {newOnes} new chucks from {sourceId}");
        } catch(Exception error) {
            Log.Error($"Obscure source of chucks: {sourceId}. Unmesurable amount of available chucks.", error);
        }
    }

    bool UpdateNext(IChucksTrace fromSource, int atIndex, IChucksKeeper jail) {
        try {
            var next = fromSource.GetChucks(atIndex, 1);
            if(next == null || next.Length == 0
                || false == IsGoodChuck(next[0])
                || jail.IsAlreadyThere(next[0], IsBasicallyTheSame)) {
                return false; //TODO: optimize it, it's wasted bandwidth, some cache would be good to not look at the same things again and again, using checksums to compare and other stuff... lot of optimalization possible
            }
            jail.KeepHim(next[0], fromSource.Identifier());
            Log.Info($"Found new Chuck: {next[0]}");
            return true;
        } catch(Exception error) {
            Log.Error($"Escape was too strong in this chuck", error);
            return false;
        }
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
