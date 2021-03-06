﻿using System;
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
            return "INSERT INTO `algo`.`prices` (`code`, `date`, `open`, `high`, `low`, `close`, `volume`, `previous_open`, `previous_high`, `previous_low`, `previous_close`, `previous_volume`, `symbol`) " +
                        string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','0','0','0','0','0','{7}');", code, formattedDate, open, high, low, close, volume, code) + Environment.NewLine;


        }
        //Doji

        /*
        SELECT * 
        FROM algo.prices b
        where b.open=b.close 
        and b.date='2014-05-02 00:00:00' and all_ords='Y'
        and b.previous_open<>0
        and b.volume >50000
        and b.high >b.close

         * */

        //Bullish Harami
        /* 
            SELECT * 
            FROM algo.prices b
            where b.date='2014-09-12 00:00:00' 
            and b.volume >50000
            and b.previous_open <> 0
            and b.previous_close < b.previous_open
            and b.close > b.open
            and b.open > b.previous_close
            and b.close < b.previous_open
            and b.high < b.previous_open
            and b.low > b.previous_close
         * **/

        public void InsertPreviousDayData(string code)
        {
            var dateRange = "date<='2014-09-12 00:00:00' and date>='2014-09-08 00:00:00' and ";
            var records = RunQuery("SELECT * FROM algo.prices where " + dateRange + " code='" + code + "' order by date").ToArray();
            string sqlToRun = "";
            for (int i = 1; i < records.Length ; i++)
            {
                sqlToRun = sqlToRun + Environment.NewLine + GenerateUpdateSQLPrevDayData(records[i].code,
                                                                            records[i].transcationDate,
                                                                            records[i - 1].open,
                                                                            records[i - 1].high,
                                                                            records[i - 1].low,
                                                                            records[i - 1].close,
                                                                            records[i - 1].volume);
            }
            RunSqlStatement(sqlToRun);            
        }

        public string GenerateUpdateSQLPrevDayData(string code, DateTime date,
                                                    double prevOpen,
                                                    double prevHigh,
                                                    double prevLow,
                                                    double prevClose,
                                                    double prevVol)
        {
            var formattedDate = date.ToString("yyyyMMddHHmmss");
            return "UPDATE algo.prices " +
                "SET  " +
                "previous_open = " + prevOpen + ",  " +
                "previous_high = " + prevHigh + ",  " +
                "previous_low = " + prevLow + ", " +
                "previous_close = " + prevClose + ", " +
                "previous_volume = " + prevVol + " " +
                "WHERE code = '" + code + "' AND date = '" + formattedDate + "';";
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

        public List<string> GetAsxCodes()
        {
            var qry = "select distinct code from algo.prices where open > 3";
            Reporter.LogInfo("GetAsxCodes ...");
            Reporter.LogInfo(qry);
            List<string> recordSet = new List<string>();
            
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(qry, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                    recordSet.Add(dataReader[0].ToString());      

                dataReader.Close();
                this.CloseConnection();
                return recordSet;
            }
            else
            {
                return recordSet;
            }        
        }
      
        public Dictionary<string, string> RunQueryReturnSingleRow(string query)
        {
            //Reporter.LogInfo("RunQuery ...");
            //Reporter.LogInfo(query);
            Dictionary<string, string> qryResult = new Dictionary<string, string>();            

            if (!this.OpenConnection())
                return null;
            
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
                for (int i=0; i<dataReader.FieldCount;i++)                
                    qryResult.Add(dataReader.GetName(i), dataReader[i].ToString());
       
            dataReader.Close();
            this.CloseConnection();
            return qryResult;      
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
                    //Reporter.LogInfo("Added... " + stockPrice.ToString());
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
