using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    public class Size : Coordinate
    {

        /// <summary>
        /// Size(0,0,0)
        /// </summary>
        public Size()
        {
            base.X = 0;
            base.Y = 0;
            base.Z = 0;
        }

        public Size(float x, float y, float z)
        {
            base.X = x; 
            base.Y = y;
            base.Z = z;
        }

        public Size(Size size)
        {
            base.X = size.X;
            base.Y = size.Y;
            base.Z = size.Z;
        }



    }
}
