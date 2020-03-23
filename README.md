# Logging.Samples
This project contains samples for .NET Core and ASP.NET Core logging using various logger frameworks.

![this](Resources/.NET_Core_Logo_small.png)

## Executing code
Navigate to ../ConsoleApp, ../WebApi or ../WebApp and execute ```dotnet run serilog``` or ```dotnet run nlog```. Execute ```dotnet test``` for the TestApp. 

## Contents

Logging is an important aspect of any application framework. Compared to Java logging, configuring .NET Core and ASP.NET Core applications for logging seems trivial at first, until we encounter various application and logger framework specifics: each logger framework implementation requires configuration with custom methods at various places in application configuration and startup classes. To complicate matters further, framework specifics related to async code execution make it impossible to correlate log entries based on the thread id - async methods may switch threads during different stages of execution, so it is not possible to distinguish which log entry belongs to a specific activity without some sort of log entry correlation being provided with each log entry.

Logging best practices are demonstrated for tests (TestApp), console (ConsoleApp), ASP.NET Core Controller (WebApi), ASP.NET Core Blazor Server and Entity Framework Core (WebApp) applications while also demonstrating how to solve the log entry correlation problem with the help of the [Logging](https://github.com/akovac35/Logging) library. In addition, the use of compile-time logger invocation context is demonstrated, transparently providing method name and source code line number to the logger framework without the use of reflection.

All samples are configured to demonstrate the configuration of common logging requirements for every sampled logger framework implementation:

* Logging to console,
* logging to files using daily rotation with configured maximum number of historic files and maximum file size,
* logger configuration update during application execution,
* log entry correlation for web api requests and Blazor Server circuit,
* Entity Framework Core logging,
* compile-time logger invocation context information,
* serialization of complex log entry parameters using only general template patterns common to every sampled logger framework,
* best practice log entry layout configuration containing information about timestamp, time zone, log level, thread id, correlation, invocation context and message, making it possible to understand application activity and troubleshoot problems effectively,
* log filtering for context and log level.

Log sample for WebApp is provided below:

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

Each logger framework reads configuration from files and the above example demonstrates how to specify them. Regarding logger configuration update during application execution, this is either taken care of by the logger framework implementation or by the [Logging](https://github.com/akovac35/Logging) library - as long as the demonstrated patterns are adhered to. Be advised that executing the samples with the ```dotnet run``` command causes some frameworks to monitor the root configuration files (not the ones in the /bin folder). Execute the samples from the ```dotnet publish``` contents to work around this problem.

Regarding log entry correlation, console applications achieve it easily with logger scopes:

```cs
using (Logger.BeginScope(new[] { new KeyValuePair<string, object>(Constants.CorrelationId, 12345678) }))
{
    List<Task<int>> tasks = new List<Task<int>>();
    for (int i = 0; i < 10; i++)
    {
        tasks.Add(BusinessLogicMock<object>.GetTaskInstance());
    }

    // Business logic call sample
    await Task.WhenAll(tasks);
}
```

ASP.NET Core applications achieve log entry correlation with the use of the ```ICorrelationProvider``` scoped service:

```cs
services.AddHttpContextAccessor();
services.AddScoped<ICorrelationProvider, CorrelationProvider>();
services.AddScoped<WeatherForecastService>(fact =>
{
    ICorrelationProvider correlationProvider = (new HttpContextAccessor()).GetCorrelationProvider();
    return new WeatherForecastService(correlationProvider);
});
```

A relevant scope instance must exist for the ```ICorrelationProvider``` to be available via the ```HttpContextAccessor```. If it is not available, use direct logger scopes instead. Do note that Controller scope is per request while Blazor Server circuit scope is per user; this is explained in [ASP.NET Core Blazor dependency injection](https://docs.microsoft.com/en-us/aspnet/core/blazor/dependency-injection?view=aspnetcore-3.1#service-lifetime) document.

For Razor pages, correlation can be retrieved with an instance of ```HttpContextAccessor``` as follows:

```razor
@page "/counter"

@using Microsoft.Extensions.Logging
@using Microsoft.AspNetCore.Http
@using global::com.github.akovac35.Logging.AspNetCore
@using global::com.github.akovac35.Logging
@using global::Shared.Mocks

@using static global::com.github.akovac35.Logging.LoggerHelper<WebApp.Pages.Counter>

@inject IHttpContextAccessor hc

<h1>Counter</h1>

<p>Correlation id:  @hc.GetCorrelationId()</p>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private async Task IncrementCount()
    {
        Here(l => l.Entering());

        currentCount++;
        Here(l => l.LogInformation("currentCount: {currentCount}", currentCount));

        // Business logic call sample
        var blMock = new BusinessLogicMock<object>();
        await blMock.FirstLevelAsync(500);

        Here(l => l.Exiting());
    }
}
```

Similarly for Controllers:

```cs
protected IHttpContextAccessor _contextAccessor = new HttpContextAccessor();

[HttpGet]
public async Task<WeatherForecast[]> Get()
{
    _logger.Here(l => l.Entering());

    var forecasts = await _forecastService.GetForecastAsync(DateTime.Now);
    _logger.Here(l => l.LogInformation("CorrelationId for a request instance can be obtained with HttpContextAccessor: {@correlationId}", _contextAccessor.GetCorrelationId()));

    _logger.Here(l => l.Exiting(forecasts));
    return forecasts;
}
```

If used, the ```CorrelationIdMiddleware``` will find and extract the ```x-request-id``` header value and use it for log correlation:

```cs
app.UseMiddleware<CorrelationIdMiddleware>();
```

```
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -H "x-request-id: 12345678" -k https://localhost:5001/weatherforecast

[2020-03-23 00:26:57.287 +01:00] INF 4 54cf926d-457d-443e-a8e7-898cae0fb0ae <Microsoft.AspNetCore.Hosting.Diagnostics::> Request starting HTTP/2 GET https://localhost:5001/weatherforecast application/json 
[2020-03-23 00:26:57.291 +01:00] INF 4 12345678 <Microsoft.AspNetCore.Routing.EndpointMiddleware::> Executing endpoint '"WebApi.Controllers.WeatherForecastController.Get (WebApi)"'
[2020-03-23 00:26:57.294 +01:00] INF 4 12345678 <Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker::> Route matched with "{action = \"Get\", controller = \"WeatherForecast\"}". Executing controller action with signature "System.Threading.Tasks.Task`1[Shared.Services.WeatherForecast[]] Get()" on controller "WebApi.Controllers.WeatherForecastController" ("WebApi").
[2020-03-23 00:26:57.297 +01:00] VRB 4 12345678 <Shared.Services.WeatherForecastService:.ctor:18> Entering: CorrelationProvider { Value: Correlation { Id: "12345678" } }
[2020-03-23 00:26:57.300 +01:00] VRB 4 12345678 <Shared.Services.WeatherForecastService:.ctor:23> Exiting
[2020-03-23 00:26:57.302 +01:00] VRB 4 12345678 <WebApi.Controllers.WeatherForecastController:.ctor:29> Entering: WeatherForecastService {  }
[2020-03-23 00:26:57.307 +01:00] VRB 4 12345678 <WebApi.Controllers.WeatherForecastController:.ctor:34> Exiting
[2020-03-23 00:26:57.309 +01:00] VRB 4 12345678 <WebApi.Controllers.WeatherForecastController:Get:46> Entering
[2020-03-23 00:26:57.311 +01:00] VRB 4 12345678 <Shared.Services.WeatherForecastService:GetForecastAsync:37> Entering: 03/23/2020 00:26:57
[2020-03-23 00:26:57.313 +01:00] INF 4 12345678 <Shared.Services.WeatherForecastService:GetForecastAsync:39> CorrelationId is useful for correlating log contents with service or web page requests: "12345678"
[2020-03-23 00:26:57.315 +01:00] VRB 4 12345678 <Shared.Services.WeatherForecastService:GetForecastAsync:49> Exiting: Task`1 { Result: [WeatherForecast { Date: 03/24/2020 00:26:57, TemperatureC: -16, TemperatureF: 4, Summary: "Warm" }, WeatherForecast { Date: 03/25/2020 00:26:57, TemperatureC: -13, TemperatureF: 9, Summary: "Sweltering" }, WeatherForecast { Date: 03/26/2020 00:26:57, TemperatureC: 32, TemperatureF: 89, Summary: "Chilly" }, WeatherForecast { Date: 03/27/2020 00:26:57, TemperatureC: 42, TemperatureF: 107, Summary: "Cool" }, WeatherForecast { Date: 03/28/2020 00:26:57, TemperatureC: 27, TemperatureF: 80, Summary: "Hot" }], Id: 102, Exception: null, Status: RanToCompletion, IsCanceled: False, IsCompleted: True, IsCompletedSuccessfully: True, CreationOptions: None, AsyncState: null, IsFaulted: False }
[2020-03-23 00:26:57.334 +01:00] INF 4 12345678 <WebApi.Controllers.WeatherForecastController:Get:49> CorrelationId for a request instance can be obtained with HttpContextAccessor: "12345678"
[2020-03-23 00:26:57.338 +01:00] VRB 4 12345678 <WebApi.Controllers.WeatherForecastController:Get:51> Exiting: WeatherForecast { Date: 03/24/2020 00:26:57, TemperatureC: -16, TemperatureF: 4, Summary: "Warm" }
[2020-03-23 00:26:57.343 +01:00] INF 4 12345678 <Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor::> Executing ObjectResult, writing value of type '"Shared.Services.WeatherForecast[]"'.
[2020-03-23 00:26:57.346 +01:00] INF 4 12345678 <Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker::> Executed action "WebApi.Controllers.WeatherForecastController.Get (WebApi)" in 48.6032ms
[2020-03-23 00:26:57.348 +01:00] INF 4 12345678 <Microsoft.AspNetCore.Routing.EndpointMiddleware::> Executed endpoint '"WebApi.Controllers.WeatherForecastController.Get (WebApi)"'
[2020-03-23 00:26:57.352 +01:00] INF 4 12345678 <Serilog.AspNetCore.RequestLoggingMiddleware::> HTTP "GET" "/weatherforecast" responded 200 in 61.7780 ms
[2020-03-23 00:26:57.355 +01:00] INF 4  <Microsoft.AspNetCore.Hosting.Diagnostics::> Request finished in 68.0659ms 200 application/json; charset=utf-8
```

Regarding invocation context information, it is only available for logger invocations inside the ```Here``` method:

```cs
_logger.Here(l => l.Entering());

//or

using static com.github.akovac35.Logging.LoggerHelper<WebApp.Pages.Counter>
// ...
Here(l => l.Entering());
```

To configure Entity Framework Core logging, provide logger factory instance to the ```DbContextOptionsBuilder```:

```cs
services.AddTransient<BlogContext>(fact =>
{
    Here(l => l.Entering());

    // This connection is explicitly provided so we have to manage it explicitly by
    // opening, closing and disposing it
    var connection = fact.GetService<SqliteConnection>();

    // Provided by logger frameworks as a singleton
    var loggerFactory = fact.GetService<ILoggerFactory>();
    var options = new DbContextOptionsBuilder<BlogContext>()
    .UseSqlite(connection)
    .UseLoggerFactory(loggerFactory)
    .Options;
    var context = new BlogContext(options);

    Here(l => l.ExitingSimpleFormat(context));
    return context;
});
```

Log filtering for context and log level is demonstrated in logger framework configuration files.