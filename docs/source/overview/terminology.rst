Terminology
===========

The documentation and object model use specific terminology that is important to understand.

Subject
^^^^^^^^
A subject represents users or entities of the application.

Role
^^^^
A role represents security groups or access levels. Multiple subjects can be assigned to the same role.
Mappings link external authentication roles to application roles, enabling seamless integration with identity providers for role-based authorization.

Permission
^^^^^^^^^^
A permission is used for **Role-Based Access Control (RBAC)**.
RBAC defines an access control strategy based on roles related to the subject to be authorized.
The permission scope is defined by assigning roles.

Policy
^^^^^^
A policy is used for **Attribute-Based Access Control (ABAC)**
ABAC defines an access control strategy based on rules related to the context to be authorized.
The policy scope is defined by rules using a high-level grammar.

Delegation
^^^^^^^^^^
A delegation allows a subject to temporarily transfer their permissions to another subject.
This is useful in scenarios where a user needs to grant another user access to perform specific actions on their behalf.
