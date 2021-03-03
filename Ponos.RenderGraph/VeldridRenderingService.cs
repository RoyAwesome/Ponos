using Ponos.API.Rendering;
using Ponos.API.Window;
using Ponos.RenderGraph.Veldrid.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeldridNative = Veldrid;
using GraphicsDevice = Veldrid.GraphicsDevice;

namespace Ponos.RenderGraph.Veldrid
{
    class VeldridRenderingService : IRenderingService
    {
        IWindowService WindowService;

        GraphicsDevice GraphicsDevice;

        public VeldridRenderingService(IWindowService windowService)
        {
            WindowService = windowService;
            (windowService as VeldridWindowService).OnWindowCreated += VeldridRenderingService_OnWindowCreated;
        }

        private void VeldridRenderingService_OnWindowCreated()
        {
            VeldridNative.GraphicsDeviceOptions options = new()
            {
                Debug = true,
                PreferStandardClipSpaceYDirection = true,
            };

            GraphicsDevice = VeldridNative.StartupUtilities.VeldridStartup.CreateVulkanGraphicsDevice(options, (WindowService as VeldridWindowService).SDL2Window);
        }

        public IRenderGraph CreateRenderGraph(string Name)
        {
            throw new NotImplementedException();
        }

        public IRenderPass CreateRenderPass(string Name)
        {
            throw new NotImplementedException();
        }

        public IShader CreateShader(string Name)
        {
            throw new NotImplementedException();
        }

        IRenderBuffer<T> IRenderingService.CreateRenderBuffer<T>(string name, BufferUsage usage) where T : struct
        {
            return new VeldredRenderBuffer<T>(GraphicsDevice, usage);
        }

        ITexture IRenderingService.CreateTexture(string Name)
        {
            throw new NotImplementedException();
        }
    }
}
