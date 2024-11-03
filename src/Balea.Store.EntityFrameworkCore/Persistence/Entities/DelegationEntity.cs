namespace Balea.Store.EntityFrameworkCore.Entities;

public class DelegationEntity
{
    public string Id { get; set; }
    public string WhoId { get; set; }
    public string WhomId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool Enabled { get; set; } = true;

    public SubjectEntity Who { get; set; }
    public SubjectEntity Whom { get; set; }
}
