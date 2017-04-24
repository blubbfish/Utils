
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

    public class UpdaterEventArgs : EventArgs {
      public UpdaterEventArgs(Boolean hasUpdates, String message) {
        this.HasUpdates = hasUpdates;
        this.Message = message;
      }

      public String Message { get; private set; }
      public Boolean HasUpdates { get; private set; }
    }

    public struct VersionInfo {
      public VersionInfo(String name, String version, String filename, String guid) {
        this.Name = name;
        this.Version = version;
        this.Filename = filename;
        this.GUID = guid;
      }
      public VersionInfo(Type type) {
        this.Name = type.Assembly.GetName().Name;
        this.Version = type.Assembly.GetName().Version.ToString();
        this.Filename = type.Assembly.ManifestModule.Name;
        this.GUID = ((GuidAttribute)type.Assembly.GetCustomAttribute(typeof(GuidAttribute))).Value;
      }

      public String Name { get; private set; }
      public String Version { get; private set; }
      public String Filename { get; private set; }
      public String GUID { get; private set; }
    }

    public delegate void UpdateStatus(Object sender, UpdaterEventArgs e);

    public event UpdateStatus UpdateResult;

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
    public void WaitForExit() {
      throw new NotImplementedException();
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
    }

    /// <summary>
    /// Check for Updates
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Check() {
      if(this.url == "") {
        throw new ArgumentException("You must set url first.");
      }
      if(this.versions.Length == 0) {
        throw new ArgumentException("You must set a Version number first.");
      }
      if(this.UpdateResult == null) {
        throw new ArgumentNullException("You must attach an event first.");
      }
      Thread t = new Thread(this.Runner);
      t.Start();
    }

    private void Runner() {
      Thread.Sleep(1000);
      WebRequest request = WebRequest.Create(this.url + "version.xml");
      WebResponse response = null;
      try {
         response = request.GetResponse();
      } catch(WebException e) {
        this.UpdateResult(this, new UpdaterEventArgs(false, e.Message));
        return;
      }
      Stream stream = response.GetResponseStream();
      StreamReader reader = new StreamReader(stream);
      String content = reader.ReadToEnd();
    }

    /// <summary>
    /// Update the file
    /// </summary>
    /// <param name="filename">The filename of the targetfile</param>
    /// <param name="url">The url of the sourcefile</param>
    /// <param name="afterExit">Updates the Programm after it has been closed</param>
    /// <returns></returns>
    public Boolean Update(Boolean afterExit = true) {
      return true;
    }
  }
}
