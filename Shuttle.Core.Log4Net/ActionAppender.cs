using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;

namespace Shuttle.Core.Log4Net
{
	public class ActionAppender : AppenderSkeleton
	{
		private readonly Action<LoggingEvent> _action;

		public static void Register(Action<LoggingEvent> action)
		{
			Guard.AgainstNull(action, "action");

			Log.Trace($"Registering ActionAppender against action '{action.GetType().FullName}'.");

			((Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly())).Root.AddAppender(new ActionAppender(action));
		}

		public ActionAppender(Action<LoggingEvent> action)
		{
			Guard.AgainstNull(action, "action");

			_action = action;
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				return;
			}

			_action.Invoke(loggingEvent);
		}
	}
}