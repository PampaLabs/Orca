namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceDescriptorExtensions
{
    public static object CreateInstance(this ServiceDescriptor descriptor, IServiceProvider serviceProvider)
    {
        if (descriptor.ImplementationInstance is not null)
        {
            return descriptor.ImplementationInstance;
        }
        else if (descriptor.ImplementationFactory is not null)
        {
            return descriptor.ImplementationFactory(serviceProvider);
        }
        else if (descriptor.ImplementationType is not null)
        {
            return ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ImplementationType);
        }
        else
        {
            throw new Exception("Unable to create the service instance.");
        }
    }
}
