using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ChuckleIt
{
    public class Upchuckle
    {
        [FunctionName("UpdateChucks")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting update of chucks.");
            new Updater().Run(new[] { new SomeSourceOfChucks()} );
            log.LogInformation("Done with chuckling.");
        }
    }

    class SomeSourceOfChucks : IChucker {

        int IChucker.HowManyChucksAreThere() => 1;

        string[] IChucker.GetChucks(int fromIndex, int howMany) => new[] { "Haha, chuck is so fast that he is gone before he arrives." };

    }


    interface IChucker {
        int HowManyChucksAreThere();
        string[] GetChucks(int fromIndex = 0, int howMany = 1);  
    }
    
    class Updater {

        internal void Run(IChucker[] wholeWorld) {
            foreach(var source in wholeWorld) {
                UpdateFrom(source);
            }
        }

        void UpdateFrom(IChucker source) {
            int allThere = source.HowManyChucksAreThere();
            for(int i = 0; i < allThere; i++) {
                UpdateNext(i, source);    
            }
        }

        void UpdateNext(int atIndex, IChucker fromSource) {
            var next = fromSource.GetChucks(atIndex, 1);
            if(next == null || next.Length == 0
                || false == IsGoodChuck(next[0])
                || IsLocalChuck(next[0])) {
                return; //TODO: optimize it, it's wasted bandwidth
            }
            LocalizeGoodChuck(next[0]);
        }

        bool IsGoodChuck(string suspect) {
            //TODO FIXME: implementation required
            return true;
        }

        bool IsLocalChuck(string suspect) {
            //TODO FIXME: implementation required
            return true;
        }

        void LocalizeGoodChuck(string chuck) {

        }
    }
}
