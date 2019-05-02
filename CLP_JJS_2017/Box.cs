using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    public class Box
    {
        private string _Name;
        private Size _size;
        private int _boxColor;
        private double _weight;
        private string _WEIGHT_RANK_GROUP;

        public string WEIGHT_RANK_GROUP
        {
            get
            {
                return _WEIGHT_RANK_GROUP;
            }

            set
            {
                _WEIGHT_RANK_GROUP = value;
            }
        }


        public Box()
        {
            _size = new Size(0, 0, 0);
        }



        internal Size Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        public int BoxColor
        {
            get
            {
                return _boxColor;
            }

            set
            {
                _boxColor = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        public double Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }
    }
}
