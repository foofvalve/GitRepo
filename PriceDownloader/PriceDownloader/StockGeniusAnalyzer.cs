using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceDownloader.Data;
using PriceDownloader.Core;

namespace PriceDownloader
{
    class StockGeniusAnalyzer
    {
        DataProvider _dataProvider;
        List<string> _asxCodes;

        public StockGeniusAnalyzer()
        {
            _dataProvider = new DataProvider();
            _asxCodes = _dataProvider.GetAsxCodes();
        }

        public List<StockPrice> FindGapUp()
        {
            throw new NotImplementedException();
        }

        public List<StockPrice> FindHigherHighHigerLow()
        {
            throw new NotImplementedException();
        }

        public List<StockPrice> FindStockNearingDayHigh(int period)
        {
            throw new NotImplementedException();
        }

        public void FindStockNearingDayHigh()
        {
            foreach(var stock in _asxCodes)
            {
                double maxHigh = MaxHigh(stock, Convert.ToDateTime("2014-05-01"), Convert.ToDateTime("2014-07-31"));
                double closingPrice = GetClosingPrice(stock, Convert.ToDateTime("2014-05-01"), Convert.ToDateTime("2014-07-31"));
                var diff = closingPrice - maxHigh;
                //if(diff>0)
                    Reporter.LogInfo(string.Format("{0} MaxHigh : {1} Close : {2} Diff {3}", stock, maxHigh, closingPrice, diff));
            }  
        }

        public double MaxHigh(string code, DateTime from, DateTime to)
        {
            string qry = "SELECT max(high) FROM algo.prices" +
                            " where date >= '" + from.ToString("yyyy-MM-dd") + "' and date <'" + to.ToString("yyyy-MM-dd") + "'" +
                            " and code = '"+code+"'";
            var qryResults = _dataProvider.RunQueryReturnSingleCell(qry);

            if (qryResults != "")
                return Convert.ToDouble(qryResults);
            else
                return -1;
        }

        public double GetClosingPrice(string code, DateTime from, DateTime to)
        {
            string qry = "SELECT close FROM algo.prices" +
                            " where date >= '" + from.ToString("yyyy-MM-dd") + "' and date <'" + to.ToString("yyyy-MM-dd") + "'" +
                            " and code = '"+code+"' " + 
                            " order by date desc " + 
                            "limit 1";
            var qryResults = _dataProvider.RunQueryReturnSingleCell(qry);

            if (qryResults != "")
                return Convert.ToDouble(qryResults);
            else
                return -1;
        }

        public List<StockPrice> DetermineBollingerBands(int period)
        {
            throw new NotImplementedException();
        }

        public List<StockPrice> DetermineDayRSI(int period)
        {
            throw new NotImplementedException();
        }

        public List<StockPrice> DetermineMACD(int a, int b, int c)
        {
            throw new NotImplementedException();
        }
        
        public List<StockPrice> DetermineATR(int period)
        {
            throw new NotImplementedException();
        }
    }
}
