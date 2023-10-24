using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AT3Project.src
{
    public abstract class Table
    {
        public virtual string TableName { get; set; }
        protected virtual DB database { get; set; }

        public virtual DataView GetAll()
        {
            return database.GetTableAsDataView(TableName);
        }

        public virtual List<object>? GetColumnUniqueValues(int colIntLoc)
        {
            var uniqueObjects = database.GetTable(TableName)
             .AsEnumerable()
             .Select(row => row[0])
             .Distinct()
             .ToList();

            return uniqueObjects.Count == 0 ? null : uniqueObjects;
        }

        public virtual List<object>? GetColumnUniqueValues(string colName)
        {
            DataTable dt = database.GetTable(TableName);
            DataColumn? dc = dt.Columns[colName];

            if (dc == null)
            {
                return null;
            }

            Type t = dc.DataType;

            //return dt.AsEnumerable().Select(x => x.Field<t>(colName)).Distinct().ToList();
            // https://stackoverflow.com/questions/19509010/need-help-to-get-datacolumn-of-a-datatable
            return dt.AsEnumerable().Select(x => x[colName]).Distinct().ToList();
        }
    }

    public class Employee : Table
    {
        public int ID { get; set; }
        public string? GivenName { get; set; }
        public string FamilyName { get; set;}
        public DateTime DateOfBirth { get; set; }
        public string GenderIdentity { get; set; }
        public double GrossSalary { get; set; }
        public int? SupervisorID { get; set; }
        public int BranchID { get; set; }

        public Employee(DB database, 
            int id, 
            string? givenName, 
            string familyName, 
            DateTime dateOfBirth, 
            string genderIdentity, 
            double grossSalary, 
            int? supervisorID, 
            int branchID
            )
        {
            this.database = database;
            TableName = "employees";

            ID = id;
            GivenName = givenName;
            FamilyName = familyName;
            DateOfBirth = dateOfBirth;
            GenderIdentity = genderIdentity;
            GrossSalary = grossSalary;
            SupervisorID = supervisorID;
            BranchID = branchID;
        }

        public Employee(DB database)
        {
            this.database = database;
            TableName = "employees";
        }

        public void GetEmployee(int id)
        {
            string sqlQuery = $"SELECT * FROM {TableName} WHERE id={id};";
            
            DataTable dt = database.RunQuery(sqlQuery);

            Trace.Assert(dt.Rows.Count == 1, "Invalid Employee ID!");

            foreach (DataRow dtRow in dt.Rows)
            {
                ID = int.Parse(dtRow["id"].ToString());
                GivenName = dtRow["given_name"] == null ? null : dtRow["given_name"].ToString();
                FamilyName = dtRow["family_name"].ToString();
                DateOfBirth = DateTime.Parse(dtRow["date_of_birth"].ToString());
                GenderIdentity = dtRow["gender_identity"].ToString();
                GrossSalary = double.Parse(dtRow["gross_salary"].ToString());

                try
                {
                    SupervisorID = int.Parse(dtRow["supervisor_id"].ToString());
                }
                catch
                {
                    SupervisorID = null;
                }

                BranchID = int.Parse(dtRow["branch_id"].ToString());
            }

        }

        public void AddEmployee(Employee employee)
        {
            string cleanDOB = employee.DateOfBirth.ToString("yyyy-MM-dd");
            string supervisorID = employee.SupervisorID == null ? "NULL" : employee.SupervisorID.ToString();

            string sqlNonQuery = $"INSERT INTO {TableName} (given_name, family_name, date_of_birth, gender_identity, gross_salary, supervisor_id, branch_id) VALUES" +
                $"('{employee.GivenName}','{employee.FamilyName}','{cleanDOB}','{employee.GenderIdentity}',{employee.GrossSalary},{supervisorID}, {employee.BranchID});";

            database.RunNonQuery(sqlNonQuery);
        }

        public void DeleteEmployee(int id)
        {
            string sqlNonQuery = $"DELETE FROM {TableName} WHERE id={id};";
            database.RunNonQuery(sqlNonQuery);
        }

        public void UpdateEmployee(int employeeID, string? newGivenName, string newFamilyName, double newGrossSalary, int? newSupervisorID, int newBranchID)
        {
            string supervisorID = newSupervisorID == null ? "NULL" : newSupervisorID.ToString();

            string sqlNonQuery = $"UPDATE {TableName} " +
                $"SET given_name='{newGivenName}',family_name='{newFamilyName}',gross_salary={newGrossSalary},supervisor_id={supervisorID},branch_id={newBranchID} " +
                $"WHERE id={employeeID};";
            database.RunNonQuery(sqlNonQuery);
        }

        public DataView ShowEmployeesByName(string namePattern)
        {
            string sqlQuery = $"SELECT * FROM {TableName} " +
                $"WHERE given_name LIKE \"%{namePattern}%\" OR family_name LIKE \"%{namePattern}%\"" +
                $";";
            return database.GetQueryAsDataView(sqlQuery);
        }

        public DataView ShowEmployeesFromBranchID(int branchID)
        {
            string sqlQuery = $"SELECT * FROM {TableName} WHERE branch_id={branchID};";
            return database.GetQueryAsDataView(sqlQuery);
        }

        public DataView ShowEmployeesWithMinSalary(double minSalary)
        {
            string sqlQuery = $"SELECT * FROM employees WHERE gross_salary > {minSalary};";
            return database.GetQueryAsDataView(sqlQuery);
        }

        public DataView ShowEmployeeSales(int employeeID)
        {
            string sqlQuery = $"SELECT\r\n\ttbl.client_name,\r\n\tSUM(tbl.total_sales) AS `total_sales`\r\nFROM \r\n\t(\r\n\t\tSELECT \r\n\t\t\tworking_with.*, \r\n\t\t\tCONCAT(employees.given_name, \" \", employees.family_name) AS `Employee`, \r\n\t\t\tclients.client_name\r\n\t\tFROM working_with\r\n\t\tLEFT JOIN employees ON working_with.employee_id=employees.id\r\n\t\tLEFT JOIN clients ON working_with.client_id=clients.id\r\n\t\tWHERE working_with.employee_id={employeeID}\r\n\t) AS tbl\r\nGROUP BY tbl.client_name\r\n;";
            return database.GetQueryAsDataView(sqlQuery);
        }

        public Hashtable GetEmployeeIDNameDictionary()
        {
            Hashtable employeeIDName = new();
            DataTable employeeDT = database.GetTable(TableName);
            for (int i = 0; i < employeeDT.Rows.Count; i++)
            {
                int id = int.Parse(employeeDT.Rows[i]["id"].ToString());
                string name = (string)employeeDT.Rows[i]["given_name"] + " " + (string)employeeDT.Rows[i]["family_name"];
                employeeIDName.Add(id, name);
            }
            return employeeIDName;
        }
    }

    public class Branch : Table
    {
        public int ID { get; set; }
        public string BranchName { get; set; }
        public int ManagerID { get; set; }
        public DateTime ManagerStartedAt { get; set; }

        public Branch(DB database)
        {
            this.database = database;
            TableName = "branches";
        }

        public void AddBranch(Branch branch)
        {
            string cleanDate = branch.ManagerStartedAt.ToString("yyyy-MM-dd");

            string sqlNonQuery = $"INSERT INTO {TableName} (branch_name, manager_id, manager_started_at) VALUES " +
                @$"({branch.BranchName}, {branch.ManagerID}, {cleanDate});";
            
            database.RunNonQuery(sqlNonQuery);
        }

        public void DeleteBranch(int id)
        {
            string sqlNonQuery = $"DELETE FROM {TableName} WHERE id={id};";
            database.RunNonQuery(sqlNonQuery);
        }

        public void UpdateBranch(int branchID, string newBranchName, int newManagerID, DateTime newManagerStartedAt)
        {
            string cleanDate = newManagerStartedAt.ToString("yyyy-MM-dd");

            string sqlNonQuery = $"UPDATE {TableName} " +
                $"SET branch_name={newBranchName}, manager_id={newManagerID}, manager_started_at={cleanDate}" +
                $"WHERE id={branchID};";
            database.RunNonQuery(sqlNonQuery);
        }
    }

    public class Client : Table
    {

    }

    public class BranchSupplier : Table
    {

    }

    public class WorkingWith : Table
    {

    }
}
