using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using CLPLib;

namespace CLPLib.GLObject
{

    public class Cube
    {
        public Vector3[] Vertices;
        public int[] Indices;


        public Cube()
        {

        }


        public Cube(CLPLib.LoadedBox boxLoaded)
        {
            GL.Begin(PrimitiveType.Triangles);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(1.0f, 1.0f);

            if (boxLoaded.LoadedOrientation == 1)
                this.CreateCube(boxLoaded.Size.X-1f, boxLoaded.Size.Y-1f, boxLoaded.Size.Z-1f);
            else if (boxLoaded.LoadedOrientation == 2)
                this.CreateCube(boxLoaded.Size.Z-1f, boxLoaded.Size.Y - 1f, boxLoaded.Size.X - 1f);

            Vector3 v3 = new Vector3(boxLoaded.Coordinate.X, boxLoaded.Coordinate.Y, boxLoaded.Coordinate.Z);
            this.SetCubePosition(v3);
            
            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                GL.Color3(ColorTranslator.FromWin32(boxLoaded.BoxColor));
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }
            
            GL.Disable(EnableCap.PolygonOffsetFill);
            GL.End();

            float lineSize = 1f;
            GL.LineWidth(lineSize);
            GL.Begin(PrimitiveType.LineStrip);
            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                //GL.Color3(ColorTranslator.FromWin32(boxLoaded.BoxColor));
                GL.Color4(Color4.Black);
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }

            this.ClearCube();


            GL.End();
        }

        public struct ZoomFactor
        {
            public static float MinX;
            public static float MinY;
            public static float MinZ;

            public static float MaxX;
            public static float MaxY;
            public static float MaxZ;

            public static float MinSizeFactorToFit = 10000;

            public static float CenterX;
            public static float CenterY;
            public static float CenterZ;
        }

        public void CreateCube(float x, float y, float z)
        {

            Vertices = new Vector3[]
            {
                new Vector3(0, 0, z),
                new Vector3(x, 0, z),
                new Vector3(x, y, z),
                new Vector3(0, y, z),
                new Vector3(0, 0, 0),
                new Vector3(x, 0, 0),
                new Vector3(x, y, 0),
                new Vector3(0, y, 0)
            };


            Indices = new int[]
           {
                // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1
           };

            //Vertices 리스트를 읽고, Min, Max 사이즈를 결정
        }

        public void ClearCube()
        {
            Vertices = null;
        }


        //Cube의 위치를 pos만큼이동
        public void SetCubePosition(Vector3 pos)
        {
            for (int i = 0; i < Vertices.Length; ++i)
            {
                Vertices[i].X += pos.X;
                Vertices[i].Y += pos.Y;
                Vertices[i].Z += pos.Z;
            }
        }

    }
}
