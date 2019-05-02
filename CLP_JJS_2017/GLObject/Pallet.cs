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
    class Pallet : Cube
    {


        public Pallet(float palletLength, float palletWidth, float palletThickness, float loadedHeight)
        {
            float space = 30;

            //파렛트 크기에 대한 원점 조정
            //this.setDefaultPosition(palletLength, palletWidth);

            //파렛트 그리기(위, 아래)
            for (int i = 0; i < 14; i++)
            {
                GL.Begin(PrimitiveType.Triangles);

                this.CreateCube(palletLength, palletThickness / 5, palletWidth / 7 - space);
                Vector3 palletV3;
                if (i < 7)
                {
                    palletV3 = new Vector3(palletLength, palletThickness, palletWidth - ((i % 7) * (palletWidth / 7)) - (i % 7 == 0 ? 0 : space)); // 0일때를 제외하고 공간만큼 좌표 이동
                }
                else
                {
                    palletV3 = new Vector3(palletLength, palletThickness / 5, palletWidth - ((i % 7) * (palletWidth / 7)) - (i % 7 == 0 ? 0 : space)); // 0일때를 제외하고 공간만큼 좌표 이동
                }
   

                for (int ii = 0; ii < this.Indices.Length; ++ii)
                {
                    GL.Color3(Color.BurlyWood);
                    GL.Vertex3(this.Vertices[this.Indices[ii]]);
                }
                this.ClearCube();
                GL.End();


                // 파렛트 외곽선(검정색) 
                GL.Begin(PrimitiveType.LineLoop);
                this.CreateCube(palletLength, palletThickness / 5, palletWidth / 7 - space);
           

                for (int ii = 0; ii < this.Indices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[this.Indices[ii]]);
                }
                this.ClearCube();
                GL.End();
            }

            //파렛트 그리기 기둥3개
            for (int i = 0; i < 3; i++)
            {
                GL.Begin(PrimitiveType.Triangles);

                this.CreateCube(space, palletThickness / 5 * 3, palletWidth);
                Vector3 palletV3;
                if (i < 2)
                {
                    palletV3 = new Vector3(palletLength / (i + 1), palletThickness / 5 * 4, palletWidth);
                }
                else
                {
                    palletV3 = new Vector3(space, palletThickness / 5 * 4, palletWidth);
                }


                for (int ii = 0; ii < this.Indices.Length; ++ii)
                {
                    GL.Color3(Color.BurlyWood);
                    GL.Vertex3(this.Vertices[this.Indices[ii]]);
                }
                this.ClearCube();
                GL.End();
                // 파렛트 외곽선(검정색) 
                GL.Begin(PrimitiveType.LineLoop);
                this.CreateCube(space, palletThickness / 5 * 3, palletWidth);
            
                for (int ii = 0; ii < this.Indices.Length; ++ii)
                {
                    GL.Color3(Color.Black);
                    GL.Vertex3(this.Vertices[this.Indices[ii]]);
                }
                this.ClearCube();
                GL.End();
            }
        }
    }
}
