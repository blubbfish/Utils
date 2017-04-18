using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BlubbFish.Utils
{
  public class FileMutex
  {
    private static FileMutex instance;
    private String filename;
    private StreamWriter file;
    private FileMutex() { }

    public static FileMutex Instance
    {
      get {
        if (instance == null) {
          instance = new FileMutex();
        }
        return instance;
      }
    }

    public void SetName(String name)
    {
      String path = AppDomain.CurrentDomain.BaseDirectory;
      this.filename = path + String.Join(String.Empty, Array.ConvertAll(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(name)), b => b.ToString("X2"))) + ".lock.txt";
    }

    public Boolean Create()
    {
      if (File.Exists(this.filename)) {
        return false;
      }

      this.file = new StreamWriter(this.filename);
      InitFile();
      return File.Exists(this.filename) && this.file != null;
    }

    private void InitFile()
    {
      this.file.Write("Created: " + DateTime.Now.ToUniversalTime() + "\n");
      this.file.Flush();
    }

    public Boolean Delete()
    {
      if(this.file != null) {
        this.file.Close();
      }

      File.Delete(this.filename);
      return !File.Exists(this.filename);
    }
  }
}
