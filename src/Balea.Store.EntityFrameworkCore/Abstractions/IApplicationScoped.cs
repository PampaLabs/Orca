namespace Balea.Store.EntityFrameworkCore.Entities;

internal interface IApplicationScoped
{
	public string ApplicationId { get; set; }

	public ApplicationEntity Application { get; set; }
}
