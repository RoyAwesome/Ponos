using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Math
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPos
    {
        public Vector3 Position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPosNorm
    {
        public Vector3 Position;
        public Vector3 Normal;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPosNormColor
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPosNormColorUv
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;
        public Vector2 UV;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPosNormColorUv2
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;
        public Vector2 UV;
        public Vector2 UV2;
    }
}
