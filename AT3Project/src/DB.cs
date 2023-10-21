using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT3Project.src
{
    public class DB
    {
        public string DBServer { get; set; }
        public int DBPort { get; set; }
        public string DBUser { get; set; }
        public string DBPass { get; set; }
        public string DBName { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"server={DBServer}; port={DBPort}; user={DBUser}; password={DBPass}; database={DBName}";
            }
        }

        public MySqlConnection Conn
        {
            get
            {
                return new MySqlConnection(ConnectionString);
            }
        }

        public DB(string dbServer, int dbPort, string dbUser, string dbPass, string dbName)
        {
            DBServer = dbServer;
            DBPort = dbPort;
            DBUser = dbUser;
            DBPass = dbPass;
            DBName = dbName;
        }

        public DataTable RunQuery(string sqlQuery)
        {
            MySqlConnection conn = Conn;
            DataTable dt = new();
            try
            {
                conn.Open();
                using (MySqlDataAdapter da = new MySqlDataAdapter(sqlQuery, conn))
                {
                    da.Fill(dt);

                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return dt;
        }

        public DataTable GetTable(string tableName)
        {
            string sqlQuery = $"SELECT * FROM {tableName};";
            return RunQuery(sqlQuery);
        }

        public DataTable GetTableInfo(string tableName)
        {
            return RunQuery($"DESCRIBE {tableName};");
        }

        public DataView GetQueryAsDataView(string sqlQuery)
        {
            return RunQuery(sqlQuery).AsDataView();
        }

        public DataView GetTableAsDataView(string tableName)
        {
            return GetTable(tableName).AsDataView();
        }

        public IEnumerable<MySqlDataReader> GetSQLReader(string sqlQuery)
        {
            MySqlConnection conn = Conn;
            conn.Open();

            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                yield return rdr;
            }

            conn.Close();
        }

        public List<string> GetTableList()
        {
            List<string> outList = new();
            string sqlQuery = "SHOW TABLES;";
            DataTable tempDT = RunQuery(sqlQuery);

            foreach (DataRow dr in tempDT.Rows)
            {
                if (dr[0] != null)
                {
                    outList.Add(dr[0].ToString());
                }
            }

            return outList;
        }

        public List<string> GetTableColumns(string tableName)
        {
            List<string> outList = new();
            var tbl = GetTable(tableName);
            foreach (DataColumn col in tbl.Columns)
            {
                outList.Add(col.ToString());
            }
            return outList;
        }

        public DataColumn? GetColumn(string tableName, string columnName)
        {
            DataTable dataTable = GetTable(tableName);
            foreach (DataColumn col in dataTable.Columns)
            {
                if (col.ColumnName == columnName)
                {
                    return col;
                }
            }
            return null;
        }

        public void RunNonQuery(string nonQuery)
        {
            MySqlConnection conn = Conn;
            try
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(nonQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public List<DataColumn> GetPrimaryKeys(string tableName)
        {
            return GetTable(tableName).PrimaryKey.ToList();
        }
    }
}
