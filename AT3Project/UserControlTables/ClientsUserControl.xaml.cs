using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace AT3Project.UserControlTables
{
    /// <summary>
    /// Interaction logic for ClientsUserControl.xaml
    /// </summary>
    public partial class ClientsUserControl : UserControl
    {
        public MainWindow RootWindow
        {
            get
            {
                return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            }
        }

        public ClientsUserControl()
        {
            InitializeComponent();
        }

        public void RefreshClientDataContext()
        {
            DataContext = null;
            DataContext = RootWindow.client;
        }

        public void ClearAll()
        {
            ClearClientForms();
            ClearDataGrid();
        }

        public void ClearClientForms()
        {
            textboxClientID.Text = string.Empty;
            textboxClientName.Text = string.Empty;
            comboboxClientBranchID.SelectedItem = null;
        }

        public void ClearDataGrid()
        {
            datagridClients.ItemsSource = null;
        }

        private void datagridClients_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();

            // Replace all underscores with two underscores, to prevent AccessKey handling
            e.Column.Header = header.Replace("_", "__");

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void datagridClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedClient = (DataRowView)datagridClients.SelectedItem;

            if (selectedClient == null) return;

            try
            {
                RootWindow.client.GetClient(int.Parse(selectedClient["id"].ToString()));
                RefreshClientDataContext();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                Trace.WriteLine(error.ToString());
            }
        }

        private void buttonClientAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClientUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClientDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClientShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                datagridClients.ItemsSource = null;
                datagridClients.ItemsSource = RootWindow.client.GetAll();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }
    }
}
