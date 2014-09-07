using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

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

        public static void GetScreenShot(string filename)
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }
            bitmap.Save(@"C:\temp\Screenshots\" + filename + ".bmp");
        }
    }
}
