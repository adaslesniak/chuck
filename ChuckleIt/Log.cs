using Microsoft.Extensions.Logging;
using System;

namespace Downchuckle;
static class Log
{
    static ILogger implemnetation;

    internal static void Crash(string message, Exception fuckup = null) {
        //here it would be worthy to send somewhere crash report
        implemnetation?.LogCritical(fuckup, message);
    }

    internal static void Error(string message, Exception error = null) =>
        implemnetation?.LogError(error, message);

    internal static void Info(string message) =>
        implemnetation?.LogInformation(message);
    

    internal static void Setup(ILogger withImplementation) {
        implemnetation = withImplementation;
    }
}
