using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceDownloader.Data;
using PriceDownloader.Core;
using PriceDownloader.Utils;

namespace PriceDownloader
{
    class StockGeniusAnalyzer
    {
        DataProvider _dataProvider;
        List<string> _asxCodes;

        public enum ChartPattern
        {
            BearishBeltHold,
            BearishEngulfing,
            BearishHarami,
            BearishHaramiCross,
            BullishBeltHold,
            BullishEngulfing,
            BullishHarami,
            BullishHaramiCross,
            DarkCloudCover,
            Doji,
            DownsideTasukiGap,
            EveningStar,
            FallingThreeMethods,
            Hammer,
            HangingMan,
            InvertedHammer,
            MorningStar,
            PiercingLine,
            RisingThreeMethods,
            ShootingStar,
            StickSandwich,
            ThreeBlackCrows,
            ThreeWhiteSoldiers,
            UpsideGapTwoCrows,
            UpsideTasukiGap,
        }

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
                var maxHighEvent = MaxHigh(stock, Convert.ToDateTime("2014-05-01"), Convert.ToDateTime("2014-07-31"));
                double maxHighPrice = CommonUtils.GetCellDataAsDouble(maxHighEvent, "maximumHigh");
                string maxHighEventDate = CommonUtils.GetCellDataAsString(maxHighEvent, "highDate"); 
                double closingPrice = GetClosingPrice(stock, Convert.ToDateTime("2014-01-01"), Convert.ToDateTime("2014-07-31"));
                
                if (maxHighPrice > closingPrice 
                    && (( closingPrice / maxHighPrice)) > 0.98
                    && maxHighEventDate.Contains("30/07/2014"))
                {
                    var percentageDiff = closingPrice / maxHighPrice;
                    Reporter.LogInfo(string.Format("{0} Event Date {1}  MaxHigh : {2} Close : {3} Diff: {4}", stock, maxHighEventDate, maxHighPrice, closingPrice, percentageDiff));
                }
            }  
        }

        public Dictionary<string,string> MaxHigh(string code, DateTime from, DateTime to)
        {
            string qry = "SELECT high as maximumHigh,date as highDate FROM algo.prices" +
                            " where date >= '" + from.ToString("yyyy-MM-dd") + "' and date <'" + to.ToString("yyyy-MM-dd") + "'" +
                            " and code = '"+code+"'" + 
                            " order by high desc limit 1";
            return _dataProvider.RunQueryReturnSingleRow(qry);
        }

        public double GetClosingPrice(string code, DateTime from, DateTime to)
        {
            string qry = "SELECT close FROM algo.prices" +
                            " where date >= '" + from.ToString("yyyy-MM-dd") + "' and date <'" + to.ToString("yyyy-MM-dd") + "'" +
                            " and code = '"+code+"' " + 
                            " order by date desc " + 
                            "limit 1";
            var qryResults = _dataProvider.RunQueryReturnSingleRow(qry);
            return CommonUtils.GetCellDataAsDouble(qryResults, "close");          
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

