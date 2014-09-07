﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using PriceDownloader.Poms;
using PriceDownloader.Core;
using PriceDownloader.Data;
using System.IO;
using System.Diagnostics;

//Get stocks nearing 52 week high
//MACD for C#
//RSI calculator


namespace PriceDownloader.Tests
{
    [TestClass]
    public class PriceDownloaderTests
    {
        IWebDriver webDriver;
        UserActivity _userActivity;
        List<string> _asxCodes;

        [TestInitialize]
        public void SetUp()
        {
            //webDriver = new FirefoxDriver();
            webDriver = new ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application\");
            _userActivity = new UserActivity(webDriver);
            //SetupProperDataSet();
            //SetupSmallDataSet();
        }

        [TestMethod]
        public void TestTakingScreenShot()
        {
            SetupSymbolsForScreenshot();
            _userActivity.GoToUrl("https://www.commsec.com.au/", "CommSec");

            Login login = new Login(webDriver);
            login.DoLogin();

            foreach (var asxCode in _asxCodes)
            {
                _userActivity.GoToUrl("https://www2.commsec.com.au/Private/MarketPrices/Charts/Charts.aspx?stockCode=" + asxCode, "CommSec | Quotes");
                Utils.CommonUtils.GetScreenShot(asxCode);
            }            
        }

    
        //[TestMethod]
        public void RunUITest()
        {
            webDriver.Navigate().GoToUrl("http://www.ranorex.com");
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.Contains("Ranorex"); });
        }

        //[TestMethod]
        public void DownloadEndOfDayPrices()
        {
            _userActivity.GoToUrl("https://www.commsec.com.au/", "CommSec");

            Login login = new Login(webDriver);
            login.DoLogin();

            _userActivity.GoToUrl("https://www2.commsec.com.au/Private/Charts/EndOfDayPrices.aspx", "CommSec | End of Day Prices");
            EndOfDayPrices endOfDayPrices = new EndOfDayPrices(webDriver);

            foreach (var asxCode in _asxCodes)
                endOfDayPrices.DownloadPriceHistory(asxCode);
        }

        //[TestCleanup]
        public void Teardown()
        {
            webDriver.Quit();  
        }

        private void SetupSymbolsForScreenshot()
        {
            _asxCodes = new List<string> { 
            "BHP",
            "WOW",
            "RIO",
             };
        }

        private void SetupSmallDataSet()
        {
            _asxCodes = new List<string> { 
            "AAA",
            "AAC",
            "AAD",
             };
        }

        private void SetupProperDataSet()
        {
            _asxCodes = new List<string> {    
            "SUNKOP",
            "SUNPC",
            "SUNPD",
            "SUNPE",
            "SUR",
            "SVM",
            "SVW",
            "SVWPA",
            "SVY",
            "SWK",
            "SWL",
            "SWM",
            "SWMJOM",
            "SWMKOB",
            "SXA",
            "SXE",
            "SXI",
            "SXL",
            "SXS",
            "SXX",
            "SXY",
            "SYD",
            "SYDKOC",
            "SYDSSR",
            "SYI",
            "SYR",
            "SZG",
            "TAH",
            "TAHHB",
            "TAHSWD",
            "TAM",
            "TAN",
            "TAP",
            "TAS",
            "TAW",
            "TBR",
            "TCL",
            "TCLKOP",
            "TCN",
            "TCQ",
            "TDO",
            "TDX",
            "TEL",
            "TEN",
            "TFC",
            "TGA",
            "TGG",
            "TGP",
            "TGR",
            "TGS",
            "TGZ",
            "THR",
            "THX",
            "TIG",
            "TIS",
            "TIX",
            "TLG",
            "TLGO",
            "TLM",
            "TLS",
            "TLSIOV",
            "TLSISE",
            "TLSISG",
            "TLSJOC",
            "TLSJOH",
            "TLSJOO",
            "TLSKOA",
            "TLSKOJ",
            "TLSKOT",
            "TLSSRU",
            "TLSSSU",
            "TLU",
            "TME",
            "TNE",
            "TNG",
            "TNGO",
            "TNP",
            "TOE",
            "TOF",
            "TOL",
            "TOLIYA",
            "TON",
            "TOP",
            "TOU",
            "TOX",
            "TPAPA",
            "TPC",
            "TPI",
            "TPM",
            "TPN",
            "TRF",
            "TRG",
            "TRO",
            "TRS",
            "TRY",
            "TSE",
            "TSM",
            "TSN",
            "TSV",
            "TTE",
            "TTEO",
            "TTI",
            "TTN",
            "TTS",
            "TTSHA",
            "TUP",
            "TWD",
            "TWE",
            "TWH",
            "TWR",
            "TYK",
            "TZL",
            "TZN",
            "UBI",
            "UCM",
            "UEQ",
            "UGL",
            "UML",
            "UNS",
            "UNV",
            "UNX",
            "UNXO",
            "UOS",
            "URF",
            "USA",
            "USD",
            "UXC",
            "VAF",
            "VAH",
            "VAP",
            "VAR",
            "VAS",
            "VBP",
            "VED",
            "VEI",
            "VELCP",
            "VET",
            "VEU",
            "VGB",
            "VGE",
            "VHL",
            "VHY",
            "VIC",
            "VIE",
            "VLA",
            "VLC",
            "VLW",
            "VMC",
            "VMG",
            "VML",
            "VMS",
            "VMT",
            "VOC",
            "VOR",
            "VRL",
            "VRT",
            "VRX",
            "VSC",
            "VSO",
            "VTG",
            "VTM",
            "VTS",
            "VXL",
            "VXLO",
            "VXR",
            "WAA",
            "WAF",
            "WAM",
            "WAT",
            "WAX",
            "WBA",
            "WBB",
            "WBC",
            "WBCHA",
            "WBCHB",
            "WBCIOV",
            "WBCIYB",
            "WBCIYD",
            "WBCJOK",
            "WBCKOA",
            "WBCKOC",
            "WBCKOE",
            "WBCKOF",
            "WBCKOX",
            "WBCLOG",
            "WBCLOH",
            "WBCLOP",
            "WBCPB",
            "WBCPC",
            "WBCPD",
            "WBCPE",
            "WBCSST",
            "WBCSWB",
            "WBCSWD",
            "WBCXOT",
            "WCB",
            "WCC",
            "WCL",
            "WCN",
            "WCTPA",
            "WDIV",
            "WDR",
            "WDS",
            "WEB",
            "WEC",
            "WEG",
            "WES",
            "WESJOK",
            "WESJOL",
            "WESKOG",
            "WESKOI",
            "WESKOQ",
            "WESKOR",
            "WESKOS",
            "WESKOT",
            "WESLOB",
            "WESLOE",
            "WESLOR",
            "WESLOU",
            "WESSWB",
            "WFD",
            "WFDJOJ",
            "WFE",
            "WHC",
            "WHE",
            "WHF",
            "WHN",
            "WIC",
            "WICO",
            "WIG",
            "WIN",
            "WKT",
            "WLF",
            "WMK",
            "WMN",
            "WOR",
            "WORKOD",
            "WORKOT",
            "WOW",
            "WOWHC",
            "WOWIYB",
            "WOWJOJ",
            "WOWSOE",
            "WOWSWD",
            "WPG",
            "WPL",
            "WPLIOZ",
            "WPLJOK",
            "WPLKOB",
            "WPLKOG",
            "WPLKOL",
            "WPLLOG",
            "WPLSRX",
            "WPLSS3",
            "WPLSSS",
            "WPLSWB",
            "WRG",
            "WSA",
            "WSAKOB",
            "WSR",
            "WTF",
            "WTP",
            "WTR",
            "WVL",
            "WXHG",
            "WXOZ",
            "XAF",
            "XAM",
            "XAO",
            "XAT",
            "XBW",
            "XDI",
            "XDJ",
            "XEC",
            "XEJ",
            "XFJ",
            "XFL",
            "XGD",
            "XHJ",
            "XIJ",
            "XJO",
            "XJOKOL",
            "XJOKON",
            "XJOKOO",
            "XJOKOU",
            "XJOKOX",
            "XJOKRY",
            "XJOLOF",
            "XJOLOP",
            "XJOLOR",
            "XJOQOA",
            "XJOQOG",
            "XJOQOU",
            "XJOQOW",
            "XJOWOG",
            "XJOWOH",
            "XJOWOJ",
            "XJOWOU",
            "XJOWOV",
            "XJOWOX",
            "XJOXOC",
            "XJOXOR",
            "XJOXOT",
            "XJR",
            "XKO",
            "XLD",
            "XMD",
            "XMJ",
            "XMM",
            "XNJ",
            "XNT",
            "XNV",
            "XPJ",
            "XRF",
            "XRO",
            "XSJ",
            "XSO",
            "XST",
            "XTE",
            "XTJ",
            "XTL",
            "XTO",
            "XUJ",
            "XVI",
            "XXJ",
            "YAL",
            "YBR",
            "YDGA",
            "YDIV",
            "YETF",
            "YGEA",
            "YM1JOA",
            "YMAX",
            "YMVA",
            "YMVB",
            "YMVE",
            "YMVR",
            "YMVW",
            "YOW",
            "YOWO",
            "YOZF",
            "YOZR",
            "YQOZ",
            "YRDV",
            "YRR",
            "YRVL",
            "YSFY",
            "YSLF",
            "YSSO",
            "YSTW",
            "YSYI",
            "ZBHWMA",
            "ZBHWMB",
            "ZBHWSC",
            "ZER",
            "ZGCKOA",
            "ZGCKOU",
            "ZGL",
            "ZIP",
            "ZNC",
            "ZNZ",
            "ZRIWMA",
            "ZRL",
            "ZTA"

            };
        }
    }
}
