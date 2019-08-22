# DCCS.ExceptionHelpers.NetStandard [![Build status](https://ci.appveyor.com/api/projects/status/pqftt8hnwdv0cssp?svg=true)](https://ci.appveyor.com/project/mgeramb/dccs-exceptionhelpers-netstandard) [![NuGet Badge](https://buildstats.info/nuget/DCCS.ExceptionHelpers.NetStandard)](https://www.nuget.org/packages/DCCS.ExceptionHelpers.NetStandard/)
Provides helper functions for exceptions to get all exceptions in the InnerException hirachy and to build full messages out of it.

Note: The AggregateException which have multiple InnerExceptions will be handled too

## Installation

Install [DCCS.ExceptionHelpers.NetStandard](https://www.nuget.org/packages/DCCS.ExceptionHelpers.NetStandard/) with NuGet:

    Install-Package DCCS.ExceptionHelpers.NetStandard

Or via the .NET Core command line interface:

    dotnet add package DCCS.ExceptionHelpers.NetStandard

Either commands, from Package Manager Console or .NET Core CLI, will download and install DCCS.ExceptionHelpers.NetStandard.

## API

Available extension methods in this package:


```csharp
    // Return the message of the exception and all inner exceptions.
    public static string GetRecursiveMessage(this Exception exception, bool includeCallstack, string separator = null)

    // Returns the exception and all inner exceptions
    public static IEnumerable<Exception> GetAllExceptionsInHirachy(this Exception exception)
```

## Examples

Sample usage of the GetRecursiveMessage message:

```csharp
// Add the namespace to your file to get the extension methods
using DCCS.ExceptionHelpers.NetStandard.Tests;

class Program
{
    public void Main()
    {
        try
        {
            ReadConfig(); // Will throw an exception
        }
        catch (Exception e)
        {
            // Trace all exceptions in the stack including the call stack
            Trace.WriteLine(e.GetRecursiveMessage(true));
                
            // Write all exception messages to the console (without the call stack)
            Console.WriteLine(e.GetRecursiveMessage(false)); 
        }
    }

    void ReadConfig()
    {
        try
        {
            // This will raise an FileNotFound exception for demonstation
            using (var configFile = File.OpenRead(Path.Combine(Path.GetTempPath(), @"NOTEXISTINGCONFIGFILE.XXX")))
            {

            }
        }
        catch (Exception e)
        {
            throw new Exception("Read configuration failed", e);
        }
    }
}
```

## Contributing
All contributors are welcome to improve this project. The best way would be to start a bug or feature request and discuss the way you want find a solution with us.
After this process, please create a fork of the repository and open a pull request with your changes. Of course, if your changes are small, feel free to immediately start your pull request without previous discussion. 
