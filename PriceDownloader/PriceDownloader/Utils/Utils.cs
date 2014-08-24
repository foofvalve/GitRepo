using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceDownloader.Utils
{
    public static class CommonUtils
    {
        public static double GetCellDataAsDouble(Dictionary<string, string> recordSet, string columnName)
        {
            if (recordSet == null) return -1;

            string cellValue = "";
            if (recordSet.TryGetValue(columnName, out cellValue))
                return (cellValue=="") ? -1 :Convert.ToDouble(cellValue);
            else
                return -1;
        }

        public static DateTime GetCellDataAsDate(Dictionary<string, string> recordSet, string columnName)
        {
            if (recordSet == null) return DateTime.MinValue;

            string cellValue = "";
            if (recordSet.TryGetValue(columnName, out cellValue))
                return (cellValue == "") ? DateTime.MinValue : Convert.ToDateTime(cellValue);
            else
                return DateTime.MinValue;
        }

        public static string GetCellDataAsString(Dictionary<string, string> recordSet, string columnName)
        {
            if (recordSet == null) return "";

            string cellValue = "";
            if (recordSet.TryGetValue(columnName, out cellValue))
                return (cellValue == "") ? "" : cellValue;
            else
                return "";
        }
    }
}
