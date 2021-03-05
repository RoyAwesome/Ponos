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
using Ponos.API.Commands;
using Veldrid;
using Ponos.API;

namespace Ponos.RenderGraph.Veldrid
{
    class VeldridRenderingService : IRenderingService
    {
        IWindowService WindowService;

        GraphicsDevice GraphicsDevice;
        ICommandBuilder commandBuilder;
        public VeldridRenderingService(IWindowService windowService, ICommandBuilder commandBuilder)
        {
            WindowService = windowService;
            (windowService as VeldridWindowService).OnWindowCreated += VeldridRenderingService_OnWindowCreated;
            this.commandBuilder = commandBuilder;
        }

        private void VeldridRenderingService_OnWindowCreated()
        {
            VeldridNative.GraphicsDeviceOptions options = new()
            {
                Debug = true,
                PreferStandardClipSpaceYDirection = true,
            };

            GraphicsDevice = VeldridNative.StartupUtilities.VeldridStartup.CreateVulkanGraphicsDevice(options, (WindowService as VeldridWindowService).SDL2Window);

            commandBuilder.InVariableStage(StageNames.Default, (stage) => stage.AddSystem(new ClearScreenTestCommand() { RenderingService = this }));
        }

        class ClearScreenTestCommand : ICommandSystem
        {
            public VeldridRenderingService RenderingService
            {
                get;
                set;
            }

            VeldridNative.CommandList cl;

            public void Run()
            {
                if(cl == null)
                {
                    cl = RenderingService.GraphicsDevice.ResourceFactory.CreateCommandList();

                }
                cl.Begin();
                cl.SetFramebuffer(RenderingService.GraphicsDevice.SwapchainFramebuffer);
                cl.ClearColorTarget(0, RgbaFloat.Orange);
                cl.End();

                RenderingService.GraphicsDevice.SubmitCommands(cl);

                RenderingService.GraphicsDevice.WaitForIdle();
                RenderingService.GraphicsDevice.SwapBuffers();
                RenderingService.GraphicsDevice.WaitForIdle();
            }
        }


        public IRenderGraph CreateRenderGraph(string Name)
        {
            throw new NotImplementedException();
        }

        public IRenderPass CreateRenderPass(string Name)
        {
            throw new NotImplementedException();
        }
              
        IRenderBuffer<T> IRenderingService.CreateRenderBuffer<T>(string name, API.Rendering.BufferUsage usage) where T : struct
        {
            return new VeldredRenderBuffer<T>(GraphicsDevice, usage);
        }

        ITexture IRenderingService.CreateTexture(string Name)
        {
            throw new NotImplementedException();
        }

        public IShader CreateShader(string Name, ShaderStage shaderStage, ShaderFormat Format, byte[] byteCode)
        {
            return new VeldridShader(GraphicsDevice, Name, shaderStage, byteCode, Format);
        }

        public IShader CompileShader(string Name, ShaderStage stage, ShaderFormat fromFormat, ShaderFormat toFormat, byte[] code)
        {
            //If we don't need to compile the shader from one format to another, just return that shader
            if(fromFormat == toFormat)
            {
                return CreateShader(Name, stage, toFormat, code);
            }

            if(fromFormat == ShaderFormat.GLSL && toFormat == ShaderFormat.SPIRV)
            {
                var glslCompile = VeldridNative.SPIRV.SpirvCompilation.CompileGlslToSpirv(
                    Encoding.UTF8.GetString(code),
                    Name,
                    stage.ToVeldridStage(),
                    new VeldridNative.SPIRV.GlslCompileOptions()
                    { 
                        Debug = true,
                    });

                if(glslCompile.SpirvBytes == null)
                {
                    throw new Exception("GLSL Compile Failed!");
                }

                return CreateShader(Name, stage, toFormat, glslCompile.SpirvBytes);
            }
           
            throw new InvalidOperationException(string.Format("Veldrid Renderer does not support cross compiling {0} to {1}", fromFormat, toFormat));
        }

       
    }
}
