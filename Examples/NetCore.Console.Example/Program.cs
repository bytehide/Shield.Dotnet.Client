using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetCore.Console.Example
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            //
			//
			//  This is an example to use Shield client in .NET Core app with async methods.
			// 
			//

			var services = new ServiceCollection();
            ConfigureServices(services);
            await using var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetService<Consumer>();
            // ReSharper disable once PossibleNullReferenceException
            await app?.Run();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<Consumer>();
        }
    }
}
