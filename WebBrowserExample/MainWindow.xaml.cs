using System;
using System.Collections.Generic;
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
using System.IO;

namespace WebBrowserExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DiagnosticServer.Http.DiagnosticServer server;

        public MainWindow()
        {
            InitializeComponent();
            //this.Loaded += MainWindow_Loaded;
            //App.Current.Exit += Current_Exit;
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            server.Shutdown();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader("Resources/heartbeat.html");
            using (reader)
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            server = new DiagnosticServer.Http.DiagnosticServer(8080);
            server.Listen();
        }
    }
}
