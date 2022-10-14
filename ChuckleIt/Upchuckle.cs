using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace ChuckleIt
{
    public class Upchuckle
    {
        [FunctionName("UpdateChucks")]
        public void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
            //TODO FIXME: configuration required
            Log.Setup(log);
            Log.Info($"Starting update of chucks.");
            new ChuckFinder().Run(
                new SqlLiteChucks(),
                new[] { new RapidApiChucks()} );
            Log.Info("Done with chuckling.");
        }
    }

    public interface IChucker {
        int HowManyChucksAreThere();
        string[] GetChucks(int fromIndex = 0, int howMany = 1);
        string Identifier();
    }

    public interface IChuckStore {
        bool IsAlreadyThere(string suspect, Comparison<string> comparator);
        void GetHim(string suspect);
    }
}
