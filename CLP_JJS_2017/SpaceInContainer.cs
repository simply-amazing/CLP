using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    class SpaceInContainer : Box
    {
        private LoadedBox _loadedBox;
        private Coordinate _coordinate;
        

        public SpaceInContainer()
        {
            _loadedBox = null;
            _coordinate = new Coordinate();
        }

        /// <summary>
        /// 적재할 수 있는 공간의 공간의 시작위치
        /// </summary>
        public Coordinate PositionCoord
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

        internal LoadedBox LoadedBox
        {
            get
            {
                return _loadedBox;
            }

            set
            {
                _loadedBox = value;
            }
        }

        public SpaceInContainer GetSubSpaceSide()
        {
            if (_loadedBox == null)
                return null;

            SpaceInContainer subSpaceSide = new SpaceInContainer();
            subSpaceSide.PositionCoord.X = _coordinate.X;
            subSpaceSide.PositionCoord.Y = _coordinate.Y;
            subSpaceSide.PositionCoord.Z = _loadedBox.CoordinateEnd.Z;

            subSpaceSide.Size.X = _loadedBox.CoordinateEnd.X - _loadedBox.Coordinate.X;
            subSpaceSide.Size.Y = this.Size.Y;
            subSpaceSide.Size.Z = this.Size.Z - _loadedBox.CoordinateEnd.Z;
            
            return subSpaceSide;
        }

        public SpaceInContainer GetSubSpaceTop()
        {
            if (_loadedBox == null)
                return null;

            SpaceInContainer subSpaceTop = new SpaceInContainer();
            subSpaceTop.PositionCoord.X = _coordinate.X;
            subSpaceTop.PositionCoord.Y = _loadedBox.CoordinateEnd.Y;
            subSpaceTop.PositionCoord.Z = _coordinate.Z;

            subSpaceTop.Size.X = _loadedBox.CoordinateEnd.X - _loadedBox.Coordinate.X;
            subSpaceTop.Size.Y = this.Size.Y - (_loadedBox.Size.Y);
            subSpaceTop.Size.Z = this.Size.Z - (_loadedBox.CoordinateEnd.Z - _loadedBox.Coordinate.Z);

            return subSpaceTop;
        }

        public SpaceInContainer GetSubSpaceFront()
        {
            if (_loadedBox == null)
                return null;

            SpaceInContainer subSpaceFront = new SpaceInContainer();
            subSpaceFront.PositionCoord.X = _loadedBox.CoordinateEnd.X;
            subSpaceFront.PositionCoord.Y = _loadedBox.CoordinateEnd.Y;
            subSpaceFront.PositionCoord.Z = base.Size.Z;

            subSpaceFront.Size.X = base.Size.X - (_loadedBox.CoordinateEnd.X - _loadedBox.Coordinate.X);
            subSpaceFront.Size.Y = this.Size.Y;
            subSpaceFront.Size.Z = this.Size.Z;

            return subSpaceFront;
        }

    }
}
