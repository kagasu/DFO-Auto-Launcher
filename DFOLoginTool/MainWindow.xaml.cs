using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DFOLoginTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(textBox_ID.Text != "" && textBox_Password.Password != "")
            {
                dynamic json = new DynamicJson();
                json.id = textBox_ID.Text;
                json.password = textBox_Password.Password;
                json.proxy = textBox_Proxy.Text;
                File.WriteAllText("data.json", json.ToString(), new UTF8Encoding());
                Window_Initialized(null, null);
            }
            else
            {
                MessageBox.Show("ID or Password is invalid.", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            if (File.Exists("data.json"))
            {
                var loadingWindow = new LoadingWindow();
                loadingWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                loadingWindow.Show();
                this.Close();
            }
        }
    }
}
