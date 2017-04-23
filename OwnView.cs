using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlubbFish.Utils {
  public abstract class OwnView {

    protected OwnView() {
      this.Init();
    }
    /// <summary>
    /// Called if the Oberver (Model) updates its View
    /// </summary>
    public abstract void Update();
    /// <summary>
    /// Called if view is viewed
    /// </summary>
    protected abstract void Init();
    /// <summary>
    /// Called if Form is Disposed
    /// </summary>
    public abstract void Dispose();
  }
}
