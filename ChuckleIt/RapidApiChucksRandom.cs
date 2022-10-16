using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ChuckleIt;

//this makes some sense, but doesn't allow to control amount of pulled data, so doesn't meed requirements
class RapidApiChucksRandom : IChuckTrace
{
    string IChuckTrace.GetSomeChuck() =>
        remote.GetObject($"jokes/random").Value<string>("value");

    string IChuckTrace.Identifier() => nameof(RapidApiChucksRandom);

    readonly RapidApiClient remote = new();
}

