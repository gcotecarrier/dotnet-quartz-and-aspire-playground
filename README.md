## Overview

The focus of the project was about setting up:
- A [Quartz scheduler](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html) running on a local environment with a SQL Server database
- A local dev environment orchestrated by [.NET aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
- OpenTelemetry instrumentation and collection, most specifically custom metrics [with the modern .NET way (System.Diagnostics.Metrics)](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation) of doing so

## How to run

You will require a docker engine active to run some containers

### With an IDE
I've been using the Rider IDE to run this locally targeting the `DemoApplication.Local` aspire project.

### With the CLI Tool
If you want to run the whole Aspire thing from the command line, you'll have to install the Aspire CLI tool:
```sh
dotnet tool install -g Aspire.Cli
```
Then, you'll be able to run the project locally:
```sh
aspire run
```
