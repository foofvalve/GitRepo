using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PriceDownloader.Core;

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
        }

        public void DoLogin(string username, string password)
        {           
            _userActivity.InputText(txtclientId, username);
            _userActivity.InputText(txtpassword, password);
            _userActivity.Click(btnLogin);
        }
    }

}