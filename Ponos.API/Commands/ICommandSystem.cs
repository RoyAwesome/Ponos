using Ponos.API.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Commands
{
    public interface ICommandSystem
    {
        public void Run();
    }

    public interface IECSQuerySystem : ICommandSystem
    {
        public EntityQuery Query
        {
            get;
        }
    }
}
