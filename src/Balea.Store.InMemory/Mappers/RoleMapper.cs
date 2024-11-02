namespace Balea.Store.Configuration;

internal class RoleMapper : IEntityMapper<RoleConfiguration, Role>
{
    public void FromEntity(RoleConfiguration source, Role destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }

    public void ToEntity(Role source, RoleConfiguration destination)
    {
        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Enabled = source.Enabled;
        destination.Mappings = [.. source.Mappings];
    }
}
