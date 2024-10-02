using Balea;

namespace FunctionalTests.Seedwork
{
    public static class FixtureExtensions
    {
        public static async Task GivenAnApplication(
            this TestServerFixture fixture)
        {
            await fixture.ExecuteDbContextAsync(async context =>
            {
                var viewGradesPermission = new Permission { Name = Policies.ViewGrades };
                var editGradesPermission = new Permission { Name = Policies.EditGrades };

                await context.PermissionStore.CreateAsync(viewGradesPermission);
                await context.PermissionStore.CreateAsync(editGradesPermission);
            });
        }

        public static async Task GivenARole(
            this TestServerFixture fixture,
            string name,
            string subject,
            bool withPermissions = true)
        {
            var role = new Role
            {
                Name = name,
                Description = $"{name} role"
            };

            await fixture.ExecuteDbContextAsync(async context =>
            {
                await context.RoleStore.CreateAsync(role);
                await context.RoleStore.AddSubjectAsync(role, subject);

                if (withPermissions)
                {
                    var permissions = await context.PermissionStore.ListAsync();
                
                    foreach (var permission in permissions)
                    {
                        await context.PermissionStore.AddRoleAsync(permission, role);
                    }
                }
            });
        }

        public static async Task GivenAPolicy(
            this TestServerFixture fixture,
            string policyName,
            string policyContent)
        {
            var policy = new Policy
            {
                Name = policyName,
                Content = policyContent,
            };

            await fixture.ExecuteDbContextAsync(async context =>
            {
                await context.PolicyStore.CreateAsync(policy);
            });
        }

        public static async Task GivenAnUserWithADelegation(
            this TestServerFixture fixture,
            string who,
            string whom,
            bool enabled = true)
        {
            await fixture.ExecuteDbContextAsync(async context =>
            {
                var delegation = new Delegation
                {
                    Who = who,
                    Whom = whom,
                    From = DateTime.UtcNow.AddDays(-1),
                    To = DateTime.UtcNow.AddDays(1),
                    Enabled = enabled,
                };

                await context.DelegationStore.CreateAsync(delegation);
            });
        }
    }
}
