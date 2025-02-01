# Orca

[![CI](https://github.com/OrcaAuth/Orca/actions/workflows/ci.yml/badge.svg)](https://github.com/OrcaAuth/Orca/actions/workflows/ci.yml)
[![Downloads](https://img.shields.io/nuget/dt/Orca)](https://www.nuget.org/stats/packages/Orca?groupby=Version)
[![NuGet](https://img.shields.io/nuget/v/Orca)](https://www.nuget.org/packages/Orca/)
[![Documentation](https://img.shields.io/readthedocs/_)](https://_.readthedocs.io/en/stable/)

Orca is an authorization framework for .NET developers that aims to help us decoupling authentication and authorization in our web applications.

This project is a fork of [Balea](https://github.com/Xabaril/Balea). The foundational basis and the magnificent idea are due to the team behind [Xabaril](https://github.com/Xabaril). 🙌

The project documentation has not yet been migrated. 🚧

## Getting started

First add the Orca packages into your project.

```
dotnet add package Orca
dotnet add package Orca.Store.EntityFrameworkCore
dotnet add package Orca.AspNetCore
```

In the _Program.cs_, register the Orca services:

```csharp
builder.Services
  .AddOrca()                    // core services (Orca)
  .AddEntityFrameworkStores()   // store implementations (Orca.Store.EntityFrameworkCore)
  .AddAuthorization();          // authorization handler (Orca.AspNetCore)
```

## How is it different from Balea?

The main purpose of Orca is to introduce some abstractions, and fundamentally redefine what a store is.

### Motivation

The following key points describe the underlying vision behind this fork.

- Standardization of models (e.g. Delegation, Permission, Policy, Role, Subject)
- Well-defined store interfaces, in order to provide common CRUD operations.
- Decoupled authorization logic. Authorization behavior is defined in the core library, so stores only deal with IO operations.
- Enable the use of authorization concerns from the application logic through abstractions.

### New features

Some subsequent benefits turned out to be low hanging fruits.

- The configuration store has been replaced by the in-memory store. However, it can be populated from the configuration.
- The EntityFramework store has been improved to allow extensibility and customization of the _DbContext_.
- The HTTP store has been rewritten to be fully compliant with the new store interfaces.
- Endpoint provisioning has been included to allow exposing an HTTP store server using minimal API.
- Blazor samples have been generated to provide common GUI templates.

### Other changes

The code base received additional enhancements.

- Decompling of Abstractions/Core/AspNetCore packages.
- Common data seeding strategy across different stores.
- Improved tests to use theorems for the different schemas and stores.
- Adoption of _TestContainers_.

## What about multi-tentant support?

If you have been using Balea, you may be wondering what happened to the support for multiple applications (a.k.a. tenants).

At the time of generating abstractions, the inclusion of the application concept may result in complex interfaces to interact with.

Also, it is worth asking "Should subjects be shared between applications?". Each solution will probably find different answers according to their needs. The same question may be applied to some other models.

Orca chose to adopt a more flexible approach, delegating multi-tenancy concerns to the store configuration.

### EntityFrameworkCore

The _EntityFrameworkCore_ store options allows configuring the _DbContext_. [Finbuckle](https://github.com/Finbuckle/Finbuckle.MultiTenant) could be adopted for data isolation.

### HTTP

The _HTTP_ store options allows configuring the _HttpClientFactory_. A custom _header_ could be included to send the tenant identifier.

## Authentication != Authorization

Authentication and authorization might sound similar, but both are distinct security processes in the world of identity and access management and understanding the difference between these two concepts is the key to successfully implementing a good IAM solution.

While authentication is the act of verifying oneself, authorization is the process of verifying what you have access to, so coupling identity and access management in a single solution is not considered a good approach. Authentication is really good to provide a common identity across all applications while authorization is something that varies in each application, for these reasons we should treat them independently.

It is very common to see how people misuse OIDC servers by adding permissions into tokens and there are many reasons why this approach is a wrong solution:

- Permissions are something that depends on each application and sometimes depends on complex business rules.
- Permissions could change during the user session, so if you are using JWT tokens, you must wait until the lifetime of the token expires to retrieve a new token with the permissions up to date.
- You should keep your tokens small because we have some well-known restrictions such as URL Path Length Restrictions, bandwidth...

## Technologies

- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit](https://xunit.net/)
- [Fluent Assertions](https://fluentassertions.com/)
- [TestContainers](https://testcontainers.com/)

## Code of conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
