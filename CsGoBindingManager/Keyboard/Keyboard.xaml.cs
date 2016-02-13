using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Binding = CsGoBindingManager.Keyboard.Binding;

namespace CsGoBindingManager.Keyboard
{
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : Window
    {
        public Keyboard()
        {
            InitializeComponent();
            Title += " [" + typeof(App).Assembly.GetName().Version + "]";
            var bindings = CommandProcessor.Read();
            //Console.WriteLine(GetType().GetField("TxtLeftShift").GetValue(this));
            //Console.WriteLine((FindName("Txt") as TextBlock).Text);
            foreach (var binding in bindings)
            {
                var keyName = binding.Key;
                if (keyName == "Shift" || keyName == "Control" || keyName == "Alt")
                {
                    SetKeyText(new Binding { Key = "Left" + keyName, Command = binding.Command });
                    keyName = "Right" + keyName;
                }
                SetKeyText(new Binding { Key = keyName, Command = binding.Command });
            }
        }

        private void SetKeyText(Binding binding)
        {
            var textBlock = FindName("Txt" + binding.Key) as TextBlock;
            if (textBlock == null) return;

            textBlock.Text = binding.Command;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
        }
    }
}