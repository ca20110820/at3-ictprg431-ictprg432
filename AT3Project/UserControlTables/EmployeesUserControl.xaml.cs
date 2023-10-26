using AT3Project.OtherWindows;
using AT3Project.src;
using System;
using System.Collections;
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
    /// Interaction logic for EmployeesUserControl.xaml
    /// </summary>
    public partial class EmployeesUserControl : UserControl
    {
        public MainWindow RootWindow
        {
            get
            {
                return (MainWindow)Window.GetWindow(this);
            }
        }

        public EmployeesUserControl()
        {
            InitializeComponent();
        }

        private void RefreshGenderIdentityComboBox()
        {
            comboboxEmployeeGenderIdentity.ItemsSource = null;
            comboboxEmployeeGenderIdentity.ItemsSource = new List<string>() { "M", "F", "" };
        }

        private void RefreshEmployeeDataContext()
        {
            DataContext = null;
            DataContext = RootWindow.employee;
        }

        private void RefreshDataGrid()
        {
            try
            {
                datagridEmployees.ItemsSource = null;
                datagridEmployees.ItemsSource = RootWindow.employee.GetAll();
            }
            catch (Exception error)
            {
                Trace.WriteLine(error.ToString());
            }
            
        }

        public void ClearAll()
        {
            ClearEmployeeForms();
            ClearDataGrid();
        }

        public void ClearEmployeeForms()
        {
            datagridEmployees.SelectedItem = null;
            DataContext = null;
            textboxEmployeeBranchID.Text = string.Empty;
            textboxEmployeeGivenName.Text = string.Empty;
            textboxEmployeeFamilyName.Text = string.Empty;
            datepickerEmployeeDOB.SelectedDate = null;
            comboboxEmployeeGenderIdentity.SelectedItem = string.Empty;
            textboxEmployeeGrossSalary.Text = string.Empty;
            textboxEmployeeSupervisorID.Text = string.Empty;
            textboxEmployeeBranchID.Text = string.Empty;
            RefreshGenderIdentityComboBox();
        }

        public void ClearDataGrid()
        {
            datagridEmployees.ItemsSource = null;
        }

        private void datagridEmployees_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();

            // Replace all underscores with two underscores, to prevent AccessKey handling
            e.Column.Header = header.Replace("_", "__");

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void datagridEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)datagridEmployees.SelectedItem;

            if (row == null)
            {
                return;
            }

            try
            {
                RefreshGenderIdentityComboBox();
                RootWindow.employee.GetEmployee(Convert.ToInt32(row["id"]));
                RefreshEmployeeDataContext();
                textboxEmployeeShowSales.Text = row["id"].ToString();
            }
            catch (InvalidCastException error)
            {
                comboboxEmployeeGenderIdentity.SelectedItem = RootWindow.employee.GenderIdentity;
                Trace.WriteLine(error.ToString());
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message + "\n" + error.ToString(), "Error");
            }
           
        }

        private void buttonEmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = RootWindow.database;
                var newEmployee = new Employee(db) {
                    GivenName = textboxEmployeeGivenName.Text,
                    FamilyName = textboxEmployeeFamilyName.Text,
                    DateOfBirth = datepickerEmployeeDOB.SelectedDate.Value,
                    GenderIdentity = (string)comboboxEmployeeGenderIdentity.SelectedItem,
                    GrossSalary = double.Parse(textboxEmployeeGrossSalary.Text),
                    SupervisorID = string.IsNullOrEmpty(textboxEmployeeSupervisorID.Text)? null : int.Parse(textboxEmployeeSupervisorID.Text),
                    BranchID = int.Parse(textboxEmployeeBranchID.Text)
                };
                RootWindow.employee.AddEmployee(newEmployee);
                Trace.WriteLine("Successfully added a new employee!");
                RefreshDataGrid();
                ClearEmployeeForms();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                //return;
            }
        }

        private void buttonEmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedEmployee = (DataRowView)datagridEmployees.SelectedItem;
            if (selectedEmployee == null) return;

            try
            {
                int id = int.Parse(textboxEmployeeID.Text);
                string? newGivenName = textboxEmployeeGivenName.Text;
                string newFamilyName = textboxEmployeeFamilyName.Text;
                double newGrossSalary = double.Parse(textboxEmployeeGrossSalary.Text);
                int? newSupervisorID = string.IsNullOrEmpty(textboxEmployeeSupervisorID.Text)? null : int.Parse(textboxEmployeeSupervisorID.Text);
                int newBranchID = int.Parse(textboxEmployeeBranchID.Text);

                RootWindow.employee.UpdateEmployee(id, newGivenName, newFamilyName, newGrossSalary, newSupervisorID, newBranchID);
                Trace.WriteLine($"Successfully updated employee id={id}!");
                ClearEmployeeForms();
                RefreshDataGrid();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedEmployee = (DataRowView)datagridEmployees.SelectedItem;
            if (selectedEmployee == null) return;

            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    RootWindow.employee.DeleteEmployee(int.Parse(selectedEmployee["id"].ToString()));
                }
                RefreshDataGrid();
                ClearEmployeeForms();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                datagridEmployees.ItemsSource = null;
                datagridEmployees.ItemsSource = RootWindow.employee.GetAll();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeShowByName_Click(object sender, RoutedEventArgs e)
        {
            string namePattern = textboxEmployeeShowByName.Text;
            if (string.IsNullOrEmpty(namePattern))
            {
                MessageBox.Show("Name Pattern cannot be null or empty!", "Warn");
                return;
            }

            ClearEmployeeForms();
            try
            {
                datagridEmployees.ItemsSource = RootWindow.employee.ShowEmployeesByName(namePattern);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeShowByBranchNumber_Click(object sender, RoutedEventArgs e)
        {
            if (comboboxEmployeeShowByBranchNumber.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Branch", "Warb");
                return;
            }
            RefreshGenderIdentityComboBox();

            int branchID;
            bool resultBranchID = int.TryParse(comboboxEmployeeShowByBranchNumber.SelectedItem.ToString(), out branchID);
            if (!resultBranchID)
            {
                MessageBox.Show("Could Not Parse the given Branch ID!", "Error");
                return;
            }

            ClearEmployeeForms();
            try
            {
                datagridEmployees.ItemsSource = RootWindow.employee.ShowEmployeesFromBranchID(branchID);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeShowBySalary_Click(object sender, RoutedEventArgs e)
        {
            double salary;
            bool resultSalary = double.TryParse(textboxEmployeeShowBySalary.Text, out salary);
            if (!resultSalary)
            {
                MessageBox.Show("Could Not Parse the given Salary!", "Error");
                return;
            }
            if (salary < 0)
            {
                MessageBox.Show("Salary cannot be negative!", "Error");
                return;
            }

            ClearEmployeeForms();
            try
            {
                datagridEmployees.ItemsSource = RootWindow.employee.ShowEmployeesWithMinSalary(salary);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void buttonEmployeeShowSales_Click(object sender, RoutedEventArgs e)
        {
            int employeeID;
            bool result = int.TryParse(textboxEmployeeShowSales.Text, out employeeID);

            if (!result)
            {
                MessageBox.Show("Could not parse the given Employee ID for Showing Sales!", "Error");
                return;
            }

            Hashtable ht;
            try
            {
                ht = RootWindow.employee.GetEmployeeIDNameDictionary();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }
            if (!ht.ContainsKey(employeeID))
            {
                MessageBox.Show("The Employee ID provided does not exist!", "Error");
                return;
            }

            string name = ht[employeeID].ToString();
            DataView dv;

            try
            {
                dv = RootWindow.employee.ShowEmployeeSales(employeeID);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            DataGridWindow dtWindow = new DataGridWindow(dv, $"Sales of Employee - ({employeeID}) {name}");
            dtWindow.Show();
        }

        private void buttonEmployeeShowSalesByName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlQuery = $"SELECT\r\n\ttbl.employee_id AS `id`,\r\n\ttbl.Employee,\r\n\tSUM(tbl.total_sales) AS `Total Sales`\r\nFROM \r\n\t(\r\n\t\tSELECT \r\n\t\t\tworking_with.*, \r\n\t\t\tCONCAT(employees.given_name, \" \", employees.family_name) AS `Employee`\r\n\t\tFROM working_with\r\n\t\tLEFT JOIN employees ON working_with.employee_id=employees.id\r\n\t\t-- LEFT JOIN clients ON working_with.client_id=clients.id\r\n\t\t-- WHERE working_with.employee_id={{employeeID}}\r\n\t) AS tbl\r\nGROUP BY tbl.employee_id\r\n;";

                DataView dv = RootWindow.database.GetQueryAsDataView(sqlQuery);

                DataGridWindow datagridWindow = new(dv, "Employee Sales");
                datagridWindow.Show();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }
    }
}
