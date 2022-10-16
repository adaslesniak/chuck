using Microsoft.Extensions.Logging;
using System;

namespace Downchuckle;
static class Log
{
    static ILogger implemnetation;

    internal static void Error(string message, Exception fuckup = null) {
        if(fuckup is null) {
            implemnetation?.LogError(message);
        } else {
            implemnetation?.LogError(fuckup, message);
        }
    }

    internal static void Info(string message) =>
        implemnetation?.LogInformation(message);

    internal static void Setup(ILogger withImplementation) {
        implemnetation = withImplementation;
    }
}
