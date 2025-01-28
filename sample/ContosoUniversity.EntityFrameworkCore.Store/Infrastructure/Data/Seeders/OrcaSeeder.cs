using Orca;

using ContosoUniversity.EntityFrameworkCore.Store.Models;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders
{
    public static class OrcaSeeder
    {
        public static async Task SeedDataContextAsync(this IApplicationBuilder host, Func<IOrcaStoreAccessor, Task> seed = null)
        {
            using var scope = host.ApplicationServices.CreateScope();

            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await Seed(stores);
        }

        private static async Task Seed(IOrcaStoreAccessor stores)
        {
            var records = await stores.SubjectStore.ListAsync();

            if (!records.Any())
            {
                var alice = new Subject { Sub = "818727", Name = "Alice" };
                var bob = new Subject { Sub = "88421113", Name = "Bob" };
                await stores.SubjectStore.CreateAsync(alice);
                await stores.SubjectStore.CreateAsync(bob);

                var viewGradesPermission = new Permission { Name = Permissions.GradesRead };
                var editGradesPermission = new Permission { Name = Permissions.GradesEdit };
                await stores.PermissionStore.CreateAsync(viewGradesPermission);
                await stores.PermissionStore.CreateAsync(editGradesPermission);

                var teacherRole = new Role { Name = nameof(Roles.Teacher), Description = "Teacher role" };
                await stores.RoleStore.CreateAsync(teacherRole);
                await stores.RoleStore.AddSubjectAsync(teacherRole, alice);
                await stores.PermissionStore.AddRoleAsync(viewGradesPermission, teacherRole);
                await stores.PermissionStore.AddRoleAsync(editGradesPermission, teacherRole);

                var substituteRole = new Role { Name = nameof(Roles.Substitute), Description = "Substitute role" };
                await stores.RoleStore.CreateAsync(substituteRole);
                await stores.RoleStore.AddSubjectAsync(substituteRole, bob);
                await stores.PermissionStore.AddRoleAsync(viewGradesPermission, substituteRole);
                await stores.PermissionStore.AddRoleAsync(editGradesPermission, substituteRole);

                var delegation = new Delegation { Who = alice, Whom = bob, From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow.AddYears(1), Enabled = false };
                await stores.DelegationStore.CreateAsync(delegation);

                var studentRole = new Role { Name = nameof(Roles.Student), Description = "Student role", Mappings = ["customer"] };
                await stores.RoleStore.CreateAsync(studentRole);

                var policyContent =
    @"policy substitute begin
    rule A (DENY) begin
        Subject.Role CONTAINS ""Substitute"" AND Resource.Controller = ""Grades"" AND Parameters.Value > 6
    end
end";
                var policy = new Policy { Name = "ValidateGrades", Content = policyContent };
                await stores.PolicyStore.CreateAsync(policy);
            }
        }
    }
}
