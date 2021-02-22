using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API
{
    public enum ApplicationEvent
    {
        Startup,
        RendererInit,
        Begin,
        Shutdown,
        Stopped,
    }
}
