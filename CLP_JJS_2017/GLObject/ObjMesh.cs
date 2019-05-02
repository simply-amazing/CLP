using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CLPLib.GLObject
{
    class ObjMesh
    {
        public bool is_loaded = false;
        public ObjMesh(string fileName)
        {
            is_loaded = ObjMeshLoader.Load(this, fileName);
        }

        public ObjVertex[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
        ObjVertex[] vertices;

        public ObjTriangle[] Triangles
        {
            get { return triangles; }
            set { triangles = value; }
        }
        ObjTriangle[] triangles;

        public ObjQuad[] Quads
        {
            get { return quads; }
            set { quads = value; }
        }
        ObjQuad[] quads;

        int verticesBufferId = 0;
        int trianglesBufferId = 0;
        int quadsBufferId = 0;

        public void Prepare()
        {
            if (!is_loaded) return;
            if (verticesBufferId == 0)
            {
                GL.GenBuffers(1, out verticesBufferId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(ObjVertex))), vertices, BufferUsageHint.StaticDraw);
            }

            if (trianglesBufferId == 0)
            {
                GL.GenBuffers(1, out trianglesBufferId);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);
            }

            if (quadsBufferId == 0)
            {
                GL.GenBuffers(1, out quadsBufferId);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
            }
        }

        public void Render()
        {
            if (!is_loaded) return;
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(ObjVertex)), IntPtr.Zero);
            if (triangles.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
                GL.DrawElements(BeginMode.Triangles, triangles.Length * 3, DrawElementsType.UnsignedInt, 0);
            }

            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
                GL.DrawElements(BeginMode.Quads, quads.Length * 4, DrawElementsType.UnsignedInt, 0);
            }

            GL.PopClientAttrib();
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct ObjVertex
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ObjTriangle
        {
            public UInt32 Index0;
            public UInt32 Index1;
            public UInt32 Index2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ObjQuad
        {
            public UInt32 Index0;
            public UInt32 Index1;
            public UInt32 Index2;
            public UInt32 Index3;
        }

    }
}
