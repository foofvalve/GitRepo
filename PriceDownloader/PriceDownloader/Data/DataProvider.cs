using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PriceDownloader.Core;

namespace PriceDownloader.Data
{
    class DataProvider
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
      
        public DataProvider()
        {
            Initialize();
        }
              
        // http://www.codeproject.com/Articles/43438/Connect-C-to-MySQL
        private void Initialize()
        {
            server = "localhost";
            database = "algo";
            uid = "root";
            password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
		    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
               
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Reporter.LogInfo("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Reporter.LogInfo("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

       
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Reporter.LogInfo(ex.Message);
                return false;
            }
        }


        public string GenerateInsertStatement(string code, string date, string open, string high, string low, string close, string volume)
        {
            var formattedDate = Convert.ToDateTime(date).ToString("yyyyMMddHHmmss");
            return "INSERT INTO `algo`.`prices` (`code`, `date`, `open`, `high`, `low`, `close`, `volume`) " +
                        string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');", code, formattedDate, open, high, low, close, volume);


        }

        public void RunSqlStatement(string sqlStatement)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sqlStatement, connection);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Reporter.LogInfo("RunSqlStatement in db failed: " + e.Message);
                }
                finally
                {
                    this.CloseConnection();
                }
            }
        }

        public List<StockPrice> RunQuery(string query)
        {
            Reporter.LogInfo("RunQuery ...");
            Reporter.LogInfo(query);

            List<StockPrice> recordSet = new List<StockPrice>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    StockPrice stockPrice = new StockPrice
                    {
                        code = dataReader[0].ToString(),
                        transcationDate = Convert.ToDateTime(dataReader[1]),
                        open = Convert.ToDouble(dataReader[2]),
                        high = Convert.ToDouble(dataReader[3]),
                        low = Convert.ToDouble(dataReader[4]),
                        close = Convert.ToDouble(dataReader[5]),
                        volume = Convert.ToInt64(dataReader[6])
                    };
                    recordSet.Add(stockPrice);
                    Reporter.LogInfo("Added... " + stockPrice.ToString());
                }

                dataReader.Close();
                this.CloseConnection();
                return recordSet;
            }
            else
            {
                return recordSet;
            }
        }      
    }
}
