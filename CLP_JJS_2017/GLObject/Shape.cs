using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CLPLib.GLObject
{
    abstract class Shape
    {
        protected Vector3d[] Vertices;
        protected Color4 _Color = Color4.DarkSeaGreen;
        public Color4 Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }
        /// <summary>
        /// Base initializer for all shapes
        /// </summary>
        /// <param name="vertices">The number of vertices that make up the shape</param>
        /// <param name="Position">The zero position of the vertices (these are shifted around with Calculate())</param>
        public Shape(int vertices)
        {
            this.Vertices = new Vector3d[vertices+2];
        }

        public abstract void Draw();
        protected abstract void Calculate(double width, double height);
        protected Vector3d MoveOutwards(Vector3d ToMove)
        {
            try
            {
                ToMove.Y += Math.Sign(ToMove.Y) * 0.001;
                ToMove.Z += Math.Sign(ToMove.Z) * 0.001;
            }
            // Breaks on 1/x for some reason? gives ([decimal], NaN, Infinity) in ToMove
            catch (ArithmeticException e)
            {
                Console.WriteLine("Error in MoveOutwards (probably because 1/x or smth)");
            }

            return ToMove;
        }

        // 박스용 좌표
        protected void SetPosition(Vector3d pos, Vector3d conSize, double radius, float loadedHeight)
        {
            for (int i = 0; i < Vertices.Length; ++i)
            {
                Vertices[i].X += pos.X + radius;
                Vertices[i].Y += pos.Y + 0.5f;
                Vertices[i].Z += pos.Z + radius;

                Vertices[i].X -= conSize.X;
                Vertices[i].Z -= conSize.Z;

                Vertices[i].Y -= loadedHeight / 2.0f;
            }
        }

        //public void SetPosition(Vector3d pos, Vector3d size, int loadedHeight)
        //{
        //    for (int i = 0; i < Vertices.Length; ++i)
        //    {
        //        Vertices[i].X += pos.X + size.X / 2;
        //        Vertices[i].Y += pos.Y;
        //        Vertices[i].Z += pos.Z + size.X / 2;

        //        Vertices[i].X -= PalletLength;
        //        Vertices[i].Z -= PalletWidth;

        //        Vertices[i].Y -= loadedHeight / 2.0f;
        //    }
        //}
    }
}
