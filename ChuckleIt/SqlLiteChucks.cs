using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    internal class SqlLiteChucks : IChucksKeeper
    {
        void IChucksKeeper.KeepHim(string suspect) {
            //TODO FIXME: implementation required
        }

        bool IChucksKeeper.IsAlreadyThere(string suspect, UniquenessReferee judge) {
            //that may be slow, but I am not 100% sure about business model, so prefer to get really good chucks instead of cheap ones

            //TODO FIXME: implementation required
            return false;
        }
    }
}
