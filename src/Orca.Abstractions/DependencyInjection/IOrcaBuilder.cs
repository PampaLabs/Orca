using Microsoft.Extensions.DependencyInjection;

namespace Orca
{
    /// <summary>
    /// The builder used to register Orca and dependant services.
    /// </summary>
    public interface IOrcaBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> into which Orca services should be registered.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
