# NancyActuator

Spring Boot Actuator-like library for NancyFx, with out-of-the-box checks for MongoDB, PosgresSQL, Redis and SQLServer (more comming soon).


| Nuget Packages |  |
| --- | --- |
| NancyActuator | [![NuGet Version](https://buildstats.info/nuget/NancyActuator?includePreReleases=true)](https://www.nuget.org/packages/NancyActuator) |
| NancyActuator.MongoDBHealthCheck | [![NuGet Version](https://buildstats.info/nuget/NancyActuator.MongoDBHealthCheck?includePreReleases=true)](https://www.nuget.org/packages/NancyActuator.MongoDBHealthCheck) |
| NancyActuator.PostgreSQLHealthCheck | [![NuGet Version](https://buildstats.info/nuget/NancyActuator.PostgreSQLHealthCheck?includePreReleases=true)](https://www.nuget.org/packages/NancyActuator.PostgreSQLHealthCheck) |
| NancyActuator.RedisHealthCheck | [![NuGet Version](https://buildstats.info/nuget/NancyActuator.RedisHealthCheck?includePreReleases=true)](https://www.nuget.org/packages/NancyActuator.RedisHealthCheck) |
| NancyActuator.SQLServerHealthCheck | [![NuGet Version](https://buildstats.info/nuget/NancyActuator.SQLServerHealthCheck?includePreReleases=true)](https://www.nuget.org/packages/NancyActuator.SQLServerHealthCheck) |

## Why NancyActuator

NancyFx is a great project for creating web applications in .NET. Some of its strengths are its simplicity and flexibility: it does not bring fancy artifacts like interceptors, filters or monitoring features; but it makes it very easy to implement those features thanks to the capabilities brought by the Nancy bootstrappers.

One of the best features of the wonderful [Spring Boot Project](http://projects.spring.io/spring-boot/) is the [Spring Boot Actuator](https://docs.spring.io/spring-boot/docs/current/reference/htmlsingle/#production-ready), which brings tools for easily adding health checks to your applications, publishing information, checking the current configuration, reading measures, etc.

Nancy Actuator attempts to bring part of the features of Spring Boot Actuator to NancyFx and .NET. It currently provides two main features:

* A /health endpoint, in which callers of your web application can read the health status of your application. 
* An /info endpoint, in which callers of your web application can obtain information about your application. Typically useful information are the application name and version.

The `/health` and `/info` endpoints will respectively receive and invoke all the health checks (implementations of the `IHealthIndicator` interface) and information contributors (implementations of `IInfoContributor`) available in the Nancy's dependency injection mechanism.

## How To Use NancyActuator

When your Nancy application uses an IoC container with auto-registration support, all you have to do is adding a reference to the [NancyActuator Nuget project](https://www.nuget.org/packages/NancyActuator) in order to add the `/health` and `/info` endpoints to your NancyFx application.

If the `/health` and `/info` endpoints are still unavailable after adding a reference to NancyActuator in your project, you might have to explicitly register the Nancy modules for both endpoints (`HealthModule` and `InfoModule`) in your IoC container from your custom Nancy bootstrapper. For example, using Ninject:
```csharp
container.Bind<NancyModule>().To<HealthModule>();
container.Bind<NancyModule>().To<InfoModule>();
```

In order to have specific health checks or information contributors in your application you will have to add them to your IoC container too.

**Warning!:** Adding health checks and info contributors to the IoC container will result in many implementations of the same type (`IHealthIndicator` and `IInfoContributor`) being registered in the same container. The default IoC container provided by Nancy ([PicoContainer](http://picocontainer.com/)) does not support this. You have to use another IoC container instead, like [Ninject](http://www.ninject.org/) or [Autofac](https://autofac.org/). See https://github.com/NancyFx/Nancy.Bootstrappers.Ninject and https://github.com/NancyFx/Nancy.Bootstrappers.Autofac for more information about how to use Ninject or Autofac in NancyFx.


### Health Checks

There are a series of health checks available out-of-the-box, like [NancyActuator.MongoDBHealthCheck](https://www.nuget.org/packages/NancyActuator.MongoDBHealthCheck), 
[NancyActuator.PostgreSQLHealthCheck](https://www.nuget.org/packages/NancyActuator.PostgreSQLHealthCheck), [NancyActuator.RedisHealthCheck](https://www.nuget.org/packages/NancyActuator.RedisHealthCheck) and 
[NancyActuator.SQLServerHealthCheck](https://www.nuget.org/packages/NancyActuator.SQLServerHealthCheck).

In addition to them, you may create your own health indicators by implementing the `IHealthIndicator` interface:
```csharp
namespace NancyActuator.Core.Health
{
    /// <summary>
    /// Interface used to provide an indication of application Health.
    /// </summary>
    public interface IHealthIndicator
    {
        /// <summary>
        /// Return an indication of Health.
        /// </summary>
        Health Health();
    }
}
```

In order to use both, the out-of-the-box and your custom health checks, tou have to explicitly register them as instances of `IHealthIndicator` in your IoC container. This way they will be invoked and shown any time a caller invokes the `/health` endpoint.

For instance, the registration in Ninject of the SQLServer health indicator would look like this:

```csharp
container.Bind<IHealthIndicator>().ToConstant(
                new SqlServerHealthIndicator(connectionString));
```


### Information Contributors

Nancy already provides two implementations of info contributor:

* `SimpleInfoContributor`, which provides a single piece of information with a name and an object.
* `MapInfoContributor`, which provides information from a Dictionary of string keys and objects.

In addition to them, you can create your own information contributors by implementing the `IInfoContributor` interface:
```csharp
namespace NancyActuator.Core.Info
{
    /// <summary>
    /// Contributes additional info details.
    /// </summary>
    public interface IInfoContributor
    {
        /// <summary>
        /// Contributes additional details using the specified <see cref="Info.Builder"/>.
        /// </summary>
        /// <param name="builder">Build to use to create a new instance of info.</param>
        void Contribute(Info.Builder builder);
    }
}
```

You can use any of already available information contributors or you own to publish information about your application in the `/info` endpoint. In order to do so you have to explicitly register them as instances of `IInforContributor` in your IoC container from your custom Nancy bootstrapper. For instance, a `MapInfoContributor` registration in Ninject:

```csharp
var applicationInfo = new Dictionary<string, object>();
applicationInfo.Add("applicationName", "My Application Name");
applicationInfo.Add("version", "1.0.0");
                
container.Bind<IInfoContributor>().ToConstant(new MapInfoContributor(applicationInfo));
```