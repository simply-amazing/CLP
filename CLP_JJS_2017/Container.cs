using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace CLPLib
{
    class Container
    {
        public Container()
        {
            _area1 = new Size();
            _area2 = new Size();
            _area3 = new Size();
            _area4 = new Size();
        }
        private Size _size;
        private double _tareWeight;
        private double _maxWeightIncludeTareWeight;
        private Coordinate _targetCenterOfGravity;
        
        internal Size Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
                CalculateAreaPositionBase(); //size가 설정되면 면적 정보를 재설정한다.
            }
        }


        //area 1이 컨테이너 원점에서 가장 가까운 점이고, 위에서 내려다 볼 때 시계방향으로 회전하면서 좌표를 표현함
        private Size _area1;
        private Size _area2;
        private Size _area3;
        private Size _area4;

        private void CalculateAreaPositionBase()
        {
            _area1.X = _size.X / 3f;
            _area1.Z = _size.Z / 3f;
            _area2.X = _size.X / 3f * 2f;
            _area2.Z = _size.Z / 3f;
            _area3.X = _size.X / 3f * 2f;
            _area3.Z = _size.Z / 3f * 2f;
            _area4.X = _size.X / 3f;
            _area4.Z = _size.Z / 3f * 2f;
        }

        public string GetAreaGroup(Size position)
        {
            //position의 A, B, C의 위치를 탐색하여 리턴
            if (position == null)
                return string.Empty;

            //컨테이너의 너방향 양측
            if (position.Z < _area1.Z)
                return "C";
            if (position.Z > _area4.Z)
                return "C";
            
            //중앙부분
            if (position.X < _area1.X) //운전석쪽
                return "B"; 
            if (position.X <= _area2.X) 
                return "C";
            if (position.X > _area2.X) //도어쪽
                return "B";

            /*    운전석쪽
            +-----+-----+-----+
            |     |     |     |
            |  C  |  B  |  C  |
            |     |     |     |
            +-----+-----+-----+
            |     |     |     |
            |  C  |  A  |  C  |
            |     |     |     |
            +-----+-----+-----+
            |     |     |     |
            |  C  |  B  |  C  |
            |     |     |     |
            +-----+-----+-----+                
                  도어쪽      */

            System.Diagnostics.Debug.Assert(false); //여기 들어오면 안됨
            return "NOT AVAILABLE";



        }



        public double TareWeight
        {
            get
            {
                System.Diagnostics.Debug.Assert(_maxWeightIncludeTareWeight >= _tareWeight);
                return _tareWeight;
            }

            set
            {
                _tareWeight = value;
            }
        }

        public double MaxWeightIncludeTareWeight
        {
            get
            {
                System.Diagnostics.Debug.Assert(_maxWeightIncludeTareWeight >= _tareWeight);
                return _maxWeightIncludeTareWeight;
            }

            set
            {
                _maxWeightIncludeTareWeight = value;
            }
        }

        internal Coordinate TargetCenterOfGravity
        {
            get
            {
                return _targetCenterOfGravity;
            }

            set
            {
                _targetCenterOfGravity = value;
            }
        }
    }
}
