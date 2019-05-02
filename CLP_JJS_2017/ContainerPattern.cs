using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPLib
{
    class ContainerPattern : Container
    {
        private Coordinate _centerOfGravity;
        private List<LoadedBox> listBoxLoaded;

        internal Coordinate CenterOfGravity
        {
            get
            {
                return _centerOfGravity;
            }

            set
            {
                _centerOfGravity = value;
            }
        }

        internal List<LoadedBox> ListBoxLoaded
        {
            get
            {
                return listBoxLoaded;
            }

            set
            {
                listBoxLoaded = value;
            }
        }
    }
}
