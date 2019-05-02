using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    public class BoxToLoad : Box
    {
        protected int _countToLoad;
        protected int _loadOrientation;

        private string _WEIGHT_RANK_GROUP; //중량그룹 A B C --> A그룹이 무겁고,C그룹이 가장 가벼움


        public BoxToLoad()
        {
            CountToLoad = 0;
            LoadOrientation = 1 | 2;
        }

        public object Clone()
        {
            BoxToLoad boxToLoad = new BoxToLoad();
            boxToLoad.CountToLoad = CountToLoad;
            boxToLoad.LoadOrientation = LoadOrientation;
            boxToLoad.BoxColor = base.BoxColor;
            boxToLoad.Name = base.Name;
            boxToLoad.Size = base.Size;
            boxToLoad.Weight = base.Weight;
            boxToLoad.WEIGHT_RANK_GROUP = this._WEIGHT_RANK_GROUP;

            return boxToLoad;
        }


        public int CountToLoad
        {
            get
            {
                return _countToLoad;
            }

            set
            {
                _countToLoad = value;
            }
        }

        public int LoadOrientation
        {
            get
            {
                return _loadOrientation;
            }

            set
            {
                _loadOrientation = value;
            }
        }

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
    }
}
