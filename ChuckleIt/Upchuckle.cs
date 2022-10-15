using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace ChuckleIt;
public class Upchuckle
{
    [FunctionName("FindSomeChucks")]
    public void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
    {
        //TODO FIXME: configuration required
        Log.Setup(log);
        var start = DateTime.UtcNow;
        Log.Info($"Starting search of chucks: {start}");
        new ChuckFinder().Run(
            new SqlLiteChucks(),
            new[] { new RapidApiChucks()} );
        Log.Info($"Done with chuckling after {(DateTime.UtcNow - start)}.");
    }
}

public interface IChucksTrace {
    int HowManyChucksAreThere();
    string[] GetChucks(int fromIndex = 0, int howMany = 1);
    string Identifier();
}

public delegate bool DuplicatesReferee(string someJoke, string otherJoke);

public interface IChucksKeeper {
    bool IsAlreadyThere(string suspect, DuplicatesReferee isDuplicate);
    void KeepHim(string suspect, string whereWasHeFound);
}
