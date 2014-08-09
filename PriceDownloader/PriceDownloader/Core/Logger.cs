using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PriceDownloader.Core
{
    static class Reporter
    {
        public static void LogInfo(string message)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + "\t" + message);
        }
    }
}
