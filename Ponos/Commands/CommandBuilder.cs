using Ponos.API;
using Ponos.API.Commands;
using Ponos.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.Commands
{
    public class CommandBuilder : ICommandBuilder, IApplicationEventListener
    {
        Dictionary<string, ICommandStage> Stages = new();

        private readonly MockScheduler mockScheduler;
        public CommandBuilder(MockScheduler scheduler)
        {
            mockScheduler = scheduler;
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
           
        }
      

        public ICommandStage GetStageByName(string name)
        {
            if(Stages.ContainsKey(name))
            {
                return Stages[name];
            }
            return null;
        }

        public void ExecuteStage(ICommandStage stage)
        {
            stage.Execute();
        }
        private ICommandStage GetOrCreateStageInternal(string name)
        {
            if (Stages.ContainsKey(name))
            {
                return Stages[name];
            }

            CommandStage stage = new(name, this);
            Stages[name] = stage;

            return stage;
        }

        ICommandStage ICommandBuilder.CreateStage(string name)
        {
            return GetOrCreateStageInternal(name);
        }

        ICommandStage ICommandBuilder.ScheduleStage(string name, CommandStageRunMode RunMode, double fixedRate)
        {
            CommandStage stage = GetOrCreateStageInternal(name) as CommandStage;

            stage.RunMode = RunMode;
            stage.RunRate = fixedRate;

            //If it's a custom schedule, then someone else will schedule us
            //Fixed rate and variable rate though, they need to be scheduled right away           
            mockScheduler.ScheduleStage(stage);
           
            return stage;
        }

        void ICommandBuilder.ScheduleSystem(ICommandSystem system)
        {
            Schedule(system);
        }

        public void Schedule(ICommandSystem system)
        {
            system.Run();
        }
    }
}
