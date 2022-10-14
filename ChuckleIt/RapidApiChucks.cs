using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckleIt
{
    class RapidApiChucks : IChucker
    {
        //TODO FIXME: implementation required
        int IChucker.HowManyChucksAreThere() => 1;
        //TODO FIXME: implementation required
        string[] IChucker.GetChucks(int fromIndex, int howMany) => new[] { "Haha, chuck is so fast that he is gone before he arrives." };
        string IChucker.Identifier() => nameof(RapidApiChucks);
    }
}
