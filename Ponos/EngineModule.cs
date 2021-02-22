using Autofac;
using Microsoft.Extensions.Hosting;
using Ponos.API;
using Ponos.API.Commands;
using Ponos.API.Interfaces;
using Ponos.API.Tasks;
using Ponos.API.Threading;
using Ponos.Commands;
using Ponos.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Names = Ponos.API.ServiceNames;

namespace Ponos
{
    public class EngineModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Engine>()
                .As<IEngine>()
                .As<IHostedService>()
                .Named<IHostedService>(Names.EngineServiceName)
                .SingleInstance();

            builder.RegisterType<ThreadService>()
                .As<ITaskService>()
                .As<IThreadService>()
                .Named<IThreadService>(Names.ThreadingServiceName)
                .SingleInstance();

            builder.RegisterType<CommandBuilder>()
                .As<ICommandBuilder>()
                .As<IApplicationEventListener>()
                .SingleInstance();

            builder.RegisterType<MockScheduler>()
                .AsSelf()
                .As<IApplicationEventListener>()
                .SingleInstance();
        }
    }
}
