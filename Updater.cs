
using System;
using System.IO;

namespace BlubbFish.Utils {
  public class Updater : OwnObject {
    private static Updater instances;
    private String url;
    private String[] versions;

    public class UpdaterEventArgs : EventArgs {
      public UpdaterEventArgs(Boolean hasUpdates, String message) {
        this.HasUpdates = hasUpdates;
        this.Message = message;
      }

      public String Message { get; private set; }
      public Boolean HasUpdates { get; private set; }
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
    public void SetPath(String url, String[] versions) {
      this.url = url;
      this.versions = versions;
      StreamWriter file = new StreamWriter("version.txt");
      file.BaseStream.SetLength(0);
      file.BaseStream.Flush();
      file.BaseStream.Seek(0, SeekOrigin.Begin);
      foreach (String version in versions) {
        file.WriteLine(version);
      }
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
    }

    /// <summary>
    /// Update the file
    /// </summary>
    /// <param name="filename">The filename of the targetfile</param>
    /// <param name="url">The url of the sourcefile</param>
    /// <param name="afterExit">Updates the Programm after it has been closed</param>
    /// <returns></returns>
    public Boolean Update(String filename, String url, Boolean afterExit = true) {
      return true;
    }
  }
}
