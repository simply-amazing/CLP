using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CLPLib.GLObject
{
    class Cylinder : Shape
    {
        // Number of vertices per circle is fixed at a number
        // Simplifies processing and increases speed
        const int nVertices = 80;

        // List of vertices that describe the circle
        //Vector3d[] Vertices = new Vector3d[2 * nVertices];

        // attached to property below

        public Cylinder(double conLength, double conWidth, string[] data, float loadedHeight) : base(2 * nVertices)
        {
            // Since Shapes does the work of setting up the vertices array, we can jump straight into calculating!

            // Run through and calculate the vertices relative to 
            // the position with r = Radius

            
            double radius = Convert.ToDouble(data[4]) / 2;
            double height = Convert.ToDouble(data[5]);
            double X = Convert.ToDouble(data[1]);
            double Y = Convert.ToDouble(data[2]);
            double Z = Convert.ToDouble(data[0]);
            Vector3d Position = new Vector3d(X, Y, Z);
            Vector3d ConSize = new Vector3d(conLength/2, 0, conWidth/2);
            SetPosition(Position, ConSize, radius, loadedHeight);
            Calculate(radius, height);
            _Color = ColorTranslator.FromOle(Int32.Parse(data[7]));    // 큐브마스터 엔진에서는 색상을 Int형으로 나오는 결과를 Color로 형변화
        }

        protected override void Calculate(double Radius, double Height)
        {
            double theta = 0, dtheta = MathHelper.TwoPi / nVertices;

            for (int i = 0; i < Vertices.Length; i += 2)
            {
                // Set back circle's info
                Vertices[i].X += Radius * Math.Cos(theta);
                Vertices[i].Z += Radius * Math.Sin(theta);

                // Set front circle's into
                Vertices[i + 1].X = Vertices[i].X;
                Vertices[i + 1].Z = Vertices[i].Z;
                // Add the height to the position to get
                // an identical circle, but forward some
                Vertices[i + 1].Y += Height;

                // Increment the angle measure
                theta += dtheta;
            }
        }

        /// <summary>
        /// Draw the cylinder as it's stored
        /// </summary>
        public override void Draw()
        {
            int i = 0; // premptive init of i because there's a couple loops
                       // in here that use it (but one after the other)
            GL.Color4(_Color);

            // Draw the rear circle (even indices)
            GL.Begin(PrimitiveType.Polygon);
            {
                for (i = 0; i < Vertices.Length; i += 2)
                {
                    GL.Vertex3(Vertices[i]);
                }
            }
            GL.End();

            // Draw the front circle (odd indices)
            GL.Begin(PrimitiveType.Polygon);
            {
                for (i = 1; i < Vertices.Length; i += 2)
                {
                    GL.Vertex3(Vertices[i]);
                }
            }
            GL.End();

            // Draw the connecting bits
            GL.Begin(PrimitiveType.TriangleStrip);
            {

                for (i = 0; i < Vertices.Length; i += 2)
                {
                    GL.Vertex3(Vertices[i]);
                    GL.Vertex3(Vertices[i + 1]);
                }
            }
            GL.End();

            // Draw lines along the sides of the thing to give it contours
            // and junk til i can get shading figured out
            if (true)
            {
                GL.Begin(PrimitiveType.Lines);
                {
                    GL.Color4(Color4.Black);
                    for (i = 0; i < Vertices.Length; i += 2)
                    {
                        GL.Vertex3(MoveOutwards(Vertices[i]));
                        GL.Vertex3(MoveOutwards(Vertices[i + 1]));
                    }
                }
                GL.End();
            }
            // If the user wants to draw outlines, do so
            if (true)
            {
                float lineSize = 1.5f;
                // 외곽선(검정색) 
                GL.LineWidth(lineSize);
                GL.Color4(Color4.Black);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    // Back circle
                    for (i = 0; i < Vertices.Length; i += 2)
                    {
                        GL.Vertex3(MoveOutwards(Vertices[i]));
                    }
                }
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                {
                    // Front circle
                    for (i = 1; i < Vertices.Length; i += 2)
                    {
                        GL.Vertex3(MoveOutwards(Vertices[i]));
                    }
                }
                GL.End();
            }
        }
    }
}
