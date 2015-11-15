using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlubbFish.Utils
{
    public class InIReader
    {
        private Dictionary<string, Dictionary<string, string>> cont;
        private FileSystemWatcher k = new FileSystemWatcher(Directory.GetCurrentDirectory(), "*.ini");
        private string filename;

        private static Dictionary<string, InIReader> instances = new Dictionary<string, InIReader>();

        private InIReader(string filename)
        {
            this.filename = filename;
            k.Changed += new FileSystemEventHandler(this.readAgain);
            loadFile();
        }

        public static InIReader getInstance(string filename)
        {
            if (!instances.Keys.Contains(filename))
            {
                instances.Add(filename, new InIReader(filename));
            }
            return instances[filename];
        }

        private void readAgain(object sender, EventArgs e)
        {
            this.loadFile();
        }

        private void loadFile()
        {
            this.cont = new Dictionary<string, Dictionary<string, string>>();
            StreamReader file = new StreamReader(this.filename);
            List<String> buf = new List<string>();
            string fline = "";
            while (fline != null)
            {
                fline = file.ReadLine();
                if (fline != null && fline.Length > 0 && fline.Substring(0,1) != ";")
                    buf.Add(fline);
            }
            file.Close();
            Dictionary<string, string> sub = new Dictionary<string, string>();
            string cap = "";
            foreach (string line in buf)
            {
                Match match = Regex.Match(line, @"^\[[a-zA-Z0-9\-_ ]+\]\w*$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (sub.Count != 0 && cap != "")
                    {
                        this.cont.Add(cap, sub);
                    }
                    cap = line;
                    sub = new Dictionary<string, string>();
                }
                else
                {
                    if (line != "" && cap != "")
                    {
                        string key = line.Substring(0, line.IndexOf('='));
                        string value = line.Substring(line.IndexOf('=') + 1);
                        sub.Add(key, value);
                    }
                }
            }
            if (sub.Count != 0 && cap != "")
            {
                this.cont.Add(cap, sub);
            }
        }

        public List<String> getSections()
        {
            return cont.Keys.ToList<String>();
        }

        public String getValue(String section, String key)
        {
            if (!section.StartsWith("["))
            {
                section = "[" + section + "]";
            }
            if (cont.Keys.Contains(section))
            {
                if (cont[section].Keys.Contains(key))
                {
                    return cont[section][key];
                }
            }
            return null;
        }


        public void SetValue(string section, string key, string value)
        {
            if (!section.StartsWith("["))
            {
                section = "[" + section + "]";
            }
            if (cont.Keys.Contains(section))
            {
                if (cont[section].Keys.Contains(key))
                {
                    cont[section][key] = value;
                }
                else
                {
                    cont[section].Add(key, value);
                }
            }
            else
            {
                Dictionary<string, string> sub = new Dictionary<string, string>();
                sub.Add(key, value);
                cont.Add(section, sub);
            }
            this.changed();
        }

        private void changed()
        {
            k.Changed -= null;
            saveSettings();
            loadFile();
            k.Changed += new FileSystemEventHandler(this.readAgain);
        }

        private void saveSettings()
        {
            StreamWriter file = new StreamWriter(this.filename);
            file.BaseStream.SetLength(0);
            file.BaseStream.Flush();
            file.BaseStream.Seek(0, SeekOrigin.Begin);
            foreach (KeyValuePair<string, Dictionary<string, string>> cap in this.cont)
            {
                file.WriteLine(cap.Key);
                foreach (KeyValuePair<string, string> sub in cap.Value)
                {
                    file.WriteLine(sub.Key + "=" + sub.Value);
                }
                file.WriteLine();
            }
            file.Flush();
            file.Close();
        }

        /// <summary>
        /// Fügt eine neue Sektion in der Ini-Datei ein.
        /// </summary>
        /// <param name="name">Sektionsname</param>
        /// <returns>true if added, false if error</returns>
        public bool addSection(string name)
        {
            if (!name.StartsWith("["))
            {
                name = "[" + name + "]";
            }
            if (this.cont.Keys.Contains(name))
            {
                return false;
            }
            this.cont.Add(name, new Dictionary<string, string>());
            this.changed();
            return true;
        }

        /// <summary>
        /// Löscht eine Sektion inklusive Unterpunkte aus der Ini-Datei.
        /// </summary>
        /// <param name="name">Sektionsname</param>
        /// <returns>true if removed, false if error</returns>
        public bool removeSection(string name)
        {
            if (!name.StartsWith("["))
            {
                name = "[" + name + "]";
            }
            if (!this.cont.Keys.Contains(name))
            {
                return false;
            }
            this.cont.Remove(name);
            this.changed();
            return false;
        }
    }
}
