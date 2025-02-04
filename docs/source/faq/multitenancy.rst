Multitenancy
============

If you have been using Balea, you may be wondering what happened to the support for multiple applications (a.k.a. tenants).

At the time of generating abstractions, the inclusion of the application concept may result in complex interfaces to interact with.
Also, it is worth asking "Should subjects be shared between applications?". Each solution will probably find different answers according to their needs. The same question may be applied to some other models.

Orca chose to adopt a more flexible approach, delegating multi-tenancy concerns to the store configuration.

EntityFrameworkCore
-------------------

The *EntityFrameworkCore* store options allows configuring the *DbContext*. `Finbuckle <https://github.com/Finbuckle/Finbuckle.MultiTenant>`_ could be adopted for data isolation.

HTTP
----

The *HTTP* store options allows configuring the *HttpClient*. A custom *header* could be included to send the tenant identifier.
