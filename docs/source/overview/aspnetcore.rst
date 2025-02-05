ASP.NET Core
============

This article describes how to use Orca within an ASP.NET Core web app.

Setup
-----

First add the ``Orca.AspNetCore`` package into your project.

.. code-block:: console
  :caption: .NET CLI

  dotnet add package Orca.AspNetCore

.. code-block:: console
  :caption: Package Manager

  Install-Package Orca.AspNetCore

In the *Program.cs*, register the Orca authorization services:

.. code-block:: csharp
  :caption: Basic configuration

  builder.Services
    .AddOrca()
    .AddAuthorization();

.. code-block:: csharp
  :caption: Advanced configuration

  builder.Services
    .AddOrca()
    .AddAuthorization(options =>
    {
      options.Schemes.Add("Bearer");

      options.Events.UnauthorizedFallback = (context) =>
      {
          context.Response.StatusCode = StatusCodes.Status403Forbidden;
          return Task.CompletedTask;
      };
    });

``AddAuthorization`` method configures the authorization handler for ASP.NET Core.

Usage
-----

Orca automatically maps roles and permissions to ASP.NET Core user claims.

.. code-block:: csharp
  :caption: Authorize role

  [Authorize(Roles = "custodian")]
  public IActionResult OpenDoor()
  {
    return View();
  }

.. code-block:: csharp
  :caption: Authorize policy

  [Authorize(Policy = "grades.view")]
  public IActionResult ViewGrades()
  {
    return View();
  }

.. image:: ../images/claimsidentity.png

Endpoints
---------

Orca provides a set of endpoints to interact with the stores using a minimal API.

.. code-block:: csharp

  app.MapAccessControlApi();
