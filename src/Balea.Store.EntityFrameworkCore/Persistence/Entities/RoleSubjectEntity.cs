namespace Balea.Store.EntityFrameworkCore.Entities;

public class RoleSubjectEntity
{
    public int Id { get; set; }
    public string Sub { get; set; }
    public string RoleId { get; set; }
    public RoleEntity Role { get; set; }
}
