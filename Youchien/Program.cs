//using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;

using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Edge;

namespace Yochien
{
    class Program
    {
  
        static void Main(string[] args)
        {

            var arg = "";
            if (args.Count() > 0) { 
                arg=args[0];
            }

            if (arg != "AM" && arg != "PM" )
            {
                return;
            }
            IWebDriver driver=null;
            // Chrome
            //var chromeOptions = new ChromeOptions();
            //chromeOptions.PageLoadStrategy = PageLoadStrategy.Default;
            //IWebDriver driver = new EdgeDriver();

            //Edge
            var reservation = new Reservation(arg);
            try
            {
                var options = new EdgeOptions();
                //options.UseChromium = true;

                driver = new EdgeDriver(options);
                // = new EdgeDriver(new EdgeDriverService() , options, TimeSpan.FromSeconds(180));
                //var service = EdgeDriverService.CreateDefaultService();
                //service.HideCommandPromptWindow = true;
                //EventFiringWebDriver driver = new EventFiringWebDriver(new EdgeDriver(service));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(3);
                reservation.SearchGoogle(driver);
            }
            catch (Exception ex)
            {
                var errmsg = "例外発生しました。" + ex.ToString();
                try
                {
                    //msedgedriver
                    var ps =
                        System.Diagnostics.Process.GetProcessesByName("msedgedriver").ToList<System.Diagnostics.Process>();
                    //msedgeのプロセスを取得
                    ps.AddRange(System.Diagnostics.Process.GetProcessesByName("msedge").ToList<System.Diagnostics.Process>());


                    foreach (System.Diagnostics.Process p in ps)
                    {

                        //プロセスを強制的に終了させる
                        p.Kill();
                    }
                }
                catch (Exception exps)
                {
                    errmsg = errmsg + exps.ToString();

                }
                Console.WriteLine(errmsg);
                //PublishhMessage(errmsg);
            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                }

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
