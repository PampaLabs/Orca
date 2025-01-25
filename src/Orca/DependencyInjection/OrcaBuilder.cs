using Orca.Authorization.Abac.Context;
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

        public IOrcaBuilder AddPropertyBag<TPropertyBag>() where TPropertyBag: class, IPropertyBag
        {
            Services.AddScoped<IPropertyBag, TPropertyBag>();
            return this;
        }
    }
}
