using Autofac;
using NLog;
using Ponos.API;
using Ponos.API.Commands;
using Ponos.API.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Ponos
{
    class MockScheduler : IApplicationEventListener
    {
        class FixedRateStage
        {
            public double TimeRemaining;
            public ICommandStage Stage;
        }

        public bool ShouldLoop = true;

        BlockingCollection<ICommandStage> VariableStages = new();
        BlockingCollection<FixedRateStage> FixedRateStages = new();
        Channel<ICommandStage> ManualExecuteChannel = Channel.CreateUnbounded<ICommandStage>();

        Stopwatch stopwatch = new();

        Thread SingleThread;

        private readonly IComponentContext componentContext;
        private readonly ILogger logger;
        public MockScheduler(IComponentContext componentContext, ILogger logger)
        {
            this.componentContext = componentContext;
            this.logger = logger;
        }

        public void Start()
        {
            SingleThread = new Thread(RunLoop);
            SingleThread.Start();
        }

        public void ScheduleStage(ICommandStage stage)
        {
            
            if(stage.RunMode == CommandStageRunMode.Variable)
            {
                lock(VariableStages)
                {
                    VariableStages.Add(stage);
                }
               
            }

            if(stage.RunMode == CommandStageRunMode.Fixed)
            {
                lock(FixedRateStages)
                {
                    FixedRateStages.Add(new FixedRateStage()
                    {
                        TimeRemaining = 0,
                        Stage = stage,
                    });
                }                
            }

            if(stage.RunMode == CommandStageRunMode.Custom)
            {
                ManualExecuteChannel.Writer.TryWrite(stage);
            }
        }       
      

        public void RunLoop()
        {
            logger.Info("Starting Loop");
            stopwatch.Start();
            while(ShouldLoop)
            {
                long FrameStart = stopwatch.ElapsedTicks;
                while(ManualExecuteChannel.Reader.TryRead(out ICommandStage ManualStage))
                {
                    logger.Info("Executing stage {0}", ManualStage.GetType().ToString());

                    ManualStage.Execute();
                }
                              

                foreach (var stage in VariableStages)
                {
                    stage.Execute();
                }

                long FrameDelta = stopwatch.ElapsedTicks - FrameStart;
                double FrameDeltaMs = (double)(FrameDelta) / (double)TimeSpan.TicksPerMillisecond;

                foreach(var frs in FixedRateStages)
                {
                    frs.TimeRemaining = frs.TimeRemaining - FrameDeltaMs;
                  
                    if(frs.TimeRemaining <= 0)
                    {
                        frs.Stage.Execute();
                        frs.TimeRemaining = frs.Stage.RunRate;
                    }

                }

                Thread.Sleep(1);
            }
            logger.Warn("Terminating Update Loop");
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
            if(Stage == ApplicationEvent.Begin)
            {
                var CommandBuilder = componentContext.Resolve<ICommandBuilder>();
                CommandBuilder.InVariableStage(StageNames.Default)
                    .InFixedRateStage(StageNames.Fixed_30Hz, 1.0 / 30.0)
                    .InFixedRateStage(StageNames.Fixed_60Hz, 1.0 / 60.0)
                    .InFixedRateStage(StageNames.Fixed_120Hz, 1.0 / 120.0);
            }
        }
    }
}
