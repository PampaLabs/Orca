HTTP
====

This article describes how to use Orca with HTTP store.

Setup
-----

First add the ``Orca.Store.Http`` package into your project.

.. code-block:: console
  :caption: .NET CLI

  dotnet add package Orca.Store.Http

.. code-block:: console
  :caption: Package Manager

  Install-Package Orca.Store.Http

In the *Program.cs*, register the HTTP store:

.. code-block:: csharp
  :caption: Basic configuration

  builder.Services
    .AddOrca()
    .AddHttpStores(httpClient => {
        httpClient.BaseAddress = new Uri("https://api.orca.local/");
    });

``AddInMemoryStores`` method configures Orca stores to use REST API persistence.
