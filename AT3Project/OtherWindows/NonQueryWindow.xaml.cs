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
using System.Windows.Shapes;

namespace AT3Project.OtherWindows
{
    /// <summary>
    /// Interaction logic for NonQueryWindow.xaml
    /// </summary>
    public partial class NonQueryWindow : Window
    {

        MainWindow mainWindow;
        
        public NonQueryWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            textboxNonQuery.PreviewKeyDown += textboxNonQuery_PreviewKeyDown;

            UpdateTableNamesListView();
        }

        private DataView? GetTableNameAsView(string tableName)
        {
            try
            {
                return mainWindow.database.GetTableAsDataView(tableName);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return null;
            }
        }

        private List<string> GetTableNames()
        {
            List<string> outList = new();

            // Get Tables, exclude SQL Views
            string sqlQuery = @$"SELECT table_name FROM information_schema.tables WHERE table_schema = '{mainWindow.database.DBName}' AND table_type = 'BASE TABLE';";

            DataView dv = mainWindow.database.GetQueryAsDataView(sqlQuery);

            foreach(DataRowView row in dv)
            {
                try
                {
                    outList.Add(row[0].ToString());
                }
                catch(Exception error)
                {
                    Trace.WriteLine(error.ToString());
                }
            }

            return outList;
        }

        private void UpdateTableNamesListView()
        {
            listviewTables.ItemsSource = null;
            listviewTables.ItemsSource = GetTableNames();
        }

        private void buttonRunNonQuery_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                string sqlNonQuery = @$"{textboxNonQuery.Text}";
                mainWindow.database.RunNonQuery(sqlNonQuery);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void textboxNonQuery_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                // Prevent the default behavior of adding a tab character
                e.Handled = true;

                // Insert 4 spaces instead of a tab character
                int caretIndex = textboxNonQuery.CaretIndex;
                textboxNonQuery.Text = textboxNonQuery.Text.Insert(caretIndex, "    ");
                textboxNonQuery.CaretIndex = caretIndex + 4;
            }
        }

        private void listviewTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateTableNamesListView();
        }
    }
}
