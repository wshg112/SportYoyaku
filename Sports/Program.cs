using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;

using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


namespace Sports
{
    class Program
    {
  
        static void Main(string[] args)
        {
            var startTime=int.Parse(DateTime.Now.ToString("HH"));
            if (startTime >= 0 && startTime < 8)
            {
                return;
            }

            // Chrome
            //var chromeOptions = new ChromeOptions();
            //chromeOptions.PageLoadStrategy = PageLoadStrategy.Default;
            //IWebDriver driver = new EdgeDriver();

            //Edge
            var options = new EdgeOptions();
            options.UseChromium = true;
            var driver = new EdgeDriver(options);
            try
            {
                new Reservation().SearchGoogle(driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                 new Reservation().sendLine("例外発生しました。"+ex.ToString());
            }
            finally
            {
                driver.Quit();               
            }


            //// InternetExplorer
            //IWebDriver ie = new InternetExplorerDriver();
            //SearchGoogle(ie);

            //// FireFox
            //FirefoxBinary firefoxBinary = new FirefoxBinary(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
            //FirefoxProfile firefoxProfile = new FirefoxProfile();
            //IWebDriver firefox = new FirefoxDriver(firefoxBinary, firefoxProfile);
            //SearchGoogle(firefox);
        }
    }
}
