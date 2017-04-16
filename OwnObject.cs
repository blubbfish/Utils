using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlubbFish.Utils
{
  abstract public class OwnObject
  {
    private List<Tuple<DateTime, String, String, LogLevel>> loglist = new List<Tuple<DateTime, String, String, LogLevel>>();

    public delegate void LogEvent(String location, String message, LogLevel level, DateTime date);
    public enum LogLevel : int
    {
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
    public List<String> GetLog(LogLevel level, Boolean classNames, Boolean timeStamps)
    {
      List<String> ret = new List<String>();
      foreach (Tuple<DateTime, String, String, LogLevel> t in this.loglist) {
        if (t.Item4 >= level) {
          ret.Add(LogToString(t.Item2, t.Item3, t.Item4, t.Item1, classNames, timeStamps));
        }
      }
      return ret;
    }

    /// <summary>
    /// Formates a LogMessage to a String
    /// </summary>
    public static String LogToString(String location, String message, LogLevel level, DateTime date, Boolean classNames, Boolean timeStamps)
    {
      return (timeStamps ? "[" + date.ToString("R") + "]: " + level.ToString() + " " : "") + (classNames ? location + ", " : "") + message;
    }

    protected void AddLog(String location, String message, LogLevel level)
    {
      this.AddLog(location, message, level, DateTime.Now);
    }

    protected void AddLog(String location, String message, LogLevel level, DateTime date)
    {
      if (EventDebug != null && level >= LogLevel.Debug) {
        EventDebug(location, message, level, date);
      }
      if (EventNotice != null && level >= LogLevel.Notice) {
        EventNotice(location, message, level, date);
      }
      if (EventInfo != null && level >= LogLevel.Info) {
        EventInfo(location, message, level, date);
      }
      if (EventWarn != null && level >= LogLevel.Warn) {
        EventWarn(location, message, level, date);
      }
      if (EventError != null && level >= LogLevel.Error) {
        EventError(location, message, level, date);
      }
      if (EventLog != null) {
        EventLog(location, message, level, date);
      }
      this.loglist.Add(new Tuple<DateTime, String, String, LogLevel>(date, location, message, level));
    }
  }
}
