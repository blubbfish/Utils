using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlubbFish.Utils {
  public class FileMutex {
    private static FileMutex instance;
    private String filename;
    private StreamWriter file;
    private FileMutex() { }

    public static FileMutex Instance {
      get {
        if(FileMutex.instance == null) {
          FileMutex.instance = new FileMutex();
        }
        return FileMutex.instance;
      }
    }

    public void setName(string name) {
      string path = AppDomain.CurrentDomain.BaseDirectory;
      this.filename = path + string.Join(string.Empty, Array.ConvertAll(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(name)), b => b.ToString("X2"))) + ".lock.txt";
    }

    public bool create() {
      if(File.Exists(this.filename))
        return false;
      this.file = new StreamWriter(this.filename);
      this.initFile();
      return File.Exists(this.filename) && file != null;
    }

    private void initFile() {
      this.file.Write("Created: " + DateTime.Now.ToUniversalTime()+"\n");
      this.file.Flush();
    }

    public bool delete() {
      this.file.Close();
      File.Delete(this.filename);
      return !File.Exists(this.filename);
    }
  }
}
