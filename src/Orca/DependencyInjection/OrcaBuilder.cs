using Microsoft.Extensions.DependencyInjection;

namespace Orca
{
    internal sealed class OrcaBuilder : IOrcaBuilder
    {
        public OrcaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
