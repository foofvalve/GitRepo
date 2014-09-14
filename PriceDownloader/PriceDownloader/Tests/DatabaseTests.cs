using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceDownloader.Data;
using System.IO;

namespace PriceDownloader.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        List<string> _asxCodes;

        [TestMethod]
        public void LoopThroughDirectory()
        {
            DataProvider dataProvider = new DataProvider();
            var directoryPath = @"C:\temp\Data";

            string[] files = Directory.GetFiles(directoryPath, "*.csv", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string[] allLines = File.ReadAllLines(files[i]);
                string sqlString = "";
                foreach (string line in allLines)
                {
                    string[] priceComponents = line.Split(',');
                    if (!priceComponents[0].Equals("Security Code"))
                    {
                        sqlString += dataProvider.GenerateInsertStatement(priceComponents[0].ToString(), priceComponents[1].ToString(), priceComponents[2].ToString(), priceComponents[3].ToString(), priceComponents[4].ToString(), priceComponents[5].ToString(), priceComponents[6].ToString());
                    }
                }
                Console.WriteLine(sqlString);
                dataProvider.RunSqlStatement(sqlString);
            }
        }

        //[TestMethod]
        public void ReadRecordsFromDb()
        {
            DataProvider dataProvider = new DataProvider();
            var records = dataProvider.RunQuery("SELECT * FROM algo.prices where code='BHP'");
        }

        //[TestMethod]
        public void GetMaxHighUsingDateRange()
        {
            StockGeniusAnalyzer genius = new StockGeniusAnalyzer();
            genius.FindStockNearingDayHigh();
        }

        [TestMethod]
        public void InsertPreviousDays()
        {
            DataProvider dataProvider = new DataProvider();
            var asxCodes = dataProvider.GetAsxCodes();
            foreach (var asxCode in asxCodes)
                dataProvider.InsertPreviousDayData(asxCode);
        }

        //[TestMethod]
        public void ImportCSV()
        {
            DataProvider dataProvider = new DataProvider();

            dataProvider.GenerateInsertStatement("abc", "2014-08-08 00:00:00", "5.10", "5.12", "4.3", "4.8", "45330202");

        }
               

        [TestCleanup]
        public void Teardown()
        {
            
        }

    }
}
