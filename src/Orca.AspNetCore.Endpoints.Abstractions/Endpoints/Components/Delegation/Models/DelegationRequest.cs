namespace Orca.AspNetCore.Endpoints;

public class DelegationRequest
{
    public string Who { get; set; }
    public string Whom { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool Enabled { get; set; } = true;
}
