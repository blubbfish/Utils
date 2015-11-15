using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlubbFish.Utils {
  abstract public class OwnObject {
    private List<Tuple<DateTime, string, string, LogLevel>> loglist = new List<Tuple<DateTime, string, string, LogLevel>>();

    public delegate void LogEvent(string location, string message, LogLevel level, DateTime date);
    public enum LogLevel : int {
      Debug = 1,
      Notice = 2,
      Info = 4,
      Warn = 8,
      Error = 16
    }
    
    public event LogEvent eventDebug;
    public event LogEvent eventNotice;
    public event LogEvent eventInfo;
    public event LogEvent eventWarn;
    public event LogEvent eventError;
    public event LogEvent EventLog;

    /// <summary>
    /// Get the Complete Log
    /// </summary>
    public List<string> getLog(LogLevel level, bool classNames, bool timeStamps) {
      List<string> ret = new List<string>();
      foreach(Tuple<DateTime, string, string, LogLevel> t in this.loglist) {
        if(t.Item4 >= level) {
          ret.Add(logToString(t.Item2, t.Item3, t.Item4, t.Item1, classNames, timeStamps));
        }
      }
      return ret;
    }

    /// <summary>
    /// Formates a LogMessage to a String
    /// </summary>
    public static string logToString(string location, string message, LogLevel level, DateTime date, bool classNames, bool timeStamps) {
      return (timeStamps ? "[" + date.ToString("R") + "]: "+level.ToString()+" " : "") + (classNames ? location + ", " : "") + message;
    }

    protected void addLog(string location, string message, LogLevel level) {
      this.addLog(location, message, level, DateTime.Now);
    }

    protected void addLog(string location, string message, LogLevel level, DateTime date) {
      if(eventDebug != null && level >= LogLevel.Debug) {
        eventDebug(location, message, level, date);
      }
      if(eventNotice != null && level >= LogLevel.Notice) {
        eventNotice(location, message, level, date);
      }
      if(eventInfo != null && level >= LogLevel.Info) {
        eventInfo(location, message, level, date);
      }
      if(eventWarn != null && level >= LogLevel.Warn) {
        eventWarn(location, message, level, date);
      }
      if(eventError != null && level >= LogLevel.Error) {
        eventError(location, message, level, date);
      }
      if(EventLog != null) {
        EventLog(location, message, level, date);
      }
      this.loglist.Add(new Tuple<DateTime, string, string, LogLevel>(date, location, message, level));
    }
  }
}
