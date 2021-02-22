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
    class CommandBuilder : ICommandBuilder, IApplicationEventListener
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

        ICommandStage ICommandBuilder.CreateStage(string name, CommandStageRunMode RunMode, double fixedRate)
        {
            if(Stages.ContainsKey(name))
            {
                return Stages[name];
            }

            CommandStage stage = new(name, RunMode, fixedRate);
            Stages[name] = stage;

            //If it's a custom schedule, then someone else will schedule us
            //Fixed rate and variable rate though, they need to be scheduled right away
            if(RunMode != CommandStageRunMode.Custom)
            {
                mockScheduler.ScheduleStage(stage);
            }
           

            return stage;
        }
    }
}
