Open Visual Studio 2022 Professional
Restore NuGet packages
Build the solution and Run
Click try it out and enter the number of best stories to return
It is ASP.NET CORE API runing on .NET6 Framework
NuGet pacakges
Mediatr NuGet package for mediator and command/query pattern
Swagger NuGet package - Open API specification
Serilog for logging

There are not unit tests or any tests, I would write unit tests if it were to be a production api and 
 run the tests as part of the build CI/CD pipepline

 In memory caching is used but for a production app, Redis Cache would be a better choice

 Retry policies would be implemented for production apps
