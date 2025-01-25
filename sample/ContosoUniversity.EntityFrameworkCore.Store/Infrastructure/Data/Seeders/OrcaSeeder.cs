using Orca;

using ContosoUniversity.EntityFrameworkCore.Store.Models;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders
{
    public static class OrcaSeeder
    {
        public static async Task SeedDataContextAsync(this IApplicationBuilder host, Func<IAccessControlContext, Task> seed = null)
        {
            using var scope = host.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IAccessControlContext>();

            await Seed(context);
        }

        private static async Task Seed(IAccessControlContext context)
        {
            var records = await context.SubjectStore.ListAsync();

            if (!records.Any())
            {
                var alice = new Subject { Sub = "818727", Name = "Alice" };
                var bob = new Subject { Sub = "88421113", Name = "Bob" };
                await context.SubjectStore.CreateAsync(alice);
                await context.SubjectStore.CreateAsync(bob);

                var viewGradesPermission = new Permission { Name = Permissions.GradesRead };
                var editGradesPermission = new Permission { Name = Permissions.GradesEdit };
                await context.PermissionStore.CreateAsync(viewGradesPermission);
                await context.PermissionStore.CreateAsync(editGradesPermission);

                var teacherRole = new Role { Name = nameof(Roles.Teacher), Description = "Teacher role" };
                await context.RoleStore.CreateAsync(teacherRole);
                await context.RoleStore.AddSubjectAsync(teacherRole, alice);
                await context.PermissionStore.AddRoleAsync(viewGradesPermission, teacherRole);
                await context.PermissionStore.AddRoleAsync(editGradesPermission, teacherRole);

                var substituteRole = new Role { Name = nameof(Roles.Substitute), Description = "Substitute role" };
                await context.RoleStore.CreateAsync(substituteRole);
                await context.RoleStore.AddSubjectAsync(substituteRole, bob);
                await context.PermissionStore.AddRoleAsync(viewGradesPermission, substituteRole);
                await context.PermissionStore.AddRoleAsync(editGradesPermission, substituteRole);

                var delegation = new Delegation { Who = alice, Whom = bob, From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow.AddYears(1), Enabled = false };
                await context.DelegationStore.CreateAsync(delegation);

                var studentRole = new Role { Name = nameof(Roles.Student), Description = "Student role", Mappings = ["customer"] };
                await context.RoleStore.CreateAsync(studentRole);

                var policyContent =
    @"policy substitute begin
    rule A (DENY) begin
        Subject.Role CONTAINS ""Substitute"" AND Resource.Controller = ""Grades"" AND Parameters.Value > 6
    end
end";
                var policy = new Policy { Name = "ValidateGrades", Content = policyContent };
                await context.PolicyStore.CreateAsync(policy);
            }
        }
    }
}
