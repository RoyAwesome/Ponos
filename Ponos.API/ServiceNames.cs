using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API
{
    public static class ServiceNames
    {
        public const string EngineServiceName = "Engine";
        public const string GameInstanceServiceName = "GameInstance";
        public const string RenderingServiceName = "Renderer";
        public const string WindowingServiceName = "Windowing";
        public const string ThreadingServiceName = "Windowing";        
    }

    public static class StageNames
    {
        public const string Startup = "Startup";
        public const string RendererInit = "RendererInit";
        public const string Shutdown = "Shutdown";
        public const string Fixed_30Hz = "30HZ";
        public const string Fixed_60Hz = "60HZ";
        public const string Fixed_120Hz = "120HZ";
    }

}
