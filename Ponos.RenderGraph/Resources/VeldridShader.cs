using Ponos.API.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeldridLib = Veldrid;

namespace Ponos.RenderGraph.Veldrid.Resources
{
    class VeldridShader : IShader
    {
        public string Name
        {
            get;
            set;
        }

        public ShaderStage PipelineStage
        {
            get;
            private set;
        }

        public byte[] ShaderBytecode
        {
            get;
            set;
        }

        public ShaderFormat Format
        {
            get;
            set;
        }
               

        public string Entrypoint
        {
            get;
            set;
        }

        VeldridLib.GraphicsDevice graphicsDevice;

        public VeldridShader(VeldridLib.GraphicsDevice graphicsDevice, string Name, ShaderStage PipelineStage, byte[] Bytecode, ShaderFormat Format)
        {
            this.Name = Name;
            this.PipelineStage = PipelineStage;
            this.ShaderBytecode = Bytecode;
            this.Format = Format;
            this.graphicsDevice = graphicsDevice;
        }

       

        VeldridLib.Shader _shader;

        public VeldridLib.Shader Shader
        {
            get
            {
                if (_shader == null)
                {
                    _shader = graphicsDevice.ResourceFactory.CreateShader(new VeldridLib.ShaderDescription(PipelineStage.ToVeldridStage(), ShaderBytecode, Entrypoint));
                }
                return _shader;
            }           
        }
      
    }

    public static class VeldridShaderExt
    {
        public static VeldridLib.ShaderStages ToVeldridStage(this ShaderStage stage)
        {
            return stage switch
            {
                ShaderStage.Vertex                  => VeldridLib.ShaderStages.Vertex,
                ShaderStage.Fragment                => VeldridLib.ShaderStages.Fragment,
                ShaderStage.Geometry                => VeldridLib.ShaderStages.Geometry,
                ShaderStage.TessellationControl     => VeldridLib.ShaderStages.TessellationControl,
                ShaderStage.TessellationEvaluation  => VeldridLib.ShaderStages.TessellationEvaluation,
                ShaderStage.Compute                 => VeldridLib.ShaderStages.Compute,
                _                                   => VeldridLib.ShaderStages.None,
            };
        }

        
    }
}
