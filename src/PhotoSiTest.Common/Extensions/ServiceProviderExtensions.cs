using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PhotoSiTest.Common.Extensions;

public static class ServiceProviderExtensions
{
    public static IOptions<T> GetRequiredOptions<T>(this IServiceProvider provider)
        where T : class
    {
        return provider.GetRequiredService<IOptions<T>>();
    }
}
