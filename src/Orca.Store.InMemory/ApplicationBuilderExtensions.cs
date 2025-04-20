﻿using Orca;
using Orca.Store.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for importing configuration into the application services.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Imports configuration from the application's configuration and stores it in the provided services.
    /// </summary>
    /// <param name="host">The <see cref="IApplicationBuilder"/> instance used to create a scope for accessing the services.</param>
    /// <param name="section">The name of the configuration section.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ImportConfigurationAsync(this IApplicationBuilder host, string section)
    {
        using var scope = host.ApplicationServices.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider, section);
    }

    /// <summary>
    /// Imports configuration from the application's configuration and stores it in the provided services.
    /// </summary>
    /// <param name="host">The <see cref="IWebHost"/> instance used to create a scope for accessing the services.</param>
    /// <param name="section">The name of the configuration section.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ImportConfigurationAsync(this IWebHost host, string section)
    {
        using var scope = host.Services.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider, section);
    }

    /// <summary>
    /// Imports configuration from the application's configuration and stores it in the provided services.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance that provides access to the services.</param>
    /// <param name="section">The name of the configuration section.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private static async Task ImportConfigurationAsync(IServiceProvider serviceProvider, string section)
    {
        var stores = serviceProvider.GetRequiredService<IOrcaStoreAccessor>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var config = new OrcaConfiguration();
        configuration.GetSection(section).Bind(config);

        foreach (var item in config.Subjects)
        {
            var subject = new SubjectMapper().FromEntity(item);
            await stores.SubjectStore.CreateAsync(subject);
        }

        foreach (var item in config.Policies)
        {
            var policy = new PolicyMapper().FromEntity(item);
            await stores.PolicyStore.CreateAsync(policy);
        }

        foreach (var item in config.Permissions)
        {
            var permissions = new PermissionMapper().FromEntity(item);
            await stores.PermissionStore.CreateAsync(permissions);
        }

        foreach (var item in config.Roles)
        {
            var role = new RoleMapper().FromEntity(item);
            await stores.RoleStore.CreateAsync(role);

            foreach (var sub in item.Subjects)
            {
                var subject = await stores.SubjectStore.FindBySubAsync(sub);
                await stores.RoleStore.AddSubjectAsync(role, subject);
            }

            foreach (var binding in item.Permissions)
            {
                var permission = await stores.PermissionStore.FindByNameAsync(binding);
                await stores.RoleStore.AddPermissionAsync(role, permission);
            }
        }
    }
}
