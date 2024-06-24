namespace Balea
{
    public class AuthorizationContext
    {
    	public IEnumerable<Role> Roles { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        public Delegation Delegation { get; set; }
    }
}
