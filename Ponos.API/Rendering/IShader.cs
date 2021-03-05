using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Rendering
{
    public interface IShader : IRenderResource
    {
        public ShaderStage PipelineStage
        {
            get;
        }

        public byte[] ShaderBytecode
        {
            get;
        }

        public ShaderFormat Format
        {
            get;
        }

        public string Entrypoint
        {
            get;
        }
    }

    public static class ShaderUtil
    {
        //Spir-V's magic number.  The first 4 bytes should be this, and if they are, we've loaded a spirv module.
        public const uint SPIRVMagic = 0x07230203;

        public static bool IsSPIRVShader(this IShader shader)
        {
            return shader.ShaderBytecode.Length > 4
                   && shader.ShaderBytecode[0] == 0x03
                   && shader.ShaderBytecode[1] == 0x02
                   && shader.ShaderBytecode[2] == 0x23
                   && shader.ShaderBytecode[3] == 0x07;
        }
    }

    public enum ShaderStage
    {
        Fragment,
        Vertex,
        TessellationControl,
        TessellationEvaluation,
        Geometry,
        Compute,
    }

    public enum ShaderFormat
    {
        SPIRV,
        GLSL,
        HLSL,
        CG,
    }
}
