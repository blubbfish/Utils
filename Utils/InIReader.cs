using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlubbFish.Utils {
  public class InIReader : IDisposable
  {
    private Dictionary<String, Dictionary<String, String>> inifile;
    private readonly FileSystemWatcher k;
    private readonly String filename;
    private static List<String> search_path = new List<String>() {
      Directory.GetCurrentDirectory()
    };

    private static Dictionary<String, InIReader> instances = new Dictionary<String, InIReader>();

    public static void SetSearchPath(List<String> directorys) {
      search_path.AddRange(directorys);
    }

    public static Boolean ConfigExist(String filename) {
      foreach (String path in search_path) {
        if (File.Exists(path + Path.DirectorySeparatorChar + filename)) {
          return true;
        } else if (File.Exists(path + Path.DirectorySeparatorChar + filename + ".ini")) {
          return true;
        } else if (File.Exists(path + Path.DirectorySeparatorChar + filename + ".conf")) {
          return true;
        }
      }
      return false;
    }

    private InIReader(String filename)
    {
      foreach (String path in search_path) {
        if (File.Exists(path + Path.DirectorySeparatorChar + filename)) {
          this.filename = path + Path.DirectorySeparatorChar + filename;
          this.k = new FileSystemWatcher(path, filename);
          break;
        } else if (File.Exists(path + Path.DirectorySeparatorChar + filename + ".ini")) {
          this.filename = path + Path.DirectorySeparatorChar + filename + ".ini";
          this.k = new FileSystemWatcher(path, filename + ".ini");
          break;
        } else if(File.Exists(path + Path.DirectorySeparatorChar + filename + ".conf")) {
          this.filename = path + Path.DirectorySeparatorChar + filename + ".conf";
          this.k = new FileSystemWatcher(path, filename + ".conf");
          break;
        }
      }
      if(this.filename == null) {
        throw new ArgumentException(filename + " not found!");
      }
      this.k.Changed += new FileSystemEventHandler(this.ReadAgain);
      LoadFile();
    }

    /// <summary>
    /// Gibt eine InIReader-Instanz zu einer Datei zurück
    /// </summary>
    /// <param name="filename">Dateiname</param>
    /// <returns></returns>
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
      this.inifile = new Dictionary<String, Dictionary<String, String>>();
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
        Match match = Regex.Match(line, @"^\[[a-zA-ZäöüÄÖÜ0-9\-\._/ ]+\]\w*$", RegexOptions.IgnoreCase);
        if (match.Success) {
          if (sub.Count != 0 && cap != "") {
            this.inifile.Add(cap, sub);
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
        this.inifile.Add(cap, sub);
      }
    }

    /// <summary>
    /// Gibt eine Liste an Sektionen zurück
    /// </summary>
    /// <param name="withBrackets">Default = true; false, wenn die Liste ohne Klammern sein soll.</param>
    /// <returns></returns>
    public List<String> GetSections(Boolean withBrackets = true)
    {
      if(withBrackets) {
        return this.inifile.Keys.ToList();
      } else {
        List<String> ret = new List<String>();
        foreach (String item in this.inifile.Keys) {
          ret.Add(item.Substring(1, item.Length - 2));
        }
        return ret;
      }
    }

    /// <summary>
    /// Überschreibt eine InI-Datei mit der Kompletten neuen Configuration
    /// </summary>
    /// <param name="config">Neue Konfiguration</param>
    public void SetSections(Dictionary<String, Dictionary<String, String>> config) {
      this.inifile.Clear();
      foreach (KeyValuePair<String, Dictionary<String, String>> item in config) {
        String key = item.Key;
        if(!key.StartsWith("[")) {
          key = "[" + key + "]";
        }
        if (Regex.Match(key, @"^\[[a-zA-ZäöüÄÖÜ0-9\-\._/ ]+\]\w*$", RegexOptions.IgnoreCase).Success) {
          this.inifile.Add(key, item.Value);
        }
      }
      this.Changed();
    }

    public Dictionary<String, String> GetSection(String section) {
      if(this.inifile.Keys.Contains(section)) {
        return this.inifile[section];
      }
      if(this.inifile.Keys.Contains("["+section+"]")) {
        return this.inifile["[" + section + "]"];
      }
      return new Dictionary<String, String>();
    }

    /// <summary>
    /// Gibt einen einzelnen Wert zurück
    /// </summary>
    /// <param name="section">Name der Sektion</param>
    /// <param name="key">Name des Wertes</param>
    /// <returns></returns>
    public String GetValue(String section, String key)
    {
      if (!section.StartsWith("[")) {
        section = "[" + section + "]";
      }
      if (this.inifile.Keys.Contains(section)) {
        if (this.inifile[section].Keys.Contains(key)) {
          return this.inifile[section][key];
        }
      }
      return null;
    }

    /// <summary>
    /// Setzt einen Wert in einer Sektion
    /// </summary>
    /// <param name="section">Name der Sektion</param>
    /// <param name="key">Name des Wertes</param>
    /// <param name="value">Wert</param>
    public void SetValue(String section, String key, String value)
    {
      if (!section.StartsWith("[")) {
        section = "[" + section + "]";
      }
      if (this.inifile.Keys.Contains(section)) {
        if (this.inifile[section].Keys.Contains(key)) {
          this.inifile[section][key] = value;
        } else {
          this.inifile[section].Add(key, value);
        }
      } else {
        Dictionary<String, String> sub = new Dictionary<String, String> {
          { key, value }
        };
        this.inifile.Add(section, sub);
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
      foreach (KeyValuePair<String, Dictionary<String, String>> cap in this.inifile) {
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
      if (this.inifile.Keys.Contains(name)) {
        return false;
      }
      this.inifile.Add(name, new Dictionary<String, String>());
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
      if (!this.inifile.Keys.Contains(name)) {
        return false;
      }
      this.inifile.Remove(name);
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
