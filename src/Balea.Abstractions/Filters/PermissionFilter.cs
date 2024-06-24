namespace Balea
{
    public class PermissionFilter
    {
        public string Name { get; set; }
    	public string Description { get; set; }

        public IList<string> Roles { get; set; }
    }
}
