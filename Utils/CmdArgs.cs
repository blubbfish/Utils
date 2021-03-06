using System;
using System.Collections.Generic;
using System.Linq;

namespace BlubbFish.Utils
{
  public class CmdArgs
  {
    public enum ArgLength
    {
      Single,
      Touple
    }
    #region Classes
    public struct VaildArguments
    {
      public VaildArguments(ArgLength length, Boolean required, String @default = "", String description = "")
      {
        this.Required = required;
        this.Length = length;
        this.Description = description;
        this.@Default = @default;
      }
      public VaildArguments(ArgLength length, String @default = "", String description = "")
      {
        this.Required = false;
        this.Length = length;
        this.Description = description;
        this.@Default = @default;
      }

      public ArgLength Length { get; private set; }
      public Boolean Required { get; private set; }
      public String Description { get; private set; }
      public String Default { get; private set; }
    }
    private struct ArgTouple
    {
      public ArgTouple(String type, String data)
      {
        this.Type = type;
        this.Data = data;
      }
      public ArgTouple(String type)
      {
        this.Type = type;
        this.Data = null;
      }
      public String Type { get; private set; }
      public String Data { get; private set; }

      internal void SetData(String data)
      {
        if (data != "") {
          this.Data = data;
        }
      }
    }
    #endregion
    private String[] args;
    private List<ArgTouple> argList;
    private Dictionary<String, VaildArguments> argsPosible = new Dictionary<String, VaildArguments>();
    private static CmdArgs instances = null;
    private Boolean isSetArguments = false;

    private CmdArgs()
    {
    }

    /// <summary>
    /// Gibt eine Instanz der Klasse zurück
    /// </summary>
    /// <returns>Klasse</returns>
    public static CmdArgs Instance
    {
      get {
        if (instances == null) {
          instances = new CmdArgs();
        }
        return instances;
      }
    }

    /// <summary>
    /// Übernimmt die Argumente für die Klasse
    /// </summary>
    /// <param name="arguments">Mögliche Komandozeilenargumente</param>
    /// <param name="args">Tatsächliche Komandozeilenargumente</param>
    public void SetArguments(Dictionary<String, VaildArguments> arguments, String[] args)
    {
      this.args = args;
      if (!this.isSetArguments) {
        this.isSetArguments = true;
        this.argsPosible = arguments;
        this.Init();
      }
    }

    private void Init()
    {
      this.argList = new List<ArgTouple>();
      for (Int32 i = 0; i < this.args.Length; i++) {
        if (this.argsPosible.Keys.Contains(this.args[i])) {
          ArgTouple arg = new ArgTouple(this.args[i]);
          if (this.argsPosible[this.args[i]].Length == ArgLength.Touple) {
            if (this.args.Length > i + 1) {
              arg.SetData(this.args[++i]);
            } else {
              Console.WriteLine(this.GetUsageList(""));
              throw new ArgumentException("Argument: "+this.args[i]+" missing second argument.");
            }
          }
          this.argList.Add(arg);
        }
      }
      foreach(KeyValuePair<String, VaildArguments> item in this.argsPosible) {
        if(!this.HasArgumentType(item.Key) && item.Value.Length == ArgLength.Touple && item.Value.Default != "") {
          this.argList.Add(new ArgTouple(item.Key, item.Value.Default));
        }
      }
    }

    /// <summary>
    /// Menge der angegebenen Komandozeilen-Argumente
    /// </summary>
    /// <returns>Menge</returns>
    public Int32 GetArgsLength() => this.argList.Count;

    /// <summary>
    /// Gibt zurück ob ein Argument angegeben wurde
    /// </summary>
    /// <param name="name">Name des Arguments</param>
    /// <returns>true wenn angegeben</returns>
    public Boolean HasArgumentType(String name)
    {
      foreach (ArgTouple t in this.argList) {
        if (t.Type == name) {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Gibt den Inhalt des angegeben Arguments zurück, nur bei zweiteiligen Argumenten möglich
    /// </summary>
    /// <param name="name">Name des Arguments</param>
    /// <returns>Inhalt des Arguments oder ArgumentNullException</returns>
    public String GetArgumentData(String name)
    {
      foreach (ArgTouple t in this.argList) {
        if (t.Type == name && t.Data != null) {
          return t.Data;
        }
      }
      throw new ArgumentNullException();
    }

    public Boolean HasAllRequiredArguments()
    {
      foreach (KeyValuePair<String, VaildArguments> item in this.argsPosible) {
        if (item.Value.Required && !this.HasArgumentType(item.Key)) {
          return false;
        }
      }
      return true;
    }

    public String GetUsageList(String name)
    {
      String ret = "Usage: " + name + " Parameter\nParameter:\n";
      String req = "";
      String opt = "";
      foreach (KeyValuePair<String, VaildArguments> item in this.argsPosible) {
        if (item.Value.Required) {
          req += item.Key + " " + ((item.Value.Length == ArgLength.Touple) ? (item.Value.Default != "" ? " " + item.Value.Default + "\n" : " [data]\n") : "\n");
          if(item.Value.Description != "") {
            req += "\t" + item.Value.Description + "\n";
          }
        }
      }
      if (req != "") {
        ret += "Benötigte Parameter:\n" + req;
      }
      foreach (KeyValuePair<String, VaildArguments> item in this.argsPosible) {
        if (!item.Value.Required) {
          opt += item.Key + " " + ((item.Value.Length == ArgLength.Touple) ? (item.Value.Default != "" ? " " + item.Value.Default + "\n" : " [data]\n") : "\n");
          if (item.Value.Description != "") {
            opt += "\t" + item.Value.Description + "\n";
          }
        }
      }
      if (opt != "") {
        ret += "Optionale Parameter:\n" + opt;
      }
      return ret;
    }
  }
}
