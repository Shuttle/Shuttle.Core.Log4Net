using System;
using log4net;
using log4net.Config;
using log4net.Core;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Core.Log4Net
{
	public class Log4NetLog : AbstractLog
	{
		private static bool _initialize = true;
		private static readonly object _padlock = new object();

		private readonly log4net.ILog _log;
		private readonly bool _configure;

		public Log4NetLog(log4net.ILog logger) : this(logger, true)
		{
		}

		public Log4NetLog(log4net.ILog logger, bool configure)
		{
			_configure = configure;

			lock (_padlock)
			{
				if (_initialize && configure)
				{
					XmlConfigurator.Configure();

					_initialize = false;
				}
			}

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

		public override void Verbose(string message)
		{
			if (_log.Logger.IsEnabledFor(Level.Verbose))
			{
				_log.Debug(string.Format("VERBOSE: {0}", message));
			}
		}

		public override void Trace(string message)
		{
			if (_log.Logger.IsEnabledFor(Level.Trace))
			{
				_log.Debug(string.Format("TRACE: {0}", message));
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

		public override Infrastructure.ILog For(Type type)
		{
			return new Log4NetLog(LogManager.GetLogger(type), _configure);
		}

		public override Infrastructure.ILog For(object instance)
		{
			return new Log4NetLog(LogManager.GetLogger(instance.GetType()), _configure);
		}

		public override bool IsTraceEnabled
		{
			get { return _log.Logger.IsEnabledFor(Level.Trace); }
		}

		public override bool IsDebugEnabled
		{
			get { return _log.IsDebugEnabled; }
		}

		public override bool IsInformationEnabled
		{
			get { return _log.IsInfoEnabled; }
		}

		public override bool IsWarningEnabled
		{
			get { return _log.IsWarnEnabled; }
		}

		public override bool IsErrorEnabled
		{
			get { return _log.IsErrorEnabled; }
		}

		public override bool IsFatalEnabled
		{
			get { return _log.IsFatalEnabled; }
		}
	}
}
