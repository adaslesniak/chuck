using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Downchuckle;
public class Downchuckle
{
    [FunctionName("Downchuckle")]
    public void Run([TimerTrigger("%triggerTime%")]TimerInfo myTimer, ILogger log)
    {
        try {
            var amountToPull = Configurable("howManyToPull", 3);
            Log.Setup(log);
            LookForJokes(amountToPull);
        } catch(Exception error) {
            Log.Error("all is kaput", error); 
        }

        static void LookForJokes(int howMany) {
            var start = DateTime.UtcNow;
            Log.Info($"Starting search of chucks: {start}");
            new ChuckFinder(howMany).Run(
                new SqlLiteChucks(),
                new[] { new RapidApiChucksRandom() });
            Log.Info($"Done with chuckling after {(DateTime.UtcNow - start)}.");
        }

        static int Configurable(string withName, int defaulValue) {
            var found = Environment.GetEnvironmentVariable(withName, EnvironmentVariableTarget.Process);
            if(int.TryParse(found, out var reallyFound)) {
                return reallyFound;
            }
            return defaulValue;
        }
    }
}

public interface IChuckTrace {
    string GetSomeChuck();
    string Identifier();
}

public delegate bool DuplicatesReferee(string someJoke, string otherJoke);

public interface IChucksKeeper {
    bool IsAlreadyThere(string suspect, DuplicatesReferee isDuplicate);
    void KeepHim(string suspect, string whereWasHeFound);
}
