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
      
        ITaskService jobService;

        public Sdl2Window SDL2Window;

        Vector2 ScreenSize = new Vector2(400, 600);

        public VeldridWindowService(ITaskService jobService, IThreadService threadService)
        {
            this.jobService = jobService;
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
            if(Stage == ApplicationEvent.Startup)
            {
                jobService.Enqueue(CreateWindow, RequiredThread);
            }
            if(Stage == ApplicationEvent.Shutdown)
            {
                KeepRunning = false;
            }
        }

        public void CreateWindow()
        {
            this.CheckThreadExecution();

            WindowCreateInfo windowCI = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = (int)ScreenSize.X,
                WindowHeight = (int)ScreenSize.Y,
                WindowInitialState = WindowState.Normal,
                WindowTitle = "Intro",
            };

            SDL2Window = VeldridStartup.CreateWindow(ref windowCI);

            jobService.Enqueue(async () =>
            {
                //TODO: Cancellations
                while(KeepRunning)
                {
                    SDL2Window.PumpEvents();
                    await Task.Delay(TimeSpan.FromSeconds(1.0 / 60.0));
                }                
            },
            RequiredThread);
        }
    }
}
