using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports
{
    public class Checkdate
    {
        public string InnerText { get; set; }
        public string Htmlfor { get; set; }


    }
    public class Tenis
    {
        private string _id;
        public string id
        {
            set //値をhpに代入する
            {
                this._id = value;
                this.place = value.Substring(8, 3);
                this.day= value.Substring(0, 8);
                if (value.Substring(8, 3).Equals("025"))
                {
                    this.name = "一宮";
                }
                else if (value.Substring(8, 3).Equals("026"))
                {
                    this.name = "九品地";
                }
                else if (value.Substring(8, 3).Equals("027"))
                {
                    this.name = "稲荷公園";
                }
                else if (value.Substring(8, 3).Equals("029"))
                {
                    this.name = "木曽川運動場";
                }
                else if (value.Substring(8, 3).Equals("030"))
                {
                    this.name = "奥町公園";
                }
                else if (value.Substring(8, 3).Equals("031"))
                {
                    this.name = "木曽川緑地";
                }
                else if (value.Substring(8, 3).Equals("032"))
                {
                    this.name = "萬葉公園";
                }
                else if (value.Substring(8, 3).Equals("033"))
                {
                    this.name = "尾西文化広場";
                }
                else if (value.Substring(8, 3).Equals("028"))
                {
                    this.name = "尾西運動場";
                }
            }
            get //値を返す
            {
                return this._id;
            }
        }
        public string place { get; set; }
        public string name { get; set; }

        public string day { get; set; }
        public List<Time> times { get; set; }
    }
    public class Time{
        private string _id;
        public string id
        {
            set //値をhpに代入する
            {
                this._id = value;
                this.place = value.Substring(0, 3);
                this.time = value.Substring(15);
                this.name = value.Substring(13,2);
                this.day = value.Substring(5, 8);
            }
            get //値を返す
            {
                return this._id;
            }
        }
        public string place { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string day { get; set; }
    }
 
}
