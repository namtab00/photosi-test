using AutoMapper;

namespace PhotoSiTest.Common.Mapping;

public abstract class MappingProfileBase : Profile
{
    protected MappingProfileBase()
    {
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    }
}
