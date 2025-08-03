using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace CarAuction.Common.Tests.Unitary.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static T AddSubstituteFor<T>(this ServiceCollection services, params object[] constructorArguments) where T : class
    {
        return AddSubstituteFor<T>(services, ServiceLifetime.Singleton, constructorArguments);
    }

    public static T AddSubstituteFor<T>(this ServiceCollection services, ServiceLifetime serviceLifetime, params object[] constructorArguments) where T : class
    {
        var substitute = Substitute.For<T>(constructorArguments);
        var serviceDescriptor = new ServiceDescriptor(typeof(T), provider => substitute, serviceLifetime);
        services.Add(serviceDescriptor);
        return substitute;
    }
        


}
