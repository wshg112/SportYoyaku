using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Sports
{
    public class Reservation
    {
        /// <summary>
        /// 祝日リスト
        /// </summary>
        private string holidays;
        /// <summary>
        /// 対象予約施設IDリスト
        /// </summary>
        private string[] idlist = { "02501", "02601", "02603", "02605", "02607", "02701", "02703", "02901", "02903", "02905", "03001", "03101", "03201", "03301" };
        /// <summary>
        /// 対象リスト作成
        /// </summary>
        private List<string> yoyakuList = new List<string>();
        /// <summary>
        /// 予約可能リスト
        /// </summary>
        private List<string> yoyakuListOK = new List<string>();
        /// <summary>
        /// 一宮市スポーツ予約システム
        /// </summary>
        /// <param name="driver"></param>
        public void SearchGoogle(IWebDriver driver)
        {
            holidays = System.Configuration.ConfigurationManager.AppSettings["holidy"];

            driver.Url = "https://www.yoyaku.city.ichinomiya.aichi.jp/rsv_IN/Core_i/Home/WgR_ModeSelect";
            IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            IWebElement login_btn = driver.FindElement(By.Id("category_04"));
            login_btn.Click();
            //施設選択
            var shisetsuSelectors = driver.FindElements(By.ClassName("switch-off"));
            foreach (var item in shisetsuSelectors)
            {
                // element は IWebElement arguments[0] にそのオブジェクトが設定されている
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", item);
            }

           
            //次のページ
            IWebElement next_btn = driver.FindElement(By.Id("btnNext"));
            next_btn.Click();
            //月選択
            var radioPeriod1month = driver.FindElement(By.Id("radioPeriod1month"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioPeriod1month);
         
            //再表示
            var btnHyoji = driver.FindElement(By.Id("btnHyoji"));
            btnHyoji.Click();

            //直近３っか月分
            for (int i = 0; i < 3; i++)
            {
                GetList(driver);

                IWebElement dpStartDate = driver.FindElement(By.Id("dpStartDate"));
                var now = DateTime.Parse(dpStartDate.GetAttribute("value")).AddMonths(1);
                dpStartDate.Clear();
                dpStartDate.SendKeys(now.ToString("yyyy/M/d"));
                btnHyoji = driver.FindElement(By.Id("btnHyoji"));
                btnHyoji.Click();           
            }
            

        }
        /// <summary>
        /// 予約空きリスト取得
        /// </summary>
        /// <param name="driver"></param>
        public void GetList(IWebDriver driver)
        {
            IWebElement dpStartDate = driver.FindElement(By.Id("dpStartDate"));
            var startDate = DateTime.Parse(dpStartDate.GetAttribute("value"));
            yoyakuList = new List<string>();
            for (int addday = 0; addday <= 31; addday++)
            {
                var now = startDate.AddDays(addday);
                if (now.DayOfWeek == DayOfWeek.Sunday || 
                    now.DayOfWeek == DayOfWeek.Saturday || 
                    holidays.Contains(now.ToString("yyyy-MM-dd")))
                {
                    foreach (var item in idlist)
                    {
                        yoyakuList.Add(now.ToString("yyyyMMdd") + item + "   0");
                    }
                }
            }
        
            var i = 0;
            //List<Checkdate> list = new List<Checkdate>();

            //List<Checkdate> inputlist = new List<Checkdate>();
            List<Tenis> tenis = new List<Tenis>();
            var formhtml = driver.FindElement(By.TagName("form")).GetAttribute("outerHTML");
            //var reg = @"<a\s+[^>]*href\s*=\s*[""'](?<href>[^""']*)[""'][^>]*>(?<text>[^<]*)</a>";
            var reg = @"<td><input\s[^>]*id\s*=\s*[""'](?<id>[^""']*).*value\s*=\s*[""'](?<value>[^""']*).*><label\s*for\s*=\s*[""'](?<for>[^""']*)[""'][^>]*>(?<text>[^<]*)</label></td>";
            var r = new Regex(reg, RegexOptions.IgnoreCase);
            var collection = r.Matches(formhtml);
            foreach (Match m in collection)
            {

                //list.Add(new Checkdate() { Htmlfor = m.Groups["for"].Value.Trim(), InnerText = m.Groups["text"].Value.Trim() });
                // マッチした情報を出力
                //Console.WriteLine($"{m.Groups["text"].Value.Trim()}:{m.Groups["for"]}");
                if (m.Groups["for"].Value.Trim().StartsWith("checkdate") &&
                    (m.Groups["text"].Value.Trim().Equals("△") || m.Groups["text"].Value.Trim().Equals("○")) &&
                    yoyakuList.Contains(m.Groups["value"].Value.Trim()))
                {
                    if (i <= 9)
                    {
                        IWebElement item = driver.FindElement(By.Id(m.Groups["for"].Value.Trim()));

                        // element は IWebElement arguments[0] にそのオブジェクトが設定されている
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", item);
                    }
                    tenis.Add(new Tenis() { id = m.Groups["value"].Value.Trim() });
                    i++;
                }
            }
            if (i > 0)
            {
                GetTimes(driver, tenis);
            }
            

            //var dateSelectors = driver.FindElements(By.ClassName("switch-off"));

            //foreach (var he in dateSelectors)
            //{
            //    list.Add(new Checkdate() { Htmlfor= he.GetAttribute("for") ,InnerText= he.Text });
            //}

            //foreach (var he in list)
            //{

            //    if (he.Htmlfor.StartsWith("checkdate") && (he.InnerText.Equals("△") || he.InnerText.Equals("○")))
            //    {

            //        IWebElement item = driver.FindElement(By.Id(he.Htmlfor));
            //        var val = item.GetAttribute("value");
            //        //Console.WriteLine(DateTime.Now);
            //        //Console.WriteLine(val);
            //        if (yoyakuList.Contains(val))
            //        {
            //            tenis.Add(new Tenis() { id = val });
            //            i++;
            //        }

            //    }

            //}
            //MessageBox.Show(i.ToString());

            string strMsg = String.Format("テニス予約リスト:{0}～{1}", startDate.ToString("yyyy/MM/dd"), startDate.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd")) + "\r\n";

            foreach (var t in tenis)
            {
                strMsg = strMsg + "" + t.name + ":" + t.id.Substring(0, 8) + "\r\n";
                if (t.times != null)
                {
                    foreach (var time in t.times)
                    {
                        strMsg = strMsg + " " + time.name + "番コート:" + time.time + "\r\n";
                    }
                }
            }
            if (tenis.Count > 0)
            {

                sendLine(strMsg);
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["sendflsg"]!=null)
                {
                    sendLine(strMsg + "予約空きなし");
                }

                
            }


        }
        public void GetTimes(IWebDriver driver, List<Tenis> tenisPlace)
        {
            List<Time> times = new List<Time>();
            //javascript:__doPostBack('next','')
            ((IJavaScriptExecutor)driver).ExecuteScript("__doPostBack('next','');");
            var formhtml = driver.FindElement(By.TagName("form")).GetAttribute("outerHTML");

            var reg = @"<td><input\s[^>]*id\s*=\s*[""'](?<id>[^""']*).*value\s*=\s*[""'](?<value>[^""']*).*><label\s*for\s*=\s*[""'](?<for>[^""']*)[""'][^>]*>(?<text>[^<]*)</label></td>";
            var r = new Regex(reg, RegexOptions.IgnoreCase);
            var collection = r.Matches(formhtml);
            foreach (Match m in collection)
            {
                var time = new Time() { id = m.Groups["value"].Value.Trim() };
                var tenis= tenisPlace.Where(r => r.place.Equals(time.place)&&r.day.Equals(time.day)).FirstOrDefault();
                times = tenis.times ?? new List<Time>();
                times.Add(time);
                tenis.times = times;
            }
            //__doPostBack('back','')
            ((IJavaScriptExecutor)driver).ExecuteScript("__doPostBack('back','');");
        }
        /// <summary>
        /// line送信
        /// </summary>
        /// <param name="strMsg"></param>
        public void sendLine(string strMsg)
        {
            try
            {
                //アクセストークン
                //Lineに送信するためのトークン
                var token = System.Configuration.ConfigurationManager.AppSettings["token"]; 
                //送信するメッセージ
                string LINE_url = "https://notify-api.line.me/api/notify";
                Encoding enc = Encoding.UTF8;
                string payload = "message=" + HttpUtility.UrlEncode(strMsg, enc);


                using (WebClient client = new WebClient())
                {
                    client.Encoding = enc;
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.Headers.Add("Authorization", "Bearer " + token);
                    //メッセージ送信
                    client.UploadString(LINE_url, payload);


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
