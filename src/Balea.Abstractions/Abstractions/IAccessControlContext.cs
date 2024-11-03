namespace Balea
{
    public interface IAccessControlContext
    {
        IDelegationStore DelegationStore { get; }
        IPermissionStore PermissionStore { get; }
        IPolicyStore PolicyStore { get; }
        IRoleStore RoleStore { get; }
        ISubjectStore SubjectStore { get; }
    }
}
