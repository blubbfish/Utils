
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace BlubbFish.Utils {
  public class Updater : OwnObject {
    private static Updater instances;
    private String url;
    private VersionInfo[] versions;
    private Thread t;

    public struct VersionInfo {
      public VersionInfo(Type type) {
        this.Name = type.Assembly.GetName().Name;
        this.Version = type.Assembly.GetName().Version.ToString();
        this.Filename = type.Assembly.ManifestModule.Name;
        this.GUID = ((GuidAttribute)type.Assembly.GetCustomAttribute(typeof(GuidAttribute))).Value;
        this.HasUpdate = false;
      }

      public String Name { get; private set; }
      public String Version { get; private set; }
      public String Filename { get; private set; }
      public String GUID { get; private set; }
      public Boolean HasUpdate { get; set; }
    }

    public delegate void UpdateStatus(Object sender, UpdaterEventArgs e);
    public delegate void UpdateFail(Object sender, UpdaterFailEventArgs e);

    public event UpdateStatus UpdateResult;
    public event UpdateFail ErrorRaised;

    private Updater() { }

    /// <summary>
    /// Get Instance of Updater
    /// </summary>
    public static Updater Instance {
      get {
        if(instances == null) {
          instances = new Updater();
        }
        return instances;
      }
    }

    /// <summary>
    /// Waits for the Result of the Updater thread.
    /// </summary>
    public void WaitForExit(Boolean exceuteUpdate = true) {
      while (this.t.ThreadState == ThreadState.Running) { }
      if(exceuteUpdate) {
        if(File.Exists("update.bat")) {
          _ = System.Diagnostics.Process.Start("update.bat");
        }
      }
    }

    /// <summary>
    /// Set Path to check for Updates
    /// </summary>
    /// <param name="url">HTTP URI</param>
    public void SetUpdateInfo(String url, VersionInfo[] versions) {
      this.url = url;
      this.versions = versions;
      FileStream file = new FileStream("version.xml",FileMode.Create);
      XmlTextWriter xml = new XmlTextWriter(file, Encoding.UTF8);
      xml.WriteStartDocument();
      xml.WriteWhitespace("\n");
      xml.WriteStartElement("filelist");
      xml.WriteWhitespace("\n");
      foreach (VersionInfo version in versions) {
        xml.WriteWhitespace("\t");
        xml.WriteStartElement("file");
        xml.WriteAttributeString("Version", version.Version);
        xml.WriteAttributeString("Filename", version.Filename);
        xml.WriteAttributeString("GUID", version.GUID);
        xml.WriteString(version.Name);
        xml.WriteEndElement();
        xml.WriteWhitespace("\n");
      }
      xml.WriteEndElement();
      xml.Flush();
      file.Flush();
      file.Close();
      xml.Close();
    }

    /// <summary>
    /// Check for Updates
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Check() {
      if(this.url == "") {
        throw new ArgumentException("Zuerst eine URL setzen!");
      }
      if(this.versions.Length == 0) {
        throw new ArgumentException("Zuerst Dateien registrieren!");
      }
      if(this.UpdateResult == null) {
        throw new ArgumentNullException("Zuerst das Update Event anhängen.");
      }
      this.t = new Thread(this.Runner);
      this.t.Start();
    }

    private void Runner() {
      Thread.Sleep(1000);
      try {
        Stream stream = WebRequest.Create(this.url + "version.xml").GetResponse().GetResponseStream();
        StreamReader sr = new StreamReader(stream);
        String content = sr.ReadToEnd();
        sr.Close();
        Boolean update = false;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);
        foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
          String guid = node.Attributes["GUID"].Value;
          String version = node.Attributes["Version"].Value;
          for(Int32 i=0;i<this.versions.Length;i++) {
            if (this.versions[i].GUID == guid && this.versions[i].Version != version) {
              this.versions[i].HasUpdate = true;
              update = true;
            }
          }
        }
        if (update) {
          this.UpdateResult(this, new UpdaterEventArgs(true, "Update verfügbar"));
          return;
        }
      } catch (Exception e) {
        this.ErrorRaised?.Invoke(this, new UpdaterFailEventArgs(e));
        return;
      }
      this.UpdateResult(this, new UpdaterEventArgs(false, "Kein Update verfügbar"));
    }

    /// <summary>
    /// Update the file
    /// </summary>
    /// <param name="afterExit">Updates the Programm after it has been closed</param>
    /// <returns></returns>
    public Boolean Update(Boolean afterExit = true) {
      try {
        if (afterExit) {
          this.UpdateAfter();
        } else {
          this.UpdateNow();
        }
      } catch (Exception e) {
        this.ErrorRaised?.Invoke(this, new UpdaterFailEventArgs(e));
        return false;
      }
      return true;
    }

    private void UpdateAfter() {
      this.UpdateNow(true);
      StreamWriter update = new StreamWriter("update.bat", false);
      update.WriteLine("echo off");
      update.WriteLine("echo \"Warte 10s\"");
      update.WriteLine("ping 127.0.0.1 -n 10");
      update.WriteLine("echo \"Kopiere Dateien....\"");
      foreach (VersionInfo file in this.versions) {
        if (file.HasUpdate) {
          update.WriteLine("echo \"Kopiere " + file.Filename + "\"");
          update.WriteLine("del " + file.Filename);
          update.WriteLine("move " + file.Filename + "_ " + file.Filename);
        }
      }
      update.WriteLine("start cmd /C ping 127.0.0.1 -n 10 & del update.bat");
      update.Flush();
      update.Close();
    }

    private void UpdateNow(Boolean forAfter = false) {
      foreach (VersionInfo file in this.versions) {
        if (file.HasUpdate) {
          Stream stream = WebRequest.Create(this.url + file.Filename).GetResponse().GetResponseStream();
          FileStream target = new FileStream(file.Filename + (forAfter ? "_" : ""), FileMode.Create);
          stream.CopyTo(target);
          target.Flush();
          target.Close();
        }
      }
    }
  }
}
