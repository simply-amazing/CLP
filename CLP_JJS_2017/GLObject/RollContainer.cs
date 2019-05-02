using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CLPLib.GLObject
{
    class RollContainer : Cube
    {


        public RollContainer(float Length, float Width, float Thickness, float Height, float loadedHeight)
        {

            GL.Begin(PrimitiveType.Triangles);
            this.CreateCube(Length, Thickness, Width);

            Vector3 palletV3 = new Vector3(Length, Thickness, Width); // 0일때를 제외하고 공간만큼 좌표 이동

            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                GL.Color3(Color.BurlyWood);
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }

            this.ClearCube();
            GL.End();


            // 파렛트 외곽선(검정색) 
            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.LineLoop);
            this.CreateCube(Length, Thickness, Width);

            for (int ii = 0; ii < this.Indices.Length; ++ii)
            {
                GL.Color3(Color.Black);
                GL.Vertex3(this.Vertices[this.Indices[ii]]);
            }
            this.ClearCube();
            GL.End();


            GL.LineWidth(3.0f);
            GL.Begin(PrimitiveType.Lines);
            palletV3 = new Vector3(Length, 0, Width);
            for (int i = 0; i < 5; i++)
            {
                this.CreateUpDownLine(0, Height, Width / 4 * i);
                
                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }

                this.CreateLeftRightLine(0, Height / 4 * i, Width );

      

                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                this.CreateUpDownLine(Length, Height, Width / 4 * i);
                
                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }

                this.CreateLeftRightLine(Length, Height / 4 * i, Width);
                
                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                this.CreateUpDownLine(Length / 4 * i, Height, 0);
                
                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }

                this.CreateBackLeftRightLine(Length, Height / 4 * i, 0);
                
                for (int ii = 0; ii < this.Vertices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[ii]);
                }
            }

            this.ClearCube();
            GL.End();
        }


        private void CreateUpDownLine(float x, float y, float z)
        {
            Vertices = new Vector3[]
            {
                new Vector3(x == 0 ? x-2 : x+2, y, z == 0 ? z-2 : z+2),
                new Vector3(x == 0 ? x-2 : x+2, 0, z == 0 ? z-2 : z+2)
            };

        }

        private void CreateLeftRightLine(float x, float y, float z)
        {
            Vertices = new Vector3[]
            {
                new Vector3(x == 0 ? x-2 : x+2, y, z == 0 ? z-2 : z+2),
                new Vector3(x == 0 ? x-2 : x+2, y, z == 0 ? -2 : 2)
            };
        }

        private void CreateBackLeftRightLine(float x, float y, float z)
        {
            Vertices = new Vector3[]
            {
                new Vector3(x == 0 ? x-2 : x+2, y, z == 0 ? z-2 : z+2),
                new Vector3(x == 0 ? -2 : 2, y, z == 0 ? z-2 : z+2)
            };
        }
    }
}
