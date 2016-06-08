using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace BlubbFish.Utils {
  public class FileLogger {
    private static Dictionary<string, FileLogger> instances = new Dictionary<string, FileLogger>();
    private static String logDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
    private StreamWriter file;
    private FileLogger(string filename, bool append) {
      filename = logDir + filename;
      if(!File.Exists(filename)) {
        string folder = Path.GetDirectoryName(Path.GetFullPath(filename));
        if(!Directory.Exists(folder)) {
          Directory.CreateDirectory(folder);
        }
      }
      this.file = new StreamWriter(filename, append, Encoding.UTF8);
      this.file.AutoFlush = true;
    }
    public static FileLogger getInstance(string filename, bool append) {
      if(!instances.Keys.Contains(filename)) {
        instances.Add(filename, new FileLogger(filename, append));
      }
      return instances[filename];
    }

    public static void setLogDir(String v) {
      v = v.Replace("..", "");
      v = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + v;
      if(Directory.Exists(v)) {
        logDir = v;
      } else {
        Directory.CreateDirectory(v);
        logDir = v;
      }
      if(logDir.Substring(logDir.Length - 1) != Path.DirectorySeparatorChar.ToString()) {
        logDir = logDir + Path.DirectorySeparatorChar;
      }
    }

    public void setArray(string[] text) {
      this.file.Write(String.Join(file.NewLine, text) + file.NewLine);
      this.file.Flush();
    }

    public void setLine(string text) {
      this.file.WriteLine(text);
      this.file.Flush();
    }
    public void setLine(string text, DateTime d) {
      this.setLine(d.ToString("[yyyy-MM-dd HH:mm:ss.ffff] " + text));
    }
  }
}
