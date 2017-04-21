using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlubbFish.Utils
{
  abstract public class OwnObject
  {
    public struct LogObject {
      public LogObject(DateTime date, String location, String message, LogLevel level) {
        this.Date = date;
        this.Location = location;
        this.Message = message;
        this.Level = level;
      }

      public DateTime Date { get; set; }
      public String Location { get; set; }
      public String Message { get;  set; }
      public LogLevel Level { get; set; }
      /// <summary>
      /// Formates a LogMessage to a String
      /// </summary>
      /// <returns>Formated String</returns>
      public override String ToString() {
        return "[" + this.Date.ToString("R") + "]: " + this.Level.ToString() + " "+ this.Location + ", " + this.Message;
      }
      /// <summary>
      /// Formates a LogMessage to a String
      /// </summary>
      /// <param name="classNames">Enables the output of the location</param>
      /// <param name="timeStamps">Enables the output of the date</param>
      /// <returns>Formated String</returns>
      public String ToString(Boolean classNames, Boolean timeStamps) {
        return (timeStamps ? "[" + this.Date.ToString("R") + "]: " + this.Level.ToString() + " " : "") + (classNames ? this.Location + ", " : "") + this.Message;
      }
    }

    public class LogEventArgs : EventArgs {
      public LogEventArgs(String location, String message, LogLevel level, DateTime date) {
        this.Location = location;
        this.Message = message;
        this.Level = level;
        this.Date = date;
      }

      public String Location { get; private set; }
      public String Message { get; private set; }
      public LogLevel Level { get; private set; }
      public DateTime Date { get; private set; }
    }

    private List<LogObject> loglist = new List<LogObject>();

    public delegate void LogEvent(Object sender, LogEventArgs e);
    public enum LogLevel : Int32 {
      Debug = 1,
      Notice = 2,
      Info = 4,
      Warn = 8,
      Error = 16
    }

    public event LogEvent EventDebug;
    public event LogEvent EventNotice;
    public event LogEvent EventInfo;
    public event LogEvent EventWarn;
    public event LogEvent EventError;
    public event LogEvent EventLog;

    /// <summary>
    /// Get the Complete Log
    /// </summary>
    public List<String> GetLog(LogLevel level, Boolean classNames, Boolean timeStamps) {
      List<String> ret = new List<String>();
      foreach (LogObject t in this.loglist) {
        if (t.Level >= level) {
          ret.Add(t.ToString(classNames, timeStamps));
        }
      }
      return ret;
    }

    /// <summary>
    /// Put a message in the log
    /// </summary>
    /// <param name="location">Where the event arrives</param>
    /// <param name="message">The logmessage itselfs</param>
    /// <param name="level">Level of the message</param>
    protected void AddLog(String location, String message, LogLevel level)
    {
      this.AddLog(location, message, level, DateTime.Now);
    }

    /// <summary>
    /// Put a message in the log
    /// </summary>
    /// <param name="location">Where the event arrives</param>
    /// <param name="message">The logmessage itselfs</param>
    /// <param name="level">Level of the message</param>
    /// <param name="date">Date of the message</param>
    protected void AddLog(String location, String message, LogLevel level, DateTime date)
    {
      LogEventArgs e = new LogEventArgs(location, message, level, date);
      if (EventDebug != null && level >= LogLevel.Debug) {
        EventDebug(this, e);
      }
      if (EventNotice != null && level >= LogLevel.Notice) {
        EventNotice(this, e);
      }
      if (EventInfo != null && level >= LogLevel.Info) {
        EventInfo(this, e);
      }
      if (EventWarn != null && level >= LogLevel.Warn) {
        EventWarn(this, e);
      }
      if (EventError != null && level >= LogLevel.Error) {
        EventError(this, e);
      }
      EventLog?.Invoke(this, e);

      this.loglist.Add(new LogObject(date, location, message, level));
    }
  }
}
