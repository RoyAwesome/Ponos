using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Commands
{
    public enum CommandStageRunMode
    {
        /// <summary>
        /// This stage runs immediately after the last run of this stage completes.  
        /// </summary>
        Variable,
        /// <summary>
        /// This stage will run at a fixed rate, indicated by RunRate
        /// </summary>
        Fixed,
        /// <summary>
        /// This stage will run at a custom time, dictated by the ICommandBuilder.RunNow() function
        /// </summary>
        Custom,
    }
    public interface ICommandStage
    {
        public CommandStageRunMode RunMode
        {
            get;
        }

        /// <summary>
        /// Rate, in seconds, to run this stage if RunMode is fixed
        /// </summary>
        public double RunRate
        {
            get;
        }
        
        /// <summary>
        /// Which stages to run this after 
        /// </summary>
        public IEnumerable<ICommandStage> RunAfter
        {
            get;
        }

        protected internal void AddSystem(ICommandSystem system);

        public void Execute();

    }

    public static class CommandStageExt
    {
        public static ICommandStage AddSystem(this ICommandStage stage, ICommandSystem system)
        {
            if(system == null)
            {
                throw new ArgumentNullException(nameof(system));
            }

            stage.AddSystem(system);
            return stage;
        }
    }
}
