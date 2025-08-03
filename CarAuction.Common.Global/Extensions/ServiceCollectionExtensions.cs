using Microsoft.Extensions.DependencyInjection;

namespace CarAuction.Common.Global.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAllByAssembly<TInterface>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var interfaceType = typeof(TInterface);
            var adapterType = interfaceType.Assembly.GetTypes()
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            adapterType.ToList().ForEach(implementation =>
            {
                var serviceDescriptor = new ServiceDescriptor(interfaceType, implementation, lifetime);
                services.Add(serviceDescriptor);
                services.Add(new ServiceDescriptor(implementation, implementation, lifetime));
            });

            return services;
        }
    }
}
