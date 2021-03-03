using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Rendering
{
    public enum BufferUsage
    {
        VertexBuffer,
        IndexBuffer,
        Uniform,
    }
    public interface IRenderBuffer<T> : IRenderResource where T : struct
    {
        public BufferUsage Usage
        {
            get;
        }


        public bool Dynamic
        {
            get;
        }

        public void Update(T[] Data);
    }
}
