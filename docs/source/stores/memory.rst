InMemory
========

This article describes how to use Orca with InMemory store.

Setup
-----

First add the ``Orca.Store.InMemory`` package into your project.

.. code-block::
  :caption: .NET CLI
  dotnet add package Orca.Store.InMemory

.. code-block::
  :caption: Package Manager
  Install-Package Orca.Store.InMemory

In the *Program.cs*, register the InMemory store:

.. code-block:: csharp
  builder.Services
    .AddOrca()
    .AddInMemoryStores(Configuration);

``AddInMemoryStores`` method configures Orca stores to use in-memory persistence.

Import configuration
--------------------

The InMemory store allows importing the configuration from ``appsettings.json``.

.. code-block:: csharp
  if (app.Environment.IsDevelopment())
  {
    await app.ImportConfigurationAsync();
  }

.. code-clock:: csharp
  {
    "Orca": {
      "subjects": [
        {
          "subject": "1",
          "name": "alice"
        },
        {
          "subject": "2",
          "name": "bob"
        }
      ],
      "roles": [
        {
          "name": "teacher",
          "enabled": true,
          "description": "Teacher role",
          "permissions": [
            "edit.grades",
            "view.grades"
          ],
          "subjects": [
            "1"
          ]
        }
      ],
      "permissions": [
        {
          "name": "edit.grades"
        },
        {
          "name": "view.grades"
        }
      ],
      "delegations": [
        {
          "who": "1",
          "whom": "2",
          "from": "1900-01-01 00:00:00",
          "to": "9999-12-31 23:59:59",
          "enabled": true
        }
      ],
      "policies": [
        {
          "name": "abac-policy",
          "content": "policy substitute begin\n    rule A (PERMIT) begin\n  Subject.Role CONTAINS \"Teacher\" AND Subject.Sub = \"1\" AND Resource.Controller = \"School\" \n  end\nend"
        }
      ]
    }
  }
