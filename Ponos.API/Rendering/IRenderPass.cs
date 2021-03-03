using Ponos.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Rendering
{
    public interface IRenderPass : INamed
    {    

        public void WithInputResource(IRenderResource renderResource);
        public void WithOutputResource(IRenderResource renderResource);


    }
}
