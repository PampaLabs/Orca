﻿namespace Orca.AspNetCore.Endpoints;

internal class PermissionRequestMapper : IDataMapper<Permission, PermissionRequest>
{
    public void FromEntity(Permission source, PermissionRequest destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
    }

    public void ToEntity(PermissionRequest source, Permission destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
    }
}
