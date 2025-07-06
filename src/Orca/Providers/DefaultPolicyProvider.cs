namespace Orca
{
    /// <inheritdoc />
    public class DefaultPolicyProvider : IPolicyProvider
    {
        private readonly IOrcaStoreAccessor _storeAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPolicyProvider"/> class.
        /// </summary>
        /// <param name="storeAccessor">The store accessor.</param>
        public DefaultPolicyProvider(IOrcaStoreAccessor storeAccessor)
        {
            _storeAccessor = storeAccessor ?? throw new ArgumentNullException(nameof(storeAccessor));
        }

        /// <inheritdoc />
        public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _storeAccessor.PolicyStore.FindByNameAsync(name, cancellationToken);
        }
    }
}