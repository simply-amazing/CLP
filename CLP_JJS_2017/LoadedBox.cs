using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    public class LoadedBox : BoxToLoad
    {
        private readonly int _loadedCount = 1;
        private int _loadedOrientation;
        private Coordinate _coordinate;

        public LoadedBox()
        {

        }

        public LoadedBox(BoxToLoad boxToLoad)
        {
            this.Coordinate = new Coordinate();
            this.CountToLoad = 1;
            this.BoxColor = boxToLoad.BoxColor;
            this.LoadOrientation = boxToLoad.LoadOrientation;
            this.Name = boxToLoad.Name;
            this.Size = new Size(boxToLoad.Size.X, boxToLoad.Size.Y, boxToLoad.Size.Z);
            this.Weight = boxToLoad.Weight;
        }

        public int LoadedCount
        {
            get
            {
                return _loadedCount;
            }
        }

        public int LoadedOrientation
        {
            get
            {
                return _loadedOrientation;
            }

            set
            {
                _loadedOrientation = value;
            }
        }

        public Coordinate Coordinate
        {
            get
            {
                return _coordinate;
            }

            set
            {
                _coordinate = value;
            }
        }

        public Coordinate CoordinateEnd
        {
            get
            {
                Coordinate coordinateEnd = new Coordinate();
                if (_loadedOrientation == 1)
                {
                    coordinateEnd.X = _coordinate.X + base.Size.X;
                    coordinateEnd.Y = _coordinate.Y + base.Size.Y;
                    coordinateEnd.Z = _coordinate.Z + base.Size.Z;
                }
                else if (_loadedOrientation == 2)
                {
                    coordinateEnd.X = _coordinate.X + base.Size.Z;
                    coordinateEnd.Y = _coordinate.Y + base.Size.Y;
                    coordinateEnd.Z = _coordinate.Z + base.Size.X;
                }

                return coordinateEnd;
            }

        }
    }
}
