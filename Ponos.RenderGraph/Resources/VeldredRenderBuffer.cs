using Ponos.API.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;
using BufferUsage = Veldrid.BufferUsage;
using PBufferUsage = Ponos.API.Rendering.BufferUsage;

namespace Ponos.RenderGraph.Veldrid.Resources
{
   

    class VeldredRenderBuffer<T> : IRenderBuffer<T> where T : struct
    {
        

        public string Name
        {
            get;
            set;
        }

        public GraphicsDevice Device
        {
            get;
            set;
        }

        public PBufferUsage Usage
        {
            get;
            private set;
        }

        public bool Dynamic => true;

        public DeviceBuffer Buffer = null;


        public VeldredRenderBuffer(GraphicsDevice GraphicsDevice, PBufferUsage Usage)
        {
            this.Device = GraphicsDevice;
            this.Usage = Usage;
        }

        ~VeldredRenderBuffer()
        {
            Buffer?.Dispose();
        }

        private T[] CachedData;

        public void CopyToGPU()
        {
            if(CachedData == null)
            {
                CachedData = new T[] { };
            }

            if(Buffer == null || CachedData.Length > Buffer.SizeInBytes)
            {
                Buffer?.Dispose();
                BufferUsage usage = Usage.ToVeldrid();
                usage |= Dynamic ? BufferUsage.Dynamic : 0;

                Buffer = Device.ResourceFactory.CreateBuffer(new BufferDescription((uint)CachedData.Length, usage));
            }

            Device.UpdateBuffer(Buffer, 0, CachedData);
        }

        public void Update(T[] Data)
        {
            CachedData = Data;
        }
    }

    static class BufferUsageExt
    {
        public static BufferUsage ToVeldrid(this PBufferUsage usage)
        {
            return usage switch {
                PBufferUsage.VertexBuffer => BufferUsage.VertexBuffer,
                PBufferUsage.IndexBuffer => BufferUsage.IndexBuffer,
                PBufferUsage.Uniform => BufferUsage.UniformBuffer,
                _ => throw new InvalidOperationException("Unable to convert buffer usage"),
            };

        }
    }
}
