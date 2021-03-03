using Ponos.API.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.RenderGraph.Veldrid.Graph
{
    class VeldredRenderGraph : IRenderGraph
    {
        public string Name
        {
            get;
            set;
        }
        public VeldredRenderGraph(string Name)
        {
            this.Name = Name;
        }

        public void AddRenderPass(IRenderPass pass)
        {
            throw new NotImplementedException();
        }

    }
}
