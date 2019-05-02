using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    public class Coordinate
    {
        private float _x;
        private float _y;
        private float _z;

        public Coordinate()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Coordinate(float x, float y, float z)
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }


        /// <summary>
        /// X:WIDTH
        /// </summary>
        public float X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Y:LENGTH
        /// </summary>
        public float Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// Z:HEIGHT
        /// </summary>
        public float Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }
        }
    }
}
