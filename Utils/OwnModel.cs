using System;
using System.Collections.Generic;

namespace BlubbFish.Utils {
  public abstract class OwnModel<T> where T : class
  {
    private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstanceOfT());
    private readonly List<OwnView> observer = new List<OwnView>();
    public static T Instance => _instance.Value;
    private static T CreateInstanceOfT() => Activator.CreateInstance(typeof(T), true) as T;

    public void SetObserver(OwnView view)
    {
      this.observer.Add(view);
      view.Update();
    }

    public void RemoveObserver(OwnView view) => _ = this.observer.Remove(view);
    protected void Update() => this.observer.ForEach(delegate (OwnView view) {
      view.Update();
    });
    abstract protected void Init();
    abstract public void Dispose();
  }
}
