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
        }

        public double RunRate
        {
            get;
            set;
        }
        

        public IEnumerable<ICommandStage> RunAfter => null;

        List<ICommandSystem> Systems = new();

        public CommandStage(string name, CommandStageRunMode RunMode, double FixedRate = 0.0)
        {
            this.RunMode = RunMode;
            this.RunRate = FixedRate;
        }

        public void Execute()
        {
            foreach(var sys in Systems)
            {
                //Schedule this system
            }
        }

        void ICommandStage.AddSystem(ICommandSystem system)
        {
            Systems.Add(system);
        }
    }
}
