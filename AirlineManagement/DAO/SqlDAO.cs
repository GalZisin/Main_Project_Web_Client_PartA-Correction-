using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace AirlineManagement
{
    public class SqlDAO
    {
        private System.Data.SqlClient.SqlConnection sqlConnection;
   
        protected System.Data.DataSet DS;
        private bool bError;
        private string sMessage;

        public SqlDAO(string connStr)
        {
            this.sqlConnection = new System.Data.SqlClient.SqlConnection();
            this.sqlConnection.ConnectionString = connStr;
        }
        public bool ErrorFlag
        {
            get
            {
                return bError;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return sMessage;
            }
        }
        /// <summary>
        /// Get Sql Data Reader
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public SqlDataReader GetSqlDataReader(string SQL)
        {
            SqlCommand sqlSelectTableCommand = new SqlCommand();
            sqlSelectTableCommand.CommandTimeout = 6000;
            sqlSelectTableCommand.Connection = sqlConnection;
            sqlSelectTableCommand.CommandText = SQL;
            if (sqlSelectTableCommand.Connection.State != ConnectionState.Open)
                sqlConnection.Open();
            SqlDataReader rdr = sqlSelectTableCommand.ExecuteReader();
            return rdr;
            
        }
        /// <summary>
        /// Get Sql Query DS
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public DataSet GetSqlQueryDS(string SQL,string tbl)
        {
            sMessage = "";
            DataSet DS = new DataSet();
            SqlDataAdapter sqlDATable = new SqlDataAdapter();
            SqlCommand sqlSelectTableCommand = new SqlCommand();
            sqlSelectTableCommand.CommandTimeout = 600;
            sqlSelectTableCommand.Connection = sqlConnection;
            sqlSelectTableCommand.CommandText = SQL;
            sqlDATable.SelectCommand = sqlSelectTableCommand;
            try
            {
                sqlDATable.Fill(DS,tbl);
 
            }
            catch (Exception e1)
            {
                bError = true;
                sMessage = e1.Message.ToString();

            }
            return DS;
        }
        /// <summary>
        /// Execute Sql Scalar Statement
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public string ExecuteSqlScalarStatement(string strSQL)
        {
            string res = "";
            SqlCommand cmds = new SqlCommand(strSQL, sqlConnection);
            cmds.CommandTimeout = 6000;
            try
            {
                if (!(cmds.Connection.State == ConnectionState.Open))
                    cmds.Connection.Open();
                res = cmds.ExecuteScalar().ToString();
            }
            catch(Exception e1)
            {
                bError = true;
                sMessage = e1.Message.ToString();
                res = "";
            }
            finally
            {
                sqlConnection.Close();
            }
            if (cmds.Connection.State == ConnectionState.Open)
                sqlConnection.Close();
            return res;
        }
        /// <summary>
        /// Execute Sql Non Query
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public string ExecuteSqlNonQuery(string strSQL)
        {
            string res = "";
            SqlCommand cmds = new SqlCommand(strSQL, sqlConnection);
            cmds.CommandTimeout = 6000;
            try
            {
                if (!(cmds.Connection.State == ConnectionState.Open))
                    cmds.Connection.Open();
                res = cmds.ExecuteNonQuery().ToString();
            }
            catch (Exception e1)
            {
                bError = true;
                sMessage = e1.Message.ToString();
                res = "";
            }
            finally
            {
                sqlConnection.Close();
            }
            if (cmds.Connection.State == ConnectionState.Open)
                sqlConnection.Close();
            return res;
        }
        /// <summary>
        /// Move tickets expired 3 hours ago
        /// </summary>
        public void MoveTicketsExpired3HoursAgo()
        {
            //DateTime Time3HoursAgo = DateTime.Now.Subtract(DateTime.Now.AddHours(3) - DateTime.Now);
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO FlightsHistory SELECT *");
            sb.Append($"FROM Flights WHERE LANDING_TIME < DATEADD(hour, -3, SYSUTCDATETIME())");
            sb.Append($"AND Flights.ID NOT IN (SELECT ID FROM FlightsHistory)");
            string SQL = sb.ToString();
            ExecuteSqlNonQuery(SQL);
            SQL = $"DELETE FROM Flights WHERE LANDING_TIME < DATEADD(hour, -3, SYSUTCDATETIME())";
            ExecuteSqlNonQuery(SQL);
        }
        /// <summary>
        /// Move flights expired 3 hours ago
        /// </summary>
        public void MoveFlightsExpired3HoursAgo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO TicketHistory SELECT * FROM Tickets ");
            sb.Append($"WHERE FLIGHT_ID IN (SELECT ID FROM Flights WHERE LANDING_TIME < DATEADD(hour, -3, SYSUTCDATETIME()))");
            sb.Append($"AND Tickets.ID NOT IN (SELECT ID FROM TicketHistory)");
            string SQL = sb.ToString();
            ExecuteSqlNonQuery(SQL);
            SQL = $"DELETE FROM Tickets WHERE FLIGHT_ID IN (SELECT ID FROM Flights WHERE LANDING_TIME < DATEADD(hour, -3, SYSUTCDATETIME()))";
            ExecuteSqlNonQuery(SQL);
        }
    }
}
