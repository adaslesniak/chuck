using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    internal class SqlLiteChucks : IChuckStore
    {
        void IChuckStore.GetHim(string suspect) {
            //TODO FIXME: implementation required
        }

        bool IChuckStore.IsAlreadyThere(string suspect, Comparison<string> judge) {
            //that may be slow, but I am not 100% sure about business model, so prefer to get really good chucks instead of cheap ones

            //TODO FIXME: implementation required
            return false;
        }
    }
}
