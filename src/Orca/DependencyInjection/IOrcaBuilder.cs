using Orca.Authorization.Abac.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Orca
{
    /// <summary>
    /// The builder used to register Orca and dependant services.
    /// </summary>
    public interface IOrcaBuilder
    {
        /// <summary>
        /// Gets the Microsoft.Extensions.DependencyInjection.IServiceCollection into which Orca services should be registered.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Allow to register a new <see cref="IPropertyBagBuilder"/> to be used on Orca DSL policies.
        /// </summary>
        /// <typeparam name="TPropertyBag">The <see cref="IPropertyBuilder"/> to be used.</typeparam>
        /// <returns>A new <see cref="IOrcaBuilder"/> that can be chained for register services.</returns>
        IOrcaBuilder AddPropertyBag<TPropertyBag>() 
            where TPropertyBag : class, IPropertyBag;
    }
}
