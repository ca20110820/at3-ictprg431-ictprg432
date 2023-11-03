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
using System.Windows.Shapes;

namespace AT3Project.OtherWindows
{
    /// <summary>
    /// Interaction logic for QueryWindow.xaml
    /// </summary>
    public partial class QueryWindow : Window
    {

        MainWindow mainWindow;

        public QueryWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            textboxQuery.PreviewKeyDown += textboxQuery_PreviewKeyDown;
        }

        private void buttonRunQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlQuery = @$"{textboxQuery.Text}";
                datagridQuery.ItemsSource = null;
                datagridQuery.ItemsSource = mainWindow.database.GetQueryAsDataView(sqlQuery);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void textboxQuery_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                // Prevent the default behavior of adding a tab character
                e.Handled = true;

                // Insert 4 spaces instead of a tab character
                int caretIndex = textboxQuery.CaretIndex;
                textboxQuery.Text = textboxQuery.Text.Insert(caretIndex, "    ");
                textboxQuery.CaretIndex = caretIndex + 4;
            }
        }

        private void datagridQuery_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();

            // Replace all underscores with two underscores, to prevent AccessKey handling
            e.Column.Header = header.Replace("_", "__");

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void buttonSaveQuery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonLoadQuery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
