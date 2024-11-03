using Balea;

namespace FunctionalTests.Seedwork
{
    public static class AccessControlContextExtensions
    {
        public static async Task GivenAnApplication(
            this IAccessControlContext context)
        {
            var viewGradesPermission = new Permission { Name = Policies.ViewGrades };
            var editGradesPermission = new Permission { Name = Policies.EditGrades };

            await context.PermissionStore.CreateAsync(viewGradesPermission);
            await context.PermissionStore.CreateAsync(editGradesPermission);
        }

        public static async Task GivenAnSubject(
            this IAccessControlContext context,
            string sub)
        {
            var subject = new Subject { Sub = sub, Name = sub };

            await context.SubjectStore.CreateAsync(subject);
        }

        public static async Task GivenARole(
            this IAccessControlContext context,
            string name,
            string sub,
            bool withPermissions = true)
        {
            var role = new Role
            {
                Name = name,
                Description = $"{name} role"
            };

            var subject = await context.SubjectStore.FindBySubAsync(sub);

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
        }

        public static async Task GivenAPolicy(
            this IAccessControlContext context,
            string policyName,
            string policyContent)
        {
            var policy = new Policy
            {
                Name = policyName,
                Content = policyContent,
            };

            await context.PolicyStore.CreateAsync(policy);
        }

        public static async Task GivenAnUserWithADelegation(
            this IAccessControlContext context,
            string who,
            string whom,
            bool enabled = true)
        {
            var subjectWho = await context.SubjectStore.FindBySubAsync(who);
            var subjectWhom = await context.SubjectStore.FindBySubAsync(whom);

            var delegation = new Delegation
            {
                Who = subjectWho,
                Whom = subjectWhom,
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow.AddDays(1),
                Enabled = enabled,
            };

            await context.DelegationStore.CreateAsync(delegation);
        }
    }
}
