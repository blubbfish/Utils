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
            get
            {
                return _instance.Value;
            }
        }
        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        public void setObserver(OwnView tray)
        {
            this.observer.Add(tray);
            tray.update();
        }
        protected void update()
        {
            this.observer.ForEach(delegate(OwnView tray) { tray.update(); });
        }
        abstract protected void init();
    }
}
