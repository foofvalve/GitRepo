using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PriceDownloader.Core
{
    static class Reporter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
     (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string message)
        {
            log.Info(message);
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + "\t " + message);
        }
    }
}
