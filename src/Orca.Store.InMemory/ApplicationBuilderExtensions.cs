using Orca;
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
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ImportConfigurationAsync(this IApplicationBuilder host)
    {
        using var scope = host.ApplicationServices.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider);
    }

    /// <summary>
    /// Imports configuration from the application's configuration and stores it in the provided services.
    /// </summary>
    /// <param name="host">The <see cref="IWebHost"/> instance used to create a scope for accessing the services.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ImportConfigurationAsync(this IWebHost host)
    {
        using var scope = host.Services.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider);
    }

    /// <summary>
    /// Imports configuration from the application's configuration and stores it in the provided services.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance that provides access to the services.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private static async Task ImportConfigurationAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<IAccessControlContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var config = new OrcaConfiguration();
        configuration.GetSection("Orca").Bind(config);

        foreach (var item in config.Subjects)
        {
            var subject = new SubjectMapper().FromEntity(item);
            await context.SubjectStore.CreateAsync(subject);
        }

        foreach (var item in config.Policies)
        {
            var policy = new PolicyMapper().FromEntity(item);
            await context.PolicyStore.CreateAsync(policy);
        }

        foreach (var item in config.Permissions)
        {
            var permissions = new PermissionMapper().FromEntity(item);
            await context.PermissionStore.CreateAsync(permissions);
        }

        foreach (var item in config.Roles)
        {
            var role = new RoleMapper().FromEntity(item);
            await context.RoleStore.CreateAsync(role);

            foreach (var sub in item.Subjects)
            {
                var subject = await context.SubjectStore.FindBySubAsync(sub);
                await context.RoleStore.AddSubjectAsync(role, subject);
            }

            foreach (var binding in item.Permissions)
            {
                var permission = await context.PermissionStore.FindByNameAsync(binding);
                await context.RoleStore.AddPermissionAsync(role, permission);
            }
        }
    }
}
