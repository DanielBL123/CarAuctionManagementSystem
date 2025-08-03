using AutoMapper;
using NSubstitute;
using Microsoft.Extensions.Logging;

namespace CarAuction.Common.Tests.Unitary.Extensions;

public static class MapperExtensions
{
    public static IMapper CreateMapper<TSource, TDestination>()
    {

        var loggerFactory = Substitute.For<ILoggerFactory>();
        var logger = Substitute.For<ILogger>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TSource, TDestination>()
                .ForCtorParam("id", opt => opt.MapFrom(src => 1)); ;
        }, loggerFactory);

        return config.CreateMapper();
    }
}


