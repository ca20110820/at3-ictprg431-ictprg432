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
        }

        private void buttonRunNonQuery_Click(object sender, RoutedEventArgs e)
        {


            try
            {

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
    }
}
