using AutoMapper;
using NSubstitute;

namespace PhotoSiTest.Tests.Common;

public abstract class ServiceTestBase
{
    protected readonly IMapper Mapper;

    protected readonly MapperConfiguration MapperConfiguration;


    protected ServiceTestBase()
    {
        MapperConfiguration = new MapperConfiguration(ConfigureMapper);
        Mapper = MapperConfiguration.CreateMapper();
    }


    protected abstract void ConfigureMapper(IMapperConfigurationExpression config);


    protected static T CreateSubstitute<T>()
        where T : class
    {
        return Substitute.For<T>();
    }
}
