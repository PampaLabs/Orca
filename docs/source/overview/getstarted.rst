Getting started
===============

This article describes the first steps to start using Orca.

Setup
-----

First add the ``Orca`` package into your project.

.. code-block:: console
  :caption: .NET CLI

  dotnet add package Orca

.. code-block:: console
  :caption: Package Manager

  Install-Package Orca

In the *Program.cs*, register the Orca services:

.. code-block:: csharp
  :caption: Basic configuration

  builder.Services.AddOrca();

.. code-block:: csharp
  :caption: Advanced configuration

  builder.Services.AddOrca(options =>
  {
    options.DefaultClaimTypeMap = new DefaultClaimTypeMap
    {
      RoleClaimType = JwtClaimTypes.Role
      NameClaimType = JwtClaimTypes.Name
    };
  });

``AddOrca`` method registers the core services. However, you still have to configure the stores (mandatory) and the web authorization (optional).

The setup of the different stores is included as part of the documentation.

To configure web authorization, see the ASP.NET Core section. 

Usage
-----

Once the core services have been configured, you can use the interfaces provided to interact with the stores.

- ``ISubjectStore``
- ``IRoleStore``
- ``IPermissionStore``
- ``IPolicyStore``
- ``IDelegationStore``

You can access the store intances by injecting them one by one, or you can access all of them by injecting ``IOrcaStoreAccessor``.

The following interfaces, used for access control, will also be available:

- ``IAuthorizationGrantor``
- ``IPermissionEvaluator``

Example
-------

.. code-block:: csharp

  var stores = serviceProvider.GetRequiredService<IOrcaStoreAccessor>();

  var alice = new Subject { Sub = "818727", Name = "Alice" };
  await stores.SubjectStore.CreateAsync(alice);

  var viewGradesPermission = new Permission { Name = Permissions.GradesRead };
  var editGradesPermission = new Permission { Name = Permissions.GradesEdit };
  await stores.PermissionStore.CreateAsync(viewGradesPermission);
  await stores.PermissionStore.CreateAsync(editGradesPermission);

  var teacherRole = new Role { Name = nameof(Roles.Teacher), Description = "Teacher role" };
  await stores.RoleStore.CreateAsync(teacherRole);
  await stores.RoleStore.AddSubjectAsync(teacherRole, alice);
  await stores.PermissionStore.AddRoleAsync(viewGradesPermission, teacherRole);
  await stores.PermissionStore.AddRoleAsync(editGradesPermission, teacherRole);
