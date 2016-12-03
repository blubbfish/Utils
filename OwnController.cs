using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlubbFish.Utils
{
    public abstract class OwnController
    {
        /// <summary>
        /// Führt den Controller aus.
        /// </summary>
        public void execute()
        {
            this.init();
        }
        abstract protected void init();
        abstract public void Dispose();
    }
}
