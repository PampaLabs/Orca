using Balea;
using Balea.Store.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static async Task ImportConfigurationAsync(this IApplicationBuilder host)
    {
        using var scope = host.ApplicationServices.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider);
    }

    public static async Task ImportConfigurationAsync(this IWebHost host)
    {
        using var scope = host.Services.CreateScope();

        await ImportConfigurationAsync(scope.ServiceProvider);
    }

    public static async Task ImportConfigurationAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<IAccessControlContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var baleaConfig = new BaleaConfiguration();
        configuration.GetSection("Balea").Bind(baleaConfig);

        foreach (var item in baleaConfig.Subjects)
        {
            var subject = new SubjectMapper().FromEntity(item);
            await context.SubjectStore.CreateAsync(subject);
        }

        foreach (var item in baleaConfig.Policies)
        {
            var policy = new PolicyMapper().FromEntity(item);
            await context.PolicyStore.CreateAsync(policy);
        }

        foreach (var item in baleaConfig.Permissions)
        {
            var permissions = new PermissionMapper().FromEntity(item);
            await context.PermissionStore.CreateAsync(permissions);
        }

        foreach (var item in baleaConfig.Roles)
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
