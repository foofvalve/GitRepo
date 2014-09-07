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
    public class Login
    {
        string txtclientId = "//input[@id='txt-clientId']";
        string txtpassword = "//input[@id='password-field']";
        string btnLogin = "//button[@id='btn-login']";
        IWebDriver webDriver;
        UserActivity _userActivity;
        
        public Login(IWebDriver wd)
        {
            webDriver = wd;
            _userActivity = new UserActivity(wd);
            wd.Manage().Window.Maximize();
        }

        public void DoLogin()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\temp\stuff.txt");

            _userActivity.InputText(txtclientId, (Convert.ToInt32(lines[0]) * 5).ToString());
            _userActivity.InputText(txtpassword, "Qtp" + (Convert.ToInt32(lines[1]) * 5).ToString());
            _userActivity.Click(btnLogin);
        }      
    }
}