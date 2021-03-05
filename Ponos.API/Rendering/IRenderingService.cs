using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Rendering
{
    public interface IRenderingService
    {

        public IRenderGraph CreateRenderGraph(string Name);

        public IRenderPass CreateRenderPass(string Name);

        public ITexture CreateTexture(string Name);

        public IShader CreateShader(string Name, ShaderStage shaderStage, ShaderFormat Format, byte[] byteCode);

        //TODO: Async this
        public IShader CompileShader(string Name, ShaderStage stage, ShaderFormat fromFormat, ShaderFormat toFormat, byte[] code);

        protected internal IRenderBuffer<T> CreateRenderBuffer<T>(string name, BufferUsage usage) where T : struct;
    }

    public static class RenderingServiceExt
    {
        public static IRenderBuffer<T> CreateVertexBuffer<T>(this IRenderingService renderingService, string name) where T : struct
        {
            return renderingService.CreateRenderBuffer<T>(name, BufferUsage.VertexBuffer);
        }

        public static IRenderBuffer<T> CreateIndexBuffer<T>(this IRenderingService renderingService, string name) where T : struct
        {
            return renderingService.CreateRenderBuffer<T>(name, BufferUsage.IndexBuffer);
        }

        public static IRenderBuffer<T> CreateUniformBuffer<T>(this IRenderingService renderingService, string name) where T : struct
        {
            return renderingService.CreateRenderBuffer<T>(name, BufferUsage.Uniform);
        }

        
    }
}
