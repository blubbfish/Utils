using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlubbFish.Utils
{
  public abstract class OwnModel<T> where T : class
  {
    private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstanceOfT());
    private List<OwnView> observer = new List<OwnView>();
    public static T Instance
    {
      get {
        return _instance.Value;
      }
    }
    private static T CreateInstanceOfT()
    {
      return Activator.CreateInstance(typeof(T), true) as T;
    }

    public void SetObserver(OwnView view)
    {
      this.observer.Add(view);
      view.Update();
    }

    public void RemoveObserver(OwnView view) {
      this.observer.Remove(view);
    }
    protected void Update()
    {
      this.observer.ForEach(delegate (OwnView view) { view.Update(); });
    }
    abstract protected void Init();
  }
}
