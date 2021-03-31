using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetFramework.Console.NoAsync
{
    class Program
    {
        private static void Main(string[] args)
        {
            //
            //
            //  This is an example to use Shield client in .NET Framework app with sync methods.
            //
            //  IMPORTANT: For signalR add <RestoreProjectStyle>PackageReference</RestoreProjectStyle> in .csrproj (only in NET Framework)
            // 
            //

            var services = new ServiceCollection();
            ConfigureServices(services);
            using var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetService<Consumer>();
            app?.Run();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<Consumer>();
        }
    }
}
