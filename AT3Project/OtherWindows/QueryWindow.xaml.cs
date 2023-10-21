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
        public QueryWindow()
        {
            InitializeComponent();

            textboxQuery.PreviewKeyDown += textboxQuery_PreviewKeyDown;
        }

        private void buttonRunQuery_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
