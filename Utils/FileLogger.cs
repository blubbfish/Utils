using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace BlubbFish.Utils
{
  public class FileLogger
  {
    private static Dictionary<String, FileLogger> instances = new Dictionary<String, FileLogger>();
    private static String logDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
    private readonly StreamWriter file;
    private FileLogger(String filename, Boolean append)
    {
      filename = logDir + filename;
      if (!File.Exists(filename)) {
        String folder = Path.GetDirectoryName(Path.GetFullPath(filename));
        if (!Directory.Exists(folder)) {
          Directory.CreateDirectory(folder);
        }
      }
      this.file = new StreamWriter(filename, append, Encoding.UTF8) {
        AutoFlush = true
      };
    }
    public static FileLogger GetInstance(String filename, Boolean append)
    {
      if (!instances.Keys.Contains(filename)) {
        instances.Add(filename, new FileLogger(filename, append));
      }
      return instances[filename];
    }

    public static void SetLogDir(String v)
    {
      v = v.Replace("..", "");
      v = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + v;
      if (Directory.Exists(v)) {
        logDir = v;
      } else {
        Directory.CreateDirectory(v);
        logDir = v;
      }
      if (logDir.Substring(logDir.Length - 1) != Path.DirectorySeparatorChar.ToString()) {
        logDir = logDir + Path.DirectorySeparatorChar;
      }
    }

    public void SetArray(String[] text)
    {
      this.file.Write(String.Join(this.file.NewLine, text) + this.file.NewLine);
      this.file.Flush();
    }

    public void SetLine(String text)
    {
      this.file.WriteLine(text);
      this.file.Flush();
    }
    public void SetLine(String text, DateTime d)
    {
      this.SetLine(d.ToString("[yyyy-MM-dd HH:mm:ss.ffff] ") + text);
    }
  }
}
