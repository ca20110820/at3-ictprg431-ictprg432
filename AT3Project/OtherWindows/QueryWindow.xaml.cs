using Microsoft.Win32;
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

        private void SaveSQLQuery()
        {
            string sqlText = textboxQuery.Text;

            if (string.IsNullOrEmpty(sqlText))
            {
                MessageBox.Show("SQL text is empty.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "sql",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        writer.Write(sqlText);
                    }

                    MessageBox.Show("SQL saved to file successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadSQLQuery()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "sql"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;

                try
                {
                    using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
                    {
                        string sqlText = reader.ReadToEnd();
                        textboxQuery.Text = sqlText;
                    }

                    MessageBox.Show("SQL script loaded successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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
            SaveSQLQuery();
        }

        private void buttonLoadQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadSQLQuery();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textboxQuery.Text = string.Empty;
        }
    }
}
