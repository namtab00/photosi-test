using Microsoft.Extensions.Configuration;

namespace PhotoSiTest.Common.Extensions;

public static class ConfigurationExtensions
{
    public static void BindRequiredOptions<T>(this IConfiguration configuration, T option)
    {
        configuration.GetRequiredSection(typeof(T).Name).Bind(option);
    }
}
