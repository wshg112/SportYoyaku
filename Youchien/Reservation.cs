using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Yochien
{
    public class Reservation
    {
        String RunTime = "AM"; 
        public Reservation(string time)
        {
            this.RunTime = time;
        }
        /// <summary>
        /// 祝日リスト
        /// </summary>
        private string holidays;
    
        /// <summary>
        /// 一宮市スポーツ予約システム
        /// </summary>
        /// <param name="driver"></param>
        public void SearchGoogle(IWebDriver driver)
        {
            holidays = System.Configuration.ConfigurationManager.AppSettings["holidy"];

            driver.Url = "https://www.kidsinfo.jp/";
            IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            //IWebElement id_login = driver.FindElement(By.Name("id_login"));

            //IWebElement pass_login = driver.FindElement(By.Name("pass_login"));

            //id_login.SendKeys("uciu3848");
            //pass_login.SendKeys("sm3DSD");
            var inputs = driver.FindElements(By.TagName("input"));
            foreach (var item in inputs)
            {
                if (item.GetAttribute("name")== "id_login")
                {
                    item.SendKeys("uciu3848");
                }else if (item.GetAttribute("name") == "pass_login")
                {
                    item.SendKeys("sm3DSD");
                }else if (item.GetAttribute("value") ==  "利用開始" )
                {
                    item.Click();
                    break;
                }
            }
            var links = driver.FindElements(By.TagName("a"));
            foreach (var item in links)
            {
                if (item.Text.Contains("バス位置確認"))
                {
                    item.Click();
                    break;
                }
              
            }

            var busroots = driver.FindElements(By.TagName("a"));
            foreach (var item in busroots)
            {
                if (RunTime == "AM")
                {
                    if (item.GetAttribute("href").Contains("type=go"))
                    {
                        item.Click();
                        break;
                    }
                }else if(RunTime == "PM")
                {
                    if (item.GetAttribute("href").Contains("type=back"))
                    {
                        item.Click();
                        break;
                    }
                }
           
            }
            
            var formhtml = driver.FindElement(By.Id("all")).GetAttribute("innerHTML");
            var brs= formhtml.Split("<br>");
            foreach (var item in brs)
            {
                if (item.Contains("にバスがきています"))
                {
                    PublishhMessage(item);
                }
               
                
               
            }


            //次のページ
       
            

        }
        /// <summary>
        /// line送信
        /// </summary>
        /// <param name="message"></param>
        public static async void PublishhMessage(string message)
        {
            // 発行したアクセストークン
            var ACCESS_TOKEN = System.Configuration.ConfigurationManager.AppSettings["token"];

            using (var client = new HttpClient())
            {
                // 通知するメッセージ
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "message", message },
                });

                // ヘッダーにアクセストークンを追加
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

                // 実行
                var result = await client.PostAsync("https://notify-api.line.me/api/notify", content);
            }
        }


    }
}
