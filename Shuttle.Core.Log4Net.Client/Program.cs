using log4net;
using Shuttle.Core.Logging;

#if (NETCOREAPP || NETSTANDARD)
using System;
using System.IO;
#endif

namespace Shuttle.Core.Log4Net.Client
{
    class Program
    {
        static void Main(string[] args)
        {
#if (!NETCOREAPP && !NETSTANDARD)
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Program))));
            Log.Information("successful - .net framework");
#else
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Program)), new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"))));
            Log.Information("successful - .net core");
#endif
        }
    }
}
