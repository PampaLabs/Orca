namespace Orca.Authorization.Abac.Context
{
    /// <summary>
    /// Represent the collection of property bag's to use on DSL evaluation process.
    /// </summary>
    public class AbacAuthorizationContext
    {
        private readonly Dictionary<string, IPropertyBag> _propertyBagsHolder = [];

        /// <summary>
        /// Get the property bag by name.
        /// </summary>
        /// <param name="propertyBagName">The name of the property bag.</param>
        /// <returns>A dictionary that represent the items on the property bag specified by <paramref name="propertyBagName"/></returns>
        public IPropertyBag this[string propertyBagName]
        {
            get
            {
                return _propertyBagsHolder[propertyBagName];
            }
            internal set
            {
                _propertyBagsHolder.Add(propertyBagName, value);
            }
        }

        /// <summary>
        /// Adds a property bag to the collection by its name.
        /// </summary>
        /// <param name="propertyBag">The property bag to be added.</param>
        public void AddBag(IPropertyBag propertyBag)
        {
            _propertyBagsHolder.TryAdd(propertyBag.Name, propertyBag);
        }
    }
}
