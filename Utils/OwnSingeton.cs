using System;
using System.Collections.Generic;
using System.Text;

namespace BlubbFish.Utils {
  public abstract class OwnSingeton<T> where T : class  {
    private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstanceOfT());
    public static T Instance => _instance.Value;
    private static T CreateInstanceOfT() => Activator.CreateInstance(typeof(T), true) as T;
  }
}
