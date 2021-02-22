using Ponos.API.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ponos.API.Interfaces;
using Ponos.API;
using Ponos.API.Threading;
using System.Threading;
using Ponos.API.Tasks;
using Veldrid.StartupUtilities;
using Veldrid;
using Veldrid.Sdl2;
using System.Numerics;
using Ponos.API.Commands;
using System.Runtime.CompilerServices;

namespace Ponos.RenderGraph.Veldrid
{
    internal class VeldridWindowService : IWindowService, IApplicationEventListener, IThreadLocked
    {
        public string Name => "Veldrid Window Service";

        Thread _thread;
        public Thread RequiredThread
        {
            get
            {
                return _thread;
            } 
            set
            {
                _thread = value;
            }
        }

        public bool KeepRunning
        {
            get;
            set;
        } = true;
      
      
        readonly ICommandBuilder commandBuilder;

        public Sdl2Window SDL2Window;

        Vector2 ScreenSize = new Vector2(800, 600);

        public VeldridWindowService(ICommandBuilder commandBuilder)
        {
            this.commandBuilder = commandBuilder;
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
            if(Stage == ApplicationEvent.Startup)
            {
                commandBuilder.InManualStage(StageNames.RendererInit, (stage) =>
                {
                    stage.AddSystem(new CreateWindowService(this));
                })
                .InFixedRateStage(StageNames.Fixed_60Hz, 1.0 / 60.0, (stage) =>
                {
                    stage.AddSystem(new PumpWindowEventsService(this));
                });
            }
            if(Stage == ApplicationEvent.Shutdown)
            {
                KeepRunning = false;
            }
        }

        class CreateWindowService : ICommandSystem
        {
            public VeldridWindowService windowService;

            public CreateWindowService(VeldridWindowService windowService)
            {
                this.windowService = windowService;
            }

            public void Run()
            {
                WindowCreateInfo windowCI = new WindowCreateInfo
                {
                    X = 100,
                    Y = 100,
                    WindowWidth = (int)windowService.ScreenSize.X,
                    WindowHeight = (int)windowService.ScreenSize.Y,
                    WindowInitialState = WindowState.Normal,
                    WindowTitle = "Intro",
                };

                windowService.SDL2Window = VeldridStartup.CreateWindow(ref windowCI);
            }
        }

        class PumpWindowEventsService : ICommandSystem
        {
            public VeldridWindowService windowService;

            public PumpWindowEventsService(VeldridWindowService windowService)
            {
                this.windowService = windowService;
            }
            public void Run()
            {
                windowService.SDL2Window.PumpEvents();
            }
        }    
    }
}
