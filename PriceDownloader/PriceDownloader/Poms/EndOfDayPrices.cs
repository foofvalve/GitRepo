using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PriceDownloader.Core;
using System.IO;

namespace PriceDownloader.Poms
{
    // https://www2.commsec.com.au/Private/Charts/EndOfDayPrices.aspx
    public class EndOfDayPrices
    {
        IWebDriver _wd;
        UserActivity _userActivity;
        const string DOWNLOAD_PATH = @"C:\Users\Ryan\Downloads\";

        string txtAsxCode = "//input[contains(@id,'txtSmartSearch')]"; //ASX code
        string cbxDownloadFormat = "//select[contains(@id,'ddlSingleFormat')]"; //Download Format
        string radTimePeriod = "//input[contains(@id,'SingleTimePeriod_field')]"; //Time period radio button
        string cbxTimePeriod = "//select[contains(@id,'ddlSingleTimePeriod')]"; //Time period drop down
        string btnDownload = "//input[contains(@id,'btnSingleDownload')]";  //Download button

        //Error message displayed when cannot find security
        string errMessage = "//li[@class='error'][contains(text(),'security you have entered does not appear to be listed')]";

        public EndOfDayPrices(IWebDriver wd)
        {
            _wd = wd;
            _userActivity = new UserActivity(wd);
        }

        public void DownloadPriceHistory(string asxCode)
        {
            _userActivity.RefreshPage("CommSec | End of Day Prices");
            _userActivity.InputText(txtAsxCode, asxCode);
            _userActivity.SelectOption(cbxDownloadFormat, "CSV");
            _userActivity.Click(radTimePeriod);
            _userActivity.SelectOption(cbxTimePeriod, "10 Years");
            _userActivity.Click(btnDownload);
            
            if (_userActivity.IsElementDisplayed(errMessage))   //check for error message
                Reporter.LogInfo("!!! " + asxCode + " - Failed to download file, error message displayed");
            else
                WaitForDownloadCompletion(asxCode); //wait for download completion
        }

        public bool WaitForDownloadCompletion(string asxCode)
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var fileToCheck = DOWNLOAD_PATH + "CSV-" + asxCode + ".csv";
                Reporter.LogInfo("Waiting for download file: " + fileToCheck);
                while (stopwatch.Elapsed.Seconds < 20)
                {                  
                    if (!File.Exists(fileToCheck))
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        FileInfo f = new FileInfo(fileToCheck);
                        if (f.Length > 400)
                        {
                            Reporter.LogInfo("*** " + asxCode + " - File downloaded successfully, file size: " + f.Length);
                            return true;
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }

                Reporter.LogInfo("!!! " + asxCode + " - Failed to download file, possibly timed out waiting for download completion");
                return false;
            }
            catch (Exception e)
            {
                Reporter.LogInfo("!!! " + asxCode + "Failed to download file, exception thrown...");
                Reporter.LogInfo("Exception ... " + e.Message);
                return false;
            }
        }
    }
}
