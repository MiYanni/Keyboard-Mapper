using System.Windows;
using CsGoBindingManager.Login;

namespace CsGoBindingManager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            LoginView LoginScreen = new LoginView();
            LoginScreen.Show();

            InitializeComponent();
            this.Hide();
        }
    }
}