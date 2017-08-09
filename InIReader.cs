using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlubbFish.Utils
{
  public class InIReader : IDisposable
  {
    private Dictionary<String, Dictionary<String, String>> cont;
    private FileSystemWatcher k = new FileSystemWatcher(Directory.GetCurrentDirectory(), "*.ini");
    private String filename;

    private static Dictionary<String, InIReader> instances = new Dictionary<String, InIReader>();

    private InIReader(String filename)
    {
      this.filename = filename;
      this.k.Changed += new FileSystemEventHandler(this.ReadAgain);
      LoadFile();
    }

    public static InIReader GetInstance(String filename)
    {
      if (!instances.Keys.Contains(filename)) {
        instances.Add(filename, new InIReader(filename));
      }
      return instances[filename];
    }

    private void ReadAgain(Object sender, EventArgs e)
    {
      this.LoadFile();
    }

    private void LoadFile()
    {
      this.cont = new Dictionary<String, Dictionary<String, String>>();
      StreamReader file = new StreamReader(this.filename);
      List<String> buf = new List<String>();
      String fline = "";
      while (fline != null) {
        fline = file.ReadLine();
        if (fline != null && fline.Length > 0 && fline.Substring(0, 1) != ";") {
          buf.Add(fline);
        }
      }
      file.Close();
      Dictionary<String, String> sub = new Dictionary<String, String>();
      String cap = "";
      foreach (String line in buf) {
        Match match = Regex.Match(line, @"^\[[a-zA-ZäöüÄÖÜ0-9\-_ ]+\]\w*$", RegexOptions.IgnoreCase);
        if (match.Success) {
          if (sub.Count != 0 && cap != "") {
            this.cont.Add(cap, sub);
          }
          cap = line;
          sub = new Dictionary<String, String>();
        } else {
          if (line != "" && cap != "") {
            String key = line.Substring(0, line.IndexOf('='));
            String value = line.Substring(line.IndexOf('=') + 1);
            sub.Add(key, value);
          }
        }
      }
      if (sub.Count != 0 && cap != "") {
        this.cont.Add(cap, sub);
      }
    }

    public List<String> GetSections()
    {
      return this.cont.Keys.ToList<String>();
    }

    public Dictionary<String, String> GetSection(String section) {
      if(this.cont.Keys.Contains(section)) {
        return this.cont[section];
      }
      return new Dictionary<string, string>();
    }

    public String GetValue(String section, String key)
    {
      if (!section.StartsWith("[")) {
        section = "[" + section + "]";
      }
      if (this.cont.Keys.Contains(section)) {
        if (this.cont[section].Keys.Contains(key)) {
          return this.cont[section][key];
        }
      }
      return null;
    }


    public void SetValue(String section, String key, String value)
    {
      if (!section.StartsWith("[")) {
        section = "[" + section + "]";
      }
      if (this.cont.Keys.Contains(section)) {
        if (this.cont[section].Keys.Contains(key)) {
          this.cont[section][key] = value;
        } else {
          this.cont[section].Add(key, value);
        }
      } else {
        Dictionary<String, String> sub = new Dictionary<String, String> {
          { key, value }
        };
        this.cont.Add(section, sub);
      }
      this.Changed();
    }

    private void Changed()
    {
      this.k.Changed -= null;
      SaveSettings();
      LoadFile();
      this.k.Changed += new FileSystemEventHandler(this.ReadAgain);
    }

    private void SaveSettings()
    {
      StreamWriter file = new StreamWriter(this.filename);
      file.BaseStream.SetLength(0);
      file.BaseStream.Flush();
      file.BaseStream.Seek(0, SeekOrigin.Begin);
      foreach (KeyValuePair<String, Dictionary<String, String>> cap in this.cont) {
        file.WriteLine(cap.Key);
        foreach (KeyValuePair<String, String> sub in cap.Value) {
          file.WriteLine(sub.Key + "=" + sub.Value);
        }
        file.WriteLine();
      }
      file.Flush();
      file.Close();
    }

    /// <summary>
    /// Fügt eine neue Sektion in der Ini-Datei ein.
    /// </summary>
    /// <param name="name">Sektionsname</param>
    /// <returns>true if added, false if error</returns>
    public Boolean AddSection(String name)
    {
      if (!name.StartsWith("[")) {
        name = "[" + name + "]";
      }
      if (this.cont.Keys.Contains(name)) {
        return false;
      }
      this.cont.Add(name, new Dictionary<String, String>());
      this.Changed();
      return true;
    }

    /// <summary>
    /// Löscht eine Sektion inklusive Unterpunkte aus der Ini-Datei.
    /// </summary>
    /// <param name="name">Sektionsname</param>
    /// <returns>true if removed, false if error</returns>
    public Boolean RemoveSection(String name)
    {
      if (!name.StartsWith("[")) {
        name = "[" + name + "]";
      }
      if (!this.cont.Keys.Contains(name)) {
        return false;
      }
      this.cont.Remove(name);
      this.Changed();
      return false;
    }
    protected virtual void Dispose(Boolean disposing) {
      if (disposing) {
        this.k.Dispose();
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
