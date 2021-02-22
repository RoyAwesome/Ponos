﻿using System;
using Ponos.API;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading.Tasks;
using System.Threading;
using Autofac;
using Autofac.Core;
using System.Collections.Generic;
using Ponos.API.Threading;
using Ponos.Threading;
using Ponos.API.Interfaces;

namespace Ponos
{
    public class Engine : IEngine, IHostedService
    {
        private readonly ILogger Logger;
        private readonly IHostApplicationLifetime lifetime;
        private readonly IComponentContext componentContext;
        private ILifetimeScope baseScope;
        private ILifetimeScope EngineScope;
        private IThreadService threadService;

        public Engine(ILogger logger, IHostApplicationLifetime lifetime, IComponentContext componentContext, ILifetimeScope lifetimeScope, IThreadService threadService)
        {
            Logger = logger;
            this.lifetime = lifetime;
            this.componentContext = componentContext;
            baseScope = lifetimeScope;
            this.threadService = threadService;
        }

        public string Name => "Ponos Engine";

        public string Version => "Development";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopped.Register(OnStopped);
            lifetime.ApplicationStopping.Register(OnStopping);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Logger.Info("Starting up {0} version: {1}", Name, Version);

            var threader = componentContext.Resolve<IThreadService>() as ThreadService;
            int numberOfThreads = Environment.ProcessorCount;
            Logger.Info("Starting up Threading Service, {0}, with {1} threads", threader.Name, numberOfThreads);
            threader.Startup(numberOfThreads);




            Logger.Info("Found Game Instances: ");
            var gameInstances = componentContext.Resolve<IEnumerable<IGameInstance>>();
            foreach(var gi in gameInstances)
            {
                Logger.Info("\t{0} ({1})", gi.Name, gi.Version);
            }

            var GameInstace = componentContext.Resolve<IGameInstance>();
            Logger.Info("Using {0} ({1})", GameInstace, GameInstace.Version);

            threader.Enqueue(() => BeginApplicationStage(ApplicationEvent.Startup));

        }

        private void OnStopping()
        {
            Logger.Info("Shutting down the engine");
            EngineScope.Dispose();
        }

        private void OnStopped()
        {

        }

        protected void BeginApplicationStage(ApplicationEvent stage)
        {
            //TODO: Split this work up onto the task graph!
            var listeners = componentContext.Resolve<IEnumerable<IApplicationEventListener>>();
            foreach(var listener in listeners)
            {
                listener.OnApplicationEvent(stage);
            }
        }
    }
}