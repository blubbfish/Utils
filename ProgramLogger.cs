﻿using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace BlubbFish.Utils {
  public class ProgramLogger {
    private FileWriter fw;
    private ConsoleWriter stdout;
    private ConsoleWriter errout;
    private String loggerfile;

    public ProgramLogger() {
      this.loggerfile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "output.log";
      this.Init(this.loggerfile);
      this.AttachToFw();
      this.SetOutputs();
    }

    private void SetOutputs() {
      Console.SetOut(this.stdout);
      Console.SetError(this.errout);
    }

    private void Init(String file) {
      if(!this.IsWritable(file)) {
        Console.Error.WriteLine("Cannot write to " + file);
        throw new ArgumentException("Cannot write to " + file);
      }
      this.fw = new FileWriter(file);
      this.stdout = new ConsoleWriter(Console.Out, ConsoleWriterEventArgs.ConsoleType.Info);
      this.errout = new ConsoleWriter(Console.Error, ConsoleWriterEventArgs.ConsoleType.Error);
    }

    private Boolean IsWritable(String filename) {
      FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, filename);
      PermissionSet p = new PermissionSet(PermissionState.None);
      p.AddPermission(writePermission);
      if (!p.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet)) {
        return false;
      }
      try {
        using (FileStream fstream = new FileStream(filename, FileMode.Append))
        using (TextWriter writer = new StreamWriter(fstream)) {
          writer.Write("");
        }
      } catch (UnauthorizedAccessException) {
        return false;
      }
      return true;
    }

    public void SetPath(String file) {
      if(file == null) {
        return;
      }
      if (!this.IsWritable(file)) {
        Console.Error.WriteLine("Cannot write to " + file);
        throw new ArgumentException("Cannot write to " + file);
      }
      this.DisattachToFw();
      this.fw.Close();
      if(new FileInfo(this.loggerfile).Length > 0) {
        File.Move(this.loggerfile, file);
      }
      this.loggerfile = file;
      this.fw = new FileWriter(this.loggerfile);
      this.AttachToFw();
    }

    private void DisattachToFw() {
      this.stdout.WriteEvent -= this.fw.Write;
      this.stdout.WriteLineEvent -= this.fw.WriteLine;
      this.errout.WriteEvent -= this.fw.WriteLine;
      this.errout.WriteLineEvent -= this.fw.WriteLine;
    }
    private void AttachToFw() {
      this.stdout.WriteEvent += this.fw.Write;
      this.stdout.WriteLineEvent += this.fw.WriteLine;
      this.errout.WriteEvent += this.fw.WriteLine;
      this.errout.WriteLineEvent += this.fw.WriteLine;
    }
  }

  internal class FileWriter : StreamWriter {
    private Boolean newline = true;
    public FileWriter(String path) : base(path) {
    }

    public override Encoding Encoding { get { return Encoding.UTF8; } }
    public override Boolean AutoFlush { get { return true; } set { base.AutoFlush = value; } }

    private void Write(String value, TextWriter origstream, ConsoleWriterEventArgs.ConsoleType type) {
      String text = "";
      if (this.newline) {
        text = "[" + DateTime.Now.ToString("o") + "]-" + type.ToString() + ": " + value;
        this.newline = false;
      } else {
        text = value;
      }
      origstream.Write(text);
      base.Write(text);
      base.Flush();
    }

    private void WriteLine(String value, TextWriter origstream, ConsoleWriterEventArgs.ConsoleType type) {
      String text = "[" + DateTime.Now.ToString("o") + "]-" + type.ToString() + ": " + value;
      origstream.WriteLine(text);
      base.WriteLine(text);
      this.newline = true;
      base.Flush();
    }

    internal void Write(Object sender, ConsoleWriterEventArgs e) {
      this.Write(e.Value, e.Writer, e.StreamType);
    }

    internal void WriteLine(Object sender, ConsoleWriterEventArgs e) {
      this.WriteLine(e.Value, e.Writer, e.StreamType);
    }
  }

  internal class ConsoleWriterEventArgs : EventArgs {
    public String Value { get; private set; }
    public TextWriter Writer { get; private set; }
    public ConsoleType StreamType { get; private set; }

    public enum ConsoleType {
      Info,
      Error
    }

    public ConsoleWriterEventArgs(String value, TextWriter writer, ConsoleType type) {
      this.Value = value;
      this.Writer = writer;
      this.StreamType = type;
    }
  }

  internal class ConsoleWriter : TextWriter {
    private readonly TextWriter stream;
    private readonly ConsoleWriterEventArgs.ConsoleType streamtype;

    public ConsoleWriter(TextWriter writer, ConsoleWriterEventArgs.ConsoleType type) {
      this.stream = writer;
      this.streamtype = type;
    }

    public override Encoding Encoding { get { return Encoding.UTF8; } }
    public override void Write(String value) {
      this.WriteEvent?.Invoke(this, new ConsoleWriterEventArgs(value, this.stream, this.streamtype));
      base.Write(value);
    }
    public override void WriteLine(String value) {
      this.WriteLineEvent?.Invoke(this, new ConsoleWriterEventArgs(value, this.stream, this.streamtype));
      base.WriteLine(value);
    }
    public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
    public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;
  }
}