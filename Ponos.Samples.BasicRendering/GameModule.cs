using Autofac;
using Ponos.API;
using Ponos.API.Interfaces;
using System;

namespace Ponos.Samples.BasicRendering
{
    public class GameModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GameInstance>().AsSelf().As<IGameInstance>().As<IApplicationEventListener>();
        }
    }
}
