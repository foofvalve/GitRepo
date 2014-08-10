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

     
        public void Insert()
        {
            string query = "INSERT INTO `algo`.`prices` (`code`, `date`, `open`, `high`, `low`, `close`, `volume`) VALUES ('AAAF', '" + Convert.ToDateTime("07 Aug 2014").ToString("yyyyMMddHHmmss") + "', '50.11', '50.11', '50.1', '50.11', '51523');";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
              
        public List<string>[] Select()
        {
            string query = "SELECT * FROM algo.prices";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader[0] + "");
                    System.Diagnostics.Debug.WriteLine("Adding... " + dataReader[0]);
                    
                    list[1].Add(dataReader[1] + "");
                    System.Diagnostics.Debug.WriteLine("Adding... " + dataReader[1]);
                    
                    list[2].Add(dataReader[2] + "");
                    System.Diagnostics.Debug.WriteLine("Adding... " + dataReader[2]);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
    }
}