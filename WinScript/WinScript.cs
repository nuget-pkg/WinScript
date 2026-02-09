namespace Global
{
    using Microsoft.ClearScript;
    using Microsoft.ClearScript.Windows;
    using System;
    using System.Reflection;
    using Global;
    using static Global.EasyObject;
    public class WinScript : JScriptEngine
    {
        public WinScript()
        {
            WinScript engine = this;
            // expose a host type
            engine.AddHostType("Console", typeof(Console));
            // expose entire assemblies
            engine.AddHostObject("lib", new HostTypeCollection("mscorlib", "System.Core", "EasyObject"));
            //
            engine.AddHostObject("_globals", new WintScriptGlobal());
            engine.AddHostObject("console", new WinScriptConsole());
            engine.Execute("""
            var exports = {};
            function $echo(x, title) { _globals.echo(x, title); }
            function $log(x, title) { _globals.log(x, title); }
            //$array = _globals.array;
                        //var $getenv = _globals.getenv;

            """);
        }
    }
    public class WintScriptGlobal
    {
        public dynamic array(params object[] args)
        {
            return NewArray(args);
        }
        public void echo(params object[] args)
        {
            var x = args[0];
            string? title = null;
            if (args.Length > 1)
            {
                //Echo(FullName(args[1]));
                title = args[1].ToString();
                if (title == "[undefined]") title = null;
            }
            Echo(x, title);
        }
        public void log(params object[] args)
        {
            var x = args[0];
            string? title = null;
            if (args.Length > 1)
            {
                //Echo(FullName(args[1]));
                title = args[1].ToString();
                if (title == "[undefined]") title = null;
            }
            Log(x, title);
        }
        public string? getenv(string name)
        {
            return System.Environment.GetEnvironmentVariable(name);
        }
    }
    public class WinScriptConsole
    {
        private void output(string methodName, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Echo(args[i], $"console.{methodName}(#{i + 1})");
            }
        }
        public void debug(params object[] args)
        {
            output("debug", args);
        }
        public void error(params object[] args)
        {
            output("error", args);
        }
        public void info(params object[] args)
        {
            output("info", args);
        }
        public void log(params object[] args)
        {
            output("log", args);
        }
        public void warn(params object[] args)
        {
            output("warn", args);
        }
    }
}
