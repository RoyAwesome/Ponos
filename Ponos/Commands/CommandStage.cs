using Ponos.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.Commands
{
    public class CommandStage : ICommandStage
    {
        public CommandStageRunMode RunMode
        {
            get;
            set;
        } = CommandStageRunMode.Custom;

        public double RunRate
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }

        public IList<Action> RunAfter { get; } 
            = new List<Action>();

        List<ICommandSystem> Systems = new();
        List<ICommandSystem> RunOnceSystems = new();


        CommandBuilder commandBuilder;

        public CommandStage(string name, CommandBuilder commandBuilder)
        {
            this.Name = name;
            this.commandBuilder = commandBuilder;
        }

        public void Execute()
        {
            foreach(var rosys in RunOnceSystems)
            {
                commandBuilder.Schedule(rosys);
            }
            RunOnceSystems.Clear();

            foreach(var sys in Systems)
            {
                commandBuilder.Schedule(sys);
            }
        }

        void ICommandStage.AddSystem(ICommandSystem system, bool RunOnce = false)
        {
            if(RunOnce)
            {
                RunOnceSystems.Add(system);
            }
            else
            {
                Systems.Add(system);
            }
           
        }

        public override string ToString()
        {
            return string.Format("{0} - RunMode: {1}", Name, RunMode.ToString());
        }
    }
}
