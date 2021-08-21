using System;
using System.ComponentModel;
using System.Reflection;

namespace BlubbFish.Utils {
  public static class Helper {
    #region PropertyHelper
    public static Boolean HasProperty(this Object o, String type) {
      Type t = o.GetType();
      foreach (PropertyInfo item in t.GetProperties()) {
        if (item.Name == type) {
          return true;
        }
      }
      return false;
    }

    public static Object GetProperty(this Object o, String name) {
      PropertyInfo prop = o.GetType().GetProperty(name);
      return prop.CanRead ? prop.GetValue(o) : null;
    }

    public static void SetProperty(this Object o, String name, String value) {
      PropertyInfo prop = o.GetType().GetProperty(name);
      if (prop.CanWrite) {
        if (prop.PropertyType == typeof(Boolean) && Boolean.TryParse(value, out Boolean vb)) {
          prop.SetValue(o, vb);
        } else if (prop.PropertyType == typeof(Byte) && Byte.TryParse(value, out Byte v8)) {
          prop.SetValue(o, v8);
        } else if (prop.PropertyType == typeof(Int32) && Int32.TryParse(value, out Int32 v32)) {
          prop.SetValue(o, v32);
        } else if (prop.PropertyType == typeof(Single) && Single.TryParse(value, out Single vs)) {
          prop.SetValue(o, vs);
        } else if (prop.PropertyType == typeof(Double) && Double.TryParse(value, out Double vd)) {
          prop.SetValue(o, vd);
        } else if (prop.PropertyType == typeof(Int64) && Int64.TryParse(value, out Int64 v64)) {
          prop.SetValue(o, v64);
        } else if (prop.PropertyType.BaseType == typeof(Enum)) {
          try {
            prop.SetValue(o, Enum.Parse(prop.PropertyType, value));
          } catch (Exception) { }
        }
      }
    }
    #endregion

    #region FieldHelper
    public static Object GetField(this Object o, String name) {
      FieldInfo field = o.GetType().GetField(name);
      return field.IsPublic ? field.GetValue(o) : null;
    }

    public static Object GetField(this Type o, String name) {
      FieldInfo field = o.GetField(name);
      return field.IsPublic ? field.GetValue(o) : null;
    }

    public static T GetEvent<T>(this Object o, String name) {
      FieldInfo field = o.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
      return (T)field?.GetValue(o);
    }
    #endregion

    #region InterfaceHelper
    public static Boolean HasInterface(this Type o, Type interf) {
      foreach (Type item in o.GetInterfaces()) {
        if (item == interf) {
          return true;
        }
      }
      return false;
    }

    public static Boolean HasAbstract(this Type o, Type type) => o.BaseType == type;
    #endregion

    #region StringHelper
    public static String GetEnumDescription(Enum value) {
      FieldInfo fi = value.GetType().GetField(value.ToString());
      DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
      return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }

    public static String ToUpperLower(this String s) => s.Length == 0 ? "" : s.Length == 1 ? s.ToUpper() : s[0].ToString().ToUpper() + s[1..].ToLower();

    public static void WriteError(String text) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Error.WriteLine("ERROR: " + text);
      Console.ResetColor();
    }
    #endregion
  }
}
