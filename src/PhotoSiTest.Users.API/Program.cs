using PhotoSiTest.Users.API.Extensions;

namespace PhotoSiTest.Users.API;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.ConfigureApplicationBuilder();

        var app = builder.Build();
        app.ConfigureWebApplication();
        await app.RunAsync();
        return 0;
    }
}
