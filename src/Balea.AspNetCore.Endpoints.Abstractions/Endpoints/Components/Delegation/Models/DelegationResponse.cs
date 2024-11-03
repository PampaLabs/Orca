namespace Balea.AspNetCore.Endpoints;

public class DelegationResponse
{
    public string Id { get; set; }
    public SubjectResponse Who { get; set; }
    public SubjectResponse Whom { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool Enabled { get; set; } = true;
}
