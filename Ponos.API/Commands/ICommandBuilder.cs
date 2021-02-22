using Ponos.API.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Commands
{
    public interface ICommandBuilder
    {
        protected internal ICommandStage CreateStage(string name, CommandStageRunMode RunMode, double fixedRate = 0.0f);

        public ICommandStage GetStageByName(string name);
    }

    public static class CommandExt
    {
        public static ICommandBuilder AddSystemToStage(this ICommandBuilder commandBuilder, string name, ICommandSystem system)
        {
            ICommandStage stage = commandBuilder.GetStageByName(name);

            if(stage == null)
            {
                throw new ArgumentException("stage does not exist", nameof(name));
            }

            stage.AddSystem(system);

            return commandBuilder;
        }

        private static ICommandStage GetOrCreateStage(this ICommandBuilder commandBuilder, string name, Func<ICommandBuilder, string, ICommandStage> stageCreator)
        {
            var stage = commandBuilder.GetStageByName(name);
            return stage ?? stageCreator(commandBuilder, name);
        }
               

        public static ICommandBuilder InVariableStage(this ICommandBuilder commandBuilder, string name, Action<ICommandStage> runAfter = null)
        {
            
            var stage = commandBuilder.GetOrCreateStage(name, (cb, name) => cb.CreateStage(name, CommandStageRunMode.Variable));
            if(runAfter != null)
            {
                runAfter(stage);
            }          
            return commandBuilder;
        }

        public static ICommandBuilder InFixedRateStage(this ICommandBuilder commandBuilder, string name, double fixedRate, Action<ICommandStage> runAfter = null)
        {
            if(fixedRate <= 0)
            {
                throw new ArgumentException("Must be greater than 0", nameof(fixedRate));
            }

            var stage = commandBuilder.GetOrCreateStage(name, (cb, name) => cb.CreateStage(name, CommandStageRunMode.Fixed, fixedRate));

            if (runAfter != null)
            {
                runAfter(stage);
            }
            return commandBuilder;
        }

        public static ICommandBuilder InManualStage(this ICommandBuilder commandBuilder, string name, Action<ICommandStage> runAfter = null)
        {
            var stage = commandBuilder.GetOrCreateStage(name, (cb, name) => cb.CreateStage(name, CommandStageRunMode.Custom));
            if (runAfter != null)
            {
                runAfter(stage);
            }
            return commandBuilder;
        }

        public static void ExecuteManualStage(this ICommandBuilder commandBuilder, string name)
        {
            var stage = commandBuilder.GetStageByName(name);

            if (stage != null && stage.RunMode == CommandStageRunMode.Custom)
            {
                stage.Execute();
            }
        }
        
    }
}
