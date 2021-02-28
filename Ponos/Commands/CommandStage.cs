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

        public CommandStage(string name)
        {
            this.Name = name;
        }

        public void Execute()
        {
            foreach(var sys in Systems)
            {
                //TODO: Schedule these
                sys.Run();
            }
        }

        void ICommandStage.AddSystem(ICommandSystem system)
        {
            Systems.Add(system);
        }

        public override string ToString()
        {
            return string.Format("{0} - RunMode: {1}", Name, RunMode.ToString());
        }
    }
}
