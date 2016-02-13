using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LoginModule.Keyboard
{
    public static class CommandProcessor
    {
        public static IEnumerable<Binding> Read(string filePath = "config.cfg")
        {
            var configFile = new FileInfo(filePath);
            var lines = File.ReadAllLines(configFile.ToString());
            //lines.Select(CmdLineToArgvW.SplitArgs).ToList().ForEach(lg => Console.WriteLine(lg.Aggregate((current, next) => current + "|" + next)));
            //return null;
            return lines.Select(CmdLineToArgvW.SplitArgs)
                .Where(g => g[0].ToUpper() == "BIND")
                .Select(g =>
                    {
                        // bind|" fogui
                        var isBackSlash = g[1].StartsWith("\"");
                        var key = isBackSlash ? "\\" : g[1];
                        var command = isBackSlash ? g[1].Split(' ')[1] : g[2];
                        return new Binding { Key = ConvertKey(key), Command = command };
                    }
                );
        }

        private static readonly Dictionary<string, string> KeyConversions = new Dictionary<string, string>
        {
            { "[", "LeftBracket" },
            { "]", "RightBracket" },
            { "'", "SingleQuote" },
            { "`", "Tilde" },
            { ",", "Comma" },
            { ".", "Period" },
            { "/", "ForwardSlash" },
            { "\\", "BackSlash" },
            { "-", "Minus" },
            { "=", "EqualSign" },
            { "TAB", "Tab" },
            { "BACKSPACE", "Backspace" },
            { "CAPSLOCK", "CapsLock" },
            { ";", "SemiColon" },
            { "ENTER", "Enter" },
            { "SHIFT", "Shift" },
            { "CTRL", "Control" },
            { "ALT", "Alt" },
            { "SPACE", "Spacebar" }
        };

        private static string ConvertKey(string key)
        {
            var keyUpper = key.ToUpper();
            return KeyConversions.ContainsKey(keyUpper) ? KeyConversions[keyUpper] : keyUpper;
        }
    }
}