using System;

namespace BlubbFish.Utils {
  public class UpdaterEventArgs : EventArgs {
    public UpdaterEventArgs(Boolean hasUpdates, String message) {
      this.HasUpdates = hasUpdates;
      this.Message = message;
    }
    public String Message { get; private set; }
    public Boolean HasUpdates { get; private set; }
  }

  public class UpdaterFailEventArgs : EventArgs {
    public UpdaterFailEventArgs(Exception e) {
      this.Except = e;
    }

    public Exception Except { get; private set; }
  }

  public class LogEventArgs : EventArgs {
    public LogEventArgs(String location, String message, OwnObject.LogLevel level, DateTime date) {
      this.Location = location;
      this.Message = message;
      this.Level = level;
      this.Date = date;
    }

    public String Location { get; private set; }
    public String Message { get; private set; }
    public OwnObject.LogLevel Level { get; private set; }
    public DateTime Date { get; private set; }
  }
}
