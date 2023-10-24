﻿using System;
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

namespace AT3Project.UserControlTables
{
    /// <summary>
    /// Interaction logic for BranchesUserControl.xaml
    /// </summary>
    public partial class BranchesUserControl : UserControl
    {

        public MainWindow RootWindow
        {
            get
            {
                return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            }
        }

        public BranchesUserControl()
        {
            InitializeComponent();
        }

        private void buttonBranchAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonBranchUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonBranchDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonBranchShowAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void datagridBranches_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();

            // Replace all underscores with two underscores, to prevent AccessKey handling
            e.Column.Header = header.Replace("_", "__");

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void datagridBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
