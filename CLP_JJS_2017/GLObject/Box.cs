using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CLPLib.GLObject
{
    class Box : Cube
    {
        public Box()
        {

        }
        public Box(float boxLength, float boxWidth, float boxHeight, int loadedHeight)
        {

            // 밑면 사각형 그리기 
            GL.Begin(PrimitiveType.Triangles);
            this.CreateSquare(boxLength, boxWidth);
            Vector3 boxV3 = new Vector3(boxLength, 3f, boxWidth);

            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                GL.Color3(Color.RosyBrown);
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }
            this.ClearCube();
            GL.End();

            // 외곽선(검정색) 
            GL.Begin(PrimitiveType.LineLoop);
            this.CreateCube(boxLength, boxHeight, boxWidth);

            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                if (ii == 0)
                {
                    continue;
                }
                GL.Color3(Color.Black);
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }
            this.ClearCube();
            GL.End();
        }

        public void CreateSquare(float x, float z)
        {

            Vertices = new Vector3[]
            {
                new Vector3(0, 0, z),
                new Vector3(x, 0, z),
                new Vector3(0, 0, 0),
                new Vector3(x, 0, 0)
            };
            Indices = new int[]
           {
                // bottom face
                0, 1, 3, 3, 2, 0
           };
            //Vertices 리스트를 읽고, Min, Max 사이즈를 결정
        }
    }
}
