Balea
=====

This project is a fork of `Balea <https://github.com/Xabaril/Balea>`_. The foundational basis and the magnificent idea are due to the team behind `Xabaril <https://github.com/Xabaril>`_. 🙌

Motivation
----------

The following key points describe the underlying vision behind this fork.

- Standardization of models (e.g. Delegation, Permission, Policy, Role, Subject)
- Well-defined store interfaces, in order to provide common CRUD operations.
- Decoupled authorization logic. Authorization behavior is defined in the core library, so stores only deal with IO operations.
- Enable the use of authorization concerns from the application logic through abstractions.

New features
------------

Some subsequent benefits turned out to be low hanging fruits.

- The configuration store has been replaced by the in-memory store. However, it can be populated from the configuration.
- The EntityFramework store has been improved to allow extensibility and customization of the *DbContext*.
- The HTTP store has been rewritten to be fully compliant with the new store interfaces.
- Endpoint provisioning has been included to allow exposing an HTTP store server using minimal API.
- Blazor samples have been generated to provide common GUI templates.

Other changes
-------------

The code base received additional enhancements.

- Decompling of Abstractions/Core/AspNetCore packages.
- Common data seeding strategy across different stores.
- Improved tests to use theorems for the different schemas and stores.
- Adoption of *TestContainers*.
