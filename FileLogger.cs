using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BlubbFish.Utils {
  public class FileLogger {
    private static Dictionary<string, FileLogger> instances = new Dictionary<string, FileLogger>();
    private StreamWriter file;
    private FileLogger(string filename, bool append) {
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
