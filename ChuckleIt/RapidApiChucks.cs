using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    class RapidApiChucks : IChucksTrace
    {
        //TODO FIXME: implementation required
        int IChucksTrace.HowManyChucksAreThere() => 1;
        //TODO FIXME: implementation required
        string[] IChucksTrace.GetChucks(int fromIndex, int howMany) => new[] { "Haha, chuck is so fast that he is gone before he arrives." };
        string IChucksTrace.Identifier() => nameof(RapidApiChucks);
    }
}
