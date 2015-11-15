using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public class VaildArguments
        {
            public VaildArguments(ArgLength length, bool required)
            {
                this.Required = required;
                this.Length = length;
            }
            public VaildArguments(ArgLength length)
            {
                this.Required = false;
                this.Length = length;
            }

            public ArgLength Length { get; private set; }
            public bool Required { get; private set; }
        }
        private class ArgTouple
        {
            public ArgTouple(string type, string data)
            {
                this.type = type;
                this.data = data;
            }
            public ArgTouple(string type)
            {
                this.type = type;
            }
            public string type { get; private set; }
            public string data { get; private set; }

            internal void setData(string data)
            {
                if (data != "")
                    this.data = data;
            }
        }
        #endregion
        private string[] args;
        private List<ArgTouple> argList;
        private Dictionary<string, VaildArguments> argsPosible = new Dictionary<string, VaildArguments>();
        private static CmdArgs instances = null;
        private bool isSetArguments = false;

        private CmdArgs()
        {
        }

        /// <summary>
        /// Gibt eine Instanz der Klasse zurück
        /// </summary>
        /// <returns>Klasse</returns>
        public static CmdArgs getInstance()
        {
            if (instances == null)
            {
                instances = new CmdArgs();
            }
            return instances;
        }

        /// <summary>
        /// Übernimmt die Argumente für die Klasse
        /// </summary>
        /// <param name="arguments">Mögliche Komandozeilenargumente</param>
        /// <param name="args">Tatsächliche Komandozeilenargumente</param>
        public void setArguments(Dictionary<string, VaildArguments> arguments, string[] args)
        {
            this.args = args;
            if (!this.isSetArguments)
            {
                this.isSetArguments = true;
                this.argsPosible = arguments;
                this.Init();
            }
        }

        private void Init()
        {
            this.argList = new List<ArgTouple>();
            for (int i = 0; i < this.args.Length; i++)
            {
                if (this.argsPosible.Keys.Contains(args[i]))
                {
                    ArgTouple arg = new ArgTouple(args[i]);
                    if (argsPosible[args[i]].Length == ArgLength.Touple)
                    {
                        if (args.Length > i + 1)
                        {
                            arg.setData(args[++i]);
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                    this.argList.Add(arg);
                }
            }
        }

        /// <summary>
        /// Menge der angegebenen Komandozeilen-Argumente
        /// </summary>
        /// <returns>Menge</returns>
        public int GetArgsLength()
        {
            return this.argList.Count;
        }

        /// <summary>
        /// Gibt zurück ob ein Argument angegeben wurde
        /// </summary>
        /// <param name="name">Name des Arguments</param>
        /// <returns>true wenn angegeben</returns>
        public bool HasArgumentType(string name)
        {
            foreach (ArgTouple t in this.argList)
            {
                if (t.type == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gibt den Inhalt des angegeben Arguments zurück, nur bei zweiteiligen Argumenten möglich
        /// </summary>
        /// <param name="name">Name des Arguments</param>
        /// <returns>Inhalt des Arguments oder ArgumentNullException</returns>
        public string GetArgumentData(string name)
        {
            foreach (ArgTouple t in this.argList)
            {
                if (t.type == name && t.data != null)
                    return t.data;
                else
                {
                    throw new ArgumentNullException();
                }
            }
            return null;
        }

        public bool HasAllRequiredArguments()
        {
            foreach (KeyValuePair<string, VaildArguments> item in this.argsPosible)
            {
                if (item.Value.Required && !this.HasArgumentType(item.Key))
                {
                    return false;
                }
            }
            return true;
        }

        public string getUsageList(string name)
        {
            string ret = "Usage: "+name+" Parameter\nParameter:\n";
            string req ="";
            string opt = "";
            foreach (KeyValuePair<string, VaildArguments> item in this.argsPosible)
            {
                if (item.Value.Required)
                {
                    req += item.Key+" "+((item.Value.Length == ArgLength.Touple)?" [data]\n":"\n");
                }
            }
            if (req != "")
            {
                ret += "Benötigte Parameter:\n" + req;
            }
            foreach (KeyValuePair<string, VaildArguments> item in this.argsPosible)
            {
                if (!item.Value.Required)
                {
                    opt += item.Key + " " + ((item.Value.Length == ArgLength.Touple) ? " [data]\n" : "\n");
                }
            }
            if (opt != "")
            {
                ret += "Optionale Parameter:\n" + opt;
            }
            return ret;
        }
    }
}
