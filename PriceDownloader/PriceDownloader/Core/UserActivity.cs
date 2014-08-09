using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace PriceDownloader.Core
{
    class UserActivity
    {
        IWebDriver _wd;

        public UserActivity(IWebDriver wd)
        {
            _wd = wd;
        }
        
        public void GoToUrl(string url, string expectedPageTitle)
        {
            _wd.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(_wd, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.Contains(expectedPageTitle); });
            Reporter.LogInfo("Navigated to page: " + url);
        }

        public void RefreshPage(string expectedPageTitle)
        {
            _wd.Navigate().Refresh();
            WebDriverWait wait = new WebDriverWait(_wd, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.Contains(expectedPageTitle); });
            Reporter.LogInfo("Refresh the page");
        }

        public void SelectOption(string xpath, string valueToSelect)
        {
            var dropDownElement = _wd.FindElement(By.XPath(xpath));
            var selectElement = new SelectElement(dropDownElement);
            selectElement.SelectByText(valueToSelect);
            Reporter.LogInfo("Select item [" + valueToSelect + "] from dropdown element: " + xpath);
        }

        public void Click(string xpath)
        {
            _wd.FindElement(By.XPath(xpath)).Click();
            Reporter.LogInfo("Clicked element: " + xpath);
        }

        public void InputText(string xpath, string textInput)
        {
            _wd.FindElement(By.XPath(xpath)).Click();
            System.Threading.Thread.Sleep(50);
            _wd.FindElement(By.XPath(xpath)).SendKeys("");
            System.Threading.Thread.Sleep(50);
            _wd.FindElement(By.XPath(xpath)).SendKeys(textInput);
            Reporter.LogInfo("Entered value [" + _wd.FindElement(By.XPath(xpath)).GetAttribute("value") + "] into field element: " + xpath);
        }

        public bool IsElementDisplayed(string xpath)
        {
            if (_wd.FindElements(By.XPath(xpath)).Count > 0)
            {
                if (_wd.FindElement(By.XPath(xpath)).Displayed)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
