using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AT3Project.OtherWindows
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {

        MainWindow mainWindow;

        public ConnectionWindow()
        {
            InitializeComponent();

            mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); ;

            try
            {
                if (mainWindow.database == null)
                {
                    Trace.WriteLine("Database in MainWindow is null!");
                    textboxDBServer.Text = "127.0.0.1";
                    textboxDBPort.Text = "3306";
                    textboxDBUser.Text = "root";
                    passwordboxDBPass.Password = "";
                    textboxDBName.Text = "ca_ictprg402";

                    mainWindow.database = new("127.0.0.1", 3306, "root", "", "ca_ictprg402");  // Default Settings
                    Close();
                }
                else
                {
                    passwordboxDBPass.Password = string.IsNullOrEmpty(mainWindow.database.DBPass) ? "" : mainWindow.database.DBPass;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warn");
            }
            finally
            {
                DataContext = mainWindow.database;
            }
        }

        private void buttonSettingConnectionSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.database = new(textboxDBServer.Text, int.Parse(textboxDBPort.Text), textboxDBUser.Text, string.IsNullOrEmpty(passwordboxDBPass.Password) ? "" : passwordboxDBPass.Password, textboxDBName.Text);
                MessageBox.Show("Connection settings has been successfully configured");
                Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "Error");
            }
        }

        private void buttonSettingConnectionTestConnection_Click(object sender, RoutedEventArgs e)
        {
            // Test Connection
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            try
            {
                bool connectionResult = mainWindow.Init();
                if (connectionResult)
                {
                    MessageBox.Show("Connection Success!");
                }
                else
                {
                    MessageBox.Show("Connection Failed!");
                }
            }
            catch(Exception error)
            {
                Trace.WriteLine(error.ToString());
                MessageBox.Show("Connection Failed!");
            }
        }
    }
}
