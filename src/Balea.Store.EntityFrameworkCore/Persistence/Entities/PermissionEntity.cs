namespace Balea.Store.EntityFrameworkCore.Entities;

public class PermissionEntity : IApplicationScoped
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	public string ApplicationId { get; set; }
	public ApplicationEntity Application { get; set; }
}
