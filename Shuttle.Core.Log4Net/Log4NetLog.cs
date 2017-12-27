using System;
#if (NETCOREAPP2_0 || NETSTANDARD2_0)
using System.IO;
using System.Reflection;
#endif
using log4net;
using log4net.Config;
using log4net.Core;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using ILog = log4net.ILog;

namespace Shuttle.Core.Log4Net
{
    public class Log4NetLog : AbstractLog
    {
#if (!NETCOREAPP2_0 && !NETSTANDARD2_0)
        private static bool _initialize = true;
#endif
        private static readonly object Lock = new object();

        private ILog _log;

#if (!NETCOREAPP2_0 && !NETSTANDARD2_0)
        public Log4NetLog(ILog logger) : this(logger, true)
        {
        }

        public Log4NetLog(ILog logger, bool configure)
        {
            lock (Lock)
            {
                if (_initialize && configure)
                {
                    XmlConfigurator.Configure();

                    _initialize = false;
                }
            }

            ConfigureLogger(logger);
        }
#else
        public Log4NetLog(ILog logger)
        {
            ConfigureLogger(logger);
        }

        public Log4NetLog(ILog logger, FileInfo fileInfo)
        {
            Guard.AgainstNull(fileInfo, nameof(fileInfo));

            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), fileInfo);

            ConfigureLogger(logger);
        }
#endif

        private void ConfigureLogger(ILog logger)
        {
            Guard.AgainstNull(logger, nameof(logger));

            LogLevel = LogLevel.Off;

            _log = logger;

            if (logger.Logger.IsEnabledFor(Level.Verbose))
            {
                LogLevel = LogLevel.Verbose;
            }
            else if (logger.Logger.IsEnabledFor(Level.Trace))
            {
                LogLevel = LogLevel.Trace;
            }
            else if (logger.Logger.IsEnabledFor(Level.Debug))
            {
                LogLevel = LogLevel.Debug;
            }
            else if (logger.Logger.IsEnabledFor(Level.Info))
            {
                LogLevel = LogLevel.Information;
            }
            else if (logger.Logger.IsEnabledFor(Level.Warn))
            {
                LogLevel = LogLevel.Warning;
            }
            else if (logger.Logger.IsEnabledFor(Level.Error))
            {
                LogLevel = LogLevel.Error;
            }
            else if (logger.Logger.IsEnabledFor(Level.Fatal))
            {
                LogLevel = LogLevel.Fatal;
            }
        }

        public override bool IsTraceEnabled => _log.Logger.IsEnabledFor(Level.Trace);

        public override bool IsDebugEnabled => _log.IsDebugEnabled;

        public override bool IsInformationEnabled => _log.IsInfoEnabled;

        public override bool IsWarningEnabled => _log.IsWarnEnabled;

        public override bool IsErrorEnabled => _log.IsErrorEnabled;

        public override bool IsFatalEnabled => _log.IsFatalEnabled;

        public override void Verbose(string message)
        {
            if (_log.Logger.IsEnabledFor(Level.Verbose))
            {
                _log.Debug($"VERBOSE: {message}");
            }
        }

        public override void Trace(string message)
        {
            if (_log.Logger.IsEnabledFor(Level.Trace))
            {
                _log.Debug($"TRACE: {message}");
            }
        }

        public override void Debug(string message)
        {
            _log.Debug(message);
        }

        public override void Warning(string message)
        {
            _log.Warn(message);
        }

        public override void Information(string message)
        {
            _log.Info(message);
        }

        public override void Error(string message)
        {
            _log.Error(message);
        }

        public override void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public override Logging.ILog For(Type type)
        {
            Guard.AgainstNull(type, nameof(type));

#if (!NETCOREAPP2_0 && !NETSTANDARD2_0)
            return new Log4NetLog(LogManager.GetLogger(type), false);
#else
            return new Log4NetLog(LogManager.GetLogger(type));
#endif
        }

        public override Logging.ILog For(object instance)
        {
            Guard.AgainstNull(instance, nameof(instance));

#if (!NETCOREAPP2_0 && !NETSTANDARD2_0)
            return new Log4NetLog(LogManager.GetLogger(instance.GetType()), false);
#else
            return new Log4NetLog(LogManager.GetLogger(instance.GetType()));
#endif
        }
    }
}