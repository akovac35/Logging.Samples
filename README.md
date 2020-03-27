# Logging.Samples
This project contains samples for .NET Core and ASP.NET Core logging using various logger frameworks.

Also demonstrated is functionality provided in the [Logging](https://github.com/akovac35/Logging) library. **You should get familiar with it first to understand the samples**. The library implementation itself is also a good example on how to use various logger frameworks.

![this](Resources/.NET_Core_Logo_small.png)

## Executing code
Navigate to ../ConsoleApp, ../WebApi or ../WebApp and execute ```dotnet run serilog``` or ```dotnet run nlog```. Execute ```dotnet test``` for the TestApp. 

## Contents

Logging best practices are demonstrated for tests (TestApp), console (ConsoleApp), ASP.NET Core Controller (WebApi), ASP.NET Core Blazor Server and Entity Framework Core (WebApp) applications while also demonstrating how to solve the log entry correlation problem with the help of the [Logging](https://github.com/akovac35/Logging) library. In addition, the use of compile-time logger invocation context is demonstrated, transparently providing method name and source code line number to the logger framework without the use of reflection.

All samples are configured to demonstrate the configuration of common logging requirements for every sampled logger framework implementation:

* Logging to console,
* logging to files using daily rotation with configured maximum number of historic files and maximum file size,
* logger configuration update during application execution,
* log entry correlation for web API requests and Blazor Server circuit,
* Entity Framework Core logging,
* compile-time logger invocation context information,
* serialization of complex log entry parameters using only general template patterns common to every sampled logger framework,
* best practice log entry layout configuration containing information about timestamp, time zone, log level, thread id, correlation, invocation context and message, making it possible to understand application activity and troubleshoot problems effectively,
* log filtering for context and log level.

A log sample for WebApp is provided below:

```
[2020-03-22 22:44:16.076 +01:00] TRA 9 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.FetchData:OnInitializedAsync:54> Entering
[2020-03-22 22:44:16.084 +01:00] TRA 9 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Services.WeatherForecastService:GetForecastAsync:37> Entering: "2020-03-22T21:44:16.0845892Z"
[2020-03-22 22:44:16.084 +01:00] INF 9 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Services.WeatherForecastService:GetForecastAsync:39> CorrelationId is useful for correlating log contents with service or web page requests: "47e580ac-f480-474d-8d25-ada51e8c183a"
[2020-03-22 22:44:16.084 +01:00] TRA 9 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Services.WeatherForecastService:GetForecastAsync:49> Exiting: {"Result":[{"Date":"2020-03-23T21:44:16.0845892Z", "TemperatureC":25, "TemperatureF":76, "Summary":"Chilly"},{"Date":"2020-03-24T21:44:16.0845892Z", "TemperatureC":52, "TemperatureF":125, "Summary":"Scorching"},{"Date":"2020-03-25T21:44:16.0845892Z", "TemperatureC":46, "TemperatureF":114, "Summary":"Balmy"},{"Date":"2020-03-26T21:44:16.0845892Z", "TemperatureC":31, "TemperatureF":87, "Summary":"Bracing"},{"Date":"2020-03-27T21:44:16.0845892Z", "TemperatureC":34, "TemperatureF":93, "Summary":"Warm"}], "Id":2, "Status":"RanToCompletion", "IsCanceled":false, "IsCompleted":true, "IsCompletedSuccessfully":true, "CreationOptions":"None", "IsFaulted":false}
[2020-03-22 22:44:16.103 +01:00] INF 9 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.FetchData:OnInitializedAsync:57> forecasts: {"Date":"2020-03-23T21:44:16.0845892Z", "TemperatureC":25, "TemperatureF":76, "Summary":"Chilly"}
[2020-03-22 22:44:16.103 +01:00] TRA 9 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.FetchData:OnInitializedAsync:59> Exiting
[2020-03-22 22:44:45.243 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.Counter:IncrementCount:26> Entering
[2020-03-22 22:44:45.243 +01:00] INF 19 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.Counter:IncrementCount:29> currentCount: 1
[2020-03-22 22:44:45.258 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:.ctor:16> Entering
[2020-03-22 22:44:45.258 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:.ctor:18> Exiting
[2020-03-22 22:44:45.272 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:FirstLevelAsync:49> Entering: 500
[2020-03-22 22:44:45.272 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:SecondLevelAsync:59> Entering: 500
[2020-03-22 22:44:45.286 +01:00] TRA 19 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:ThirdLevelAsync:69> Entering: 500
[2020-03-22 22:44:45.795 +01:00] TRA 14 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:ThirdLevelAsync:73> Exiting: 500
[2020-03-22 22:44:45.811 +01:00] TRA 14 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:SecondLevelAsync:63> Exiting: 500
[2020-03-22 22:44:45.811 +01:00] TRA 14 47e580ac-f480-474d-8d25-ada51e8c183a <Shared.Mocks.BusinessLogicMock:FirstLevelAsync:53> Exiting: 500
[2020-03-22 22:44:45.822 +01:00] TRA 14 47e580ac-f480-474d-8d25-ada51e8c183a <WebApp.Pages.Counter:IncrementCount:35> Exiting
```

## Quickstart

Search samples for ```SamplesLoggingHelper``` to easily find code related to logger framework configuration. For example:

```cs
SamplesLoggingHelper.LoggerInit(args, configActionNLog: () =>
{
    NLogHelper.CreateLogger("NLog.config");
    LoggerFactoryProvider.LoggerFactory = NLogHelper.CreateLoggerFactory();
}, configActionSerilog: () =>
{
    SerilogHelper.CreateLogger(configure => configure.AddJsonFile("serilog.json", optional: false, reloadOnChange: true));
    LoggerFactoryProvider.LoggerFactory = SerilogHelper.CreateLoggerFactory();
});
```

Each logger framework reads configuration from files and the above example demonstrates how to specify them.

Regarding logger configuration update during application execution, this is either taken care of by the logger framework implementation or by the [Logging](https://github.com/akovac35/Logging) library - as long as the demonstrated patterns are adhered to.

Log filtering for context and log level is demonstrated in logger framework configuration files.

Be advised that executing the samples with the ```dotnet run``` command causes some frameworks to monitor the root configuration files (not the ones in the /bin folder). Execute the samples from the ```dotnet publish``` contents to work around this problem.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[Apache-2.0](LICENSE)
