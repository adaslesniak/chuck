using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    class ChuckFinder
    {
        internal void Run(IChuckStore storage, IChucker[] wholeWorld) {
            foreach(var source in wholeWorld) {
                if(source == null) {
                    Log.Error("Chuck is nowhere but we can't chase him there.");
                }
                UpdateFrom(source, storage);
            }
        }

        void UpdateFrom(IChucker source, IChuckStore target) {
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

        bool UpdateNext(IChucker fromSource, int atIndex, IChuckStore jail) {
            try {
                var next = fromSource.GetChucks(atIndex, 1);
                if(next == null || next.Length == 0
                    || false == IsGoodChuck(next[0])
                    || jail.IsAlreadyThere(next[0])) {
                    return false; //TODO: optimize it, it's wasted bandwidth, some cache would be good to not look at t he same things again and again
                }
                jail.GetHim(next[0]);
                return true;
            } catch(Exception error) {
                Log.Error($"Escape was too strong in this chuck", error);
                return false;
            }
        }

        bool IsGoodChuck(string suspect) =>
            suspect != null
            && suspect.Length > "chuck".Length
            && suspect.Length <= 200;
        //&& dearAiLord.IsFunny(suspect); //that may be little to advanced for tonight :p
    }
}
