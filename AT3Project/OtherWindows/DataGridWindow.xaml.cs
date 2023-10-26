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
    /// Interaction logic for DataGridWindow.xaml
    /// </summary>
    public partial class DataGridWindow : Window
    {

        /// <summary>
        /// Callback for when DataGrid Selection Changed. This require to be binded outside.
        /// </summary>
        public Action? callback { get; set; }

        public DataGridWindow(DataView dv, string title, int height = 400, int width = 400, bool AutoSize = false, Action? callback = null)
        {
            InitializeComponent();

            this.callback = callback;

            Height = height;
            Width = width;

            if (AutoSize)
                SizeToContent = SizeToContent.WidthAndHeight;

            Title = title;
            datagridPresentation.ItemsSource = dv;
        }

        private void datagridPresentation_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();

            // Replace all underscores with two underscores, to prevent AccessKey handling
            e.Column.Header = header.Replace("_", "__");

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void datagridPresentation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)datagridPresentation.SelectedItem;
            Trace.WriteLine(selectedRow);


            // Execute Callback
            if (callback != null)
            {
                callback();
            }
        }
    }
}
