using System;
using System.IO;
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
using AT3Project.UserControlTables;
using AT3Project.OtherWindows;
using AT3Project.src;
using System.Diagnostics;

namespace AT3Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DB database { get; set; }

        public Employee employee { get; set; }
        public Branch branch { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        public bool Init()
        {
            ConnectionWindow connectionWindow = new();  // Set the Connection Configuration
            try
            {
                // Instantiate Tables
                employee = new(database);
                branch = new(database);

                // Set Properties for Other Windows and/or UserControl's
                usercontrolEmployees.comboboxEmployeeShowByBranchNumber.ItemsSource = branch.GetColumnUniqueValues("id").ConvertAll(x => int.Parse(x.ToString())).ToList();
                usercontrolEmployees.RefreshGenderIdentityComboBox();
                return true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return false;
            }
        }

        private void menuitemFile_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuitemEdit_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                usercontrolEmployees.ClearAll();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void menuitemSettings_Connection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectionWindow connectionWindow = new();
                connectionWindow.Show();
                connectionWindow.Activate();
                connectionWindow.Focus();
                connectionWindow.Topmost = true;
            }
            catch(Exception error)
            {
                Trace.WriteLine(error.Message);
            }
        }

        private void menuitemQueries_Query_Click(object sender, RoutedEventArgs e)
        {
            QueryWindow queryWindow = new QueryWindow(this);
            queryWindow.Show();
        }

        private void menuitemQueries_NonQuery_Click(object sender, RoutedEventArgs e)
        {
            NonQueryWindow nonQueryWindow = new NonQueryWindow(this);
            nonQueryWindow.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuitemSettings_LoadPort1DB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?\n\nClicking yes would result in " +
                    "deletion of a database.", "Load Database Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult != MessageBoxResult.Yes) return;

                string fileContent = File.ReadAllText("./Assets/create_and_load_db.txt");

                string sqlNonQuery = @$"{fileContent}";
                database.RunNonQuery(sqlNonQuery);
            }
            catch (Exception error)
            {
                Trace.WriteLine(new string('=', 100));
                Trace.WriteLine(error.ToString());
                Trace.WriteLine(new string('=', 100));
                Trace.WriteLine(error.Message);
                Trace.WriteLine(new string('=', 100));
                MessageBox.Show(error.Message);
            }
        }
    }
}
