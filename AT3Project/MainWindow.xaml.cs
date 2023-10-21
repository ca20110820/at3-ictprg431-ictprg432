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
using AT3Project.UserControlTables;
using AT3Project.OtherWindows;
using AT3Project.src;


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

            ConnectionWindow connectionWindow = new(this);  // Set the Connection Configuration
            try
            {
                // Instantiate Tables
                employee = new(database);
                branch = new(database);

                // Set Properties for Other Windows and/or UserControl's
                usercontrolEmployees.comboboxEmployeeShowByBranchNumber.ItemsSource = branch.GetColumnUniqueValues("id").ConvertAll(x => int.Parse(x.ToString())).ToList();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
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
            ConnectionWindow connectionWindow = new(this);
            connectionWindow.Show();
        }
    }
}
