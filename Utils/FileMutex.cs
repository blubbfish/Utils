using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BlubbFish.Utils
{
  public class FileMutex : IDisposable
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
      SHA512Managed sha = new SHA512Managed();
      this.filename = path + String.Join(String.Empty, Array.ConvertAll(sha.ComputeHash(Encoding.UTF8.GetBytes(name)), b => b.ToString("X2"))) + ".lock.txt";
      sha.Dispose();
    }

    public Boolean Create()
    {
      if (File.Exists(this.filename)) {
        return false;
      }

      this.file = new StreamWriter(this.filename);
      this.InitFile();
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

    protected virtual void Dispose(Boolean disposing) {
      if (disposing) {
        if(this.file != null) {
          this.file.Close();
        }
      }
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
