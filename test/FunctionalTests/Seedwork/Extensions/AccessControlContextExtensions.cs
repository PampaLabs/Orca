using Orca;

namespace FunctionalTests.Seedwork
{
    public static class AccessControlContextExtensions
    {
        public static async Task GivenAnApplication(
            this IOrcaStoreAccessor stores)
        {
            var viewGradesPermission = new Permission { Name = Policies.ViewGrades };
            var editGradesPermission = new Permission { Name = Policies.EditGrades };

            await stores.PermissionStore.CreateAsync(viewGradesPermission);
            await stores.PermissionStore.CreateAsync(editGradesPermission);
        }

        public static async Task GivenAnSubject(
            this IOrcaStoreAccessor stores,
            string sub)
        {
            var subject = new Subject { Sub = sub, Name = sub };

            await stores.SubjectStore.CreateAsync(subject);
        }

        public static async Task GivenARole(
            this IOrcaStoreAccessor stores,
            string name,
            string sub,
            bool withPermissions = true)
        {
            var role = new Role
            {
                Name = name,
                Description = $"{name} role"
            };

            var subject = await stores.SubjectStore.FindBySubAsync(sub);

            await stores.RoleStore.CreateAsync(role);
            await stores.RoleStore.AddSubjectAsync(role, subject);

            if (withPermissions)
            {
                var permissions = await stores.PermissionStore.ListAsync();
                
                foreach (var permission in permissions)
                {
                    await stores.PermissionStore.AddRoleAsync(permission, role);
                }
            }
        }

        public static async Task GivenAPolicy(
            this IOrcaStoreAccessor stores,
            string policyName,
            string policyContent)
        {
            var policy = new Policy
            {
                Name = policyName,
                Content = policyContent,
            };

            await stores.PolicyStore.CreateAsync(policy);
        }

        public static async Task GivenAnUserWithADelegation(
            this IOrcaStoreAccessor stores,
            string who,
            string whom,
            bool enabled = true)
        {
            var subjectWho = await stores.SubjectStore.FindBySubAsync(who);
            var subjectWhom = await stores.SubjectStore.FindBySubAsync(whom);

            var delegation = new Delegation
            {
                Who = subjectWho,
                Whom = subjectWhom,
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow.AddDays(1),
                Enabled = enabled,
            };

            await stores.DelegationStore.CreateAsync(delegation);
        }
    }
}
