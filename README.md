Shuttle.Core.Log4Net
====================

Log4Net `ILog` implementation used by the `Log` class in the `Shuttle.Core` assembly.

# Usage

Add a reference to the `Shuttle.Core.Log4Net` package and then assign a new `Log4NetLog` to the `Log` as follows:

``` c#
Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));
```

# Configuration

Since this implementation wraps the `Log4Net` log you would use the `Log4Net` [configuration options](https://logging.apache.org/log4net/release/manual/configuration.html).

Here is a sample configuration but there are [many examples online](https://logging.apache.org/log4net/release/config-examples.html):

``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
	</configSections>

	<log4net>
		<appender name="ConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%d [%t] %-5p %c - %m%n"/>
			</layout>
		</appender>
		<appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
			<file value="logs\program"/>
			<appendToFile value="true"/>
			<rollingStyle value="Composite"/>
			<maxSizeRollBackups value="10"/>
			<maximumFileSize value="100000KB"/>
			<datePattern value="-yyyyMMdd.'log'"/>
			<param name="StaticLogFileName" value="false"/>
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%d [%t] %-5p %c - %m%n"/>
			</layout>
		</appender>
		<root>
			<level value="TRACE"/>
			<appender-ref ref="ConsoleAppender"/>
			<appender-ref ref="RollingFileAppender"/>
		</root>
	</log4net>
</configuration>
```