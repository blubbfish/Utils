
using System;

namespace BlubbFish.Utils {
  public class Updater {
    private static Updater instances;
    private String url;

    public delegate void UpdateStatus(Boolean hasUpdates, String message);

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
    public void SetPath(String url) {
      this.url = url;
    }

    /// <summary>
    /// Check for Updates
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Check() {
      if(this.url == "") {
        throw new ArgumentException("You must set url first.");
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
