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
        Dictionary<string, ICommandStage> Stages;

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

            //TODO: Schedule this stage

            return stage;
        }
    }
}
