namespace Orca
{
    /// <summary>
    /// Contains the claim types used for authorization.
    /// </summary>
    public static class OrcaClaims
    {
        /// <summary>
        /// Represents the claim type for a permission.
        /// </summary>
        public const string Permission = "permission";

        /// <summary>
        /// Represents the claim type for the subject who delegated the permission.
        /// </summary>
        public const string DelegatedBy = "delegatedby";

        /// <summary>
        /// Represents the claim type for the subject from whom the permission was delegated.
        /// </summary>
        public const string DelegatedFrom = "delegatedfrom";

        /// <summary>
        /// Represents the claim type for the subject to whom the permission was delegated.
        /// </summary>
        public const string DelegatedTo = "delegatedto";
    }
}
