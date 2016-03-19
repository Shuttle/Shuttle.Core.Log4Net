Shuttle.Core.Log4Net
====================

Log4Net `ILog` implementation used by the `Log` class in the `Shuttle.Core` assembly.

# Usage

Add a reference to the `Shuttle.Core.Log4Net` package and then assign a new `Log4NetLog` to the `Log` as follows:

``` c#
Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));
```