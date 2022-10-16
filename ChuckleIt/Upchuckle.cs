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
        new ChuckFinder(7).Run(
            new SqlLiteChucks(),
            new[] { new RapidApiChucksBatched()} );
        Log.Info($"Done with chuckling after {(DateTime.UtcNow - start)}.");
    }
}

public interface IChucksTrace {
    string[] MoreChucks();
    string Identifier();
}

public delegate bool DuplicatesReferee(string someJoke, string otherJoke);

public interface IChucksKeeper {
    bool IsAlreadyThere(string suspect, DuplicatesReferee isDuplicate);
    void KeepHim(string suspect, string whereWasHeFound);
}
