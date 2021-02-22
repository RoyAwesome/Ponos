using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.NLog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ponos;
using Ponos.API;
using NLog.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Names = Ponos.API.ServiceNames;
using Ponos.RenderGraph;

namespace Ponos.Startup
{
    class Program
    {
        static async Task Main(string[] args) =>
            await Host.CreateDefaultBuilder(args)                    
                    .ConfigureLogging(builder => {
                        ConfigureLogger();
                        builder.ClearProviders();
                        builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())                 
                    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                    .RunConsoleAsync();
      
              
        private static void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<NLogModule>();
            containerBuilder.RegisterModule<EngineModule>();
            containerBuilder.RegisterModule<VeldridModule>(); //TODO: Make this discoverable
            containerBuilder.RegisterType<NullGameInstance>().As<IGameInstance>().SingleInstance();
        }

        private static void ConfigureLogger()
        {
            var Config = new NLog.Config.LoggingConfiguration();
            var LogConsole = new NLog.Targets.ColoredConsoleTarget("LogConsole");
            var FileTarget = new NLog.Targets.FileTarget("File")
            {
                FileName = "latest.log",
                //Layout = "${message}"
            };

           
            Config.AddRuleForAllLevels(LogConsole);
            //Config.AddRuleForAllLevels(FileTarget);

            NLog.LogManager.Configuration = Config;
        }
    }
}
