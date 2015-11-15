using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlubbFish.Utils
{
    public abstract class OwnView
    {
        /// <summary>
        /// Called if the Oberver (Model) updates its View
        /// </summary>
        abstract public void update();
        /// <summary>
        /// Called if view is viewed
        /// </summary>
        abstract protected void init();
        /// <summary>
        /// Called if Form is Disposed
        /// </summary>
        abstract public void Dispose();
    }
}
