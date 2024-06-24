using Balea;
using Balea.Store.EntityFrameworkCore;
using Balea.Store.EntityFrameworkCore.Entities;

using ContosoUniversity.EntityFrameworkCore.Store.Models;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders
{
    public static class BaleaSeeder
    {
        public static async Task Seed(BaleaDbContext db, IAccessControlContext context)
        {
            if (!db.Applications.Any())
            {
                var application = new ApplicationEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = BaleaConstants.DefaultApplicationName,
                    Description = "Default Application",
                };

                await db.Applications.AddAsync(application);
                await db.SaveChangesAsync();

                // -----------------------------------------

                var viewGradesPermission = new Permission { Name = Permissions.GradesRead };
                var editGradesPermission = new Permission { Name = Permissions.GradesEdit };
                await context.PermissionStore.CreateAsync(viewGradesPermission);
                await context.PermissionStore.CreateAsync(editGradesPermission);

                var teacherRole = new Role { Name = nameof(Roles.Teacher), Description = "Teacher role" };
                await context.RoleStore.CreateAsync(teacherRole);
                await context.RoleStore.AddSubjectAsync(teacherRole, "818727");
                await context.PermissionStore.AddRoleAsync(viewGradesPermission, nameof(Roles.Teacher));
                await context.PermissionStore.AddRoleAsync(editGradesPermission, nameof(Roles.Teacher));

                var substituteRole = new Role { Name = nameof(Roles.Substitute), Description = "Substitute role" };
                await context.RoleStore.CreateAsync(substituteRole);
                await context.RoleStore.AddSubjectAsync(substituteRole, "88421113");
                await context.PermissionStore.AddRoleAsync(viewGradesPermission, nameof(Roles.Substitute));
                await context.PermissionStore.AddRoleAsync(editGradesPermission, nameof(Roles.Substitute));

                var delegation = new Delegation { Who = "818727", Whom = "88421113", From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow.AddYears(1), Enabled = false };
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
