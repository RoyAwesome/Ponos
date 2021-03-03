using Autofac;
using Ponos.API.Interfaces;
using Ponos.API.Rendering;
using Ponos.API.Threading;
using Ponos.API.Window;
using Ponos.RenderGraph.Veldrid;
using System;

namespace Ponos.RenderGraph
{
    public class VeldridModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<VeldridWindowService>()
                .As<IWindowService>()
                .As<IApplicationEventListener>()
                .As<IThreadLocked>()
                .SingleInstance();

            builder.RegisterType<VeldridRenderingService>()
                .As<IRenderingService>()
                .SingleInstance();
        }
    }
}
