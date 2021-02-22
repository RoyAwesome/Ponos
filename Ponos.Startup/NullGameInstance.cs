using Microsoft.Extensions.Hosting;
using NLog;
using Ponos.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.Startup
{
    internal class NullGameInstance : IGameInstance
    {
        public string Version => "1.0.0";

        public string Name => "Null Game Instance";

        private readonly ILogger Logger;

        public NullGameInstance(ILogger logger)
        {
            Logger = logger;
            Logger.Warn("Instanced");
        }

    }
}
