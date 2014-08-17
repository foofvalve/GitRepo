using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceDownloader
{
    class StockPrice
    {
        public string code { get; set; }
        public DateTime transcationDate { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public long volume { get; set; }

        public StockPrice() { }

        public override string ToString()
        {
            return String.Join("|", code, transcationDate, open, high, low, close, volume);
        }
    }
}
