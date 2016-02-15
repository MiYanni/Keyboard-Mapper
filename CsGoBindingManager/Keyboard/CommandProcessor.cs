using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsGoBindingManager.Keyboard
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
            { "SPACE", "Spacebar" },
            { "INS", "Insert" },
            { "HOME", "Home" },
            { "PGUP", "PageUp" },
            { "DEL", "Delete" },
            { "END", "End" },
            { "PGDN", "PageDown" },
            { "UPARROW", "UpArrow" },
            { "LEFTARROW", "LeftArrow" },
            { "DOWNARROW", "DownArrow" },
            { "RIGHTARROW", "RightArrow" },
            { "KP_SLASH", "KeypadForwardSlash" },
            { "KP_MULTIPLY", "KeypadAsterix" },
            { "KP_MINUS", "KeypadMinus" },
            { "KP_HOME", "Keypad7" },
            { "KP_UPARROW", "Keypad8" },
            { "KP_PGUP", "Keypad9" },
            { "KP_PLUS", "KeypadPlus" },
            { "KP_LEFTARROW", "Keypad4" },
            { "KP_5", "Keypad5" },
            { "KP_RIGHTARROW", "Keypad6" },
            { "KP_END", "Keypad1" },
            { "KP_DOWNARROW", "Keypad2" },
            { "KP_PGDN", "Keypad3" },
            { "KP_ENTER", "KeypadEnter" },
            { "KP_INS", "Keypad0" },
            { "KP_DEL", "KeypadPeriod" },
            { "MOUSE1", "Mouse1" },
            { "MWHEELUP", "ScrollWheelUp" },
            { "MOUSE3", "Mouse3" },
            { "MOUSE2", "Mouse2" },
            { "MOUSE4", "Mouse4" },
            { "MWHEELDOWN", "ScrollWheelDown" },
            { "MOUSE5", "Mouse5" }
        };

        private static string ConvertKey(string key)
        {
            var keyUpper = key.ToUpper();
            return KeyConversions.ContainsKey(keyUpper) ? KeyConversions[keyUpper] : keyUpper;
        }
    }
}