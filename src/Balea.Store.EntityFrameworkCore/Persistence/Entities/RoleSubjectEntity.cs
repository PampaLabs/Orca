namespace Balea.Store.EntityFrameworkCore.Entities;

public class RoleSubjectEntity
{
    public string SubjectId { get; set; }
    public string RoleId { get; set; }

    public RoleEntity Role { get; set; }
    public SubjectEntity Subject { get; set; }
}
