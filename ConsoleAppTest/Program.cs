using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XK.Common.web;

namespace ConsoleAppTest {
    class Program {
        private static void Main(string[] args) {
            //TestM testM = new TestM();
            //Console.WriteLine(testM.GetResult("3"));
            //Console.WriteLine(testM.GetResultBridge("3"));
            //Console.WriteLine("end");
            //Console.Read();

            //                 string xmlSend = @"<xml>
            //                            <ToUserName><![CDATA[123]]></ToUserName>
            //                            <FromUserName><![CDATA[2]]></FromUserName>
            //                            <CreateTime>123</CreateTime>
            //                            <MsgType><![CDATA[text]]></MsgType>
            //                            <Content><![CDATA[333]]></Content>
            //                            </xml>";
            //            XmlDocument xmldoc = new XmlDocument();
            //            xmldoc.LoadXml(xmlSend);
            //            string a = XK.Common.XmlHelper.GetXmlNodeTextByXpath(xmldoc, "//ToUserName");
            //            Console.WriteLine(a);
            //            Console.WriteLine("end");
            //            Console.Read();
            //XK.Common.web.HttpWebHelper webHelper = new HttpWebHelper("http://d.10jqka.com.cn/v2/realhead/hs_600372/last.js");
            //string res = webHelper.GetResponseStr();
            //int firstIndex = res.IndexOf('{');
            //string s = res.Substring(firstIndex).TrimEnd(')');

            //JObject jo = (JObject)JsonConvert.DeserializeObject(s);
            //string zone = jo["items"]["7"].ToString();
            //Newtonsoft.Json.JsonReader reader=new JTokenReader();

            string reqBase = "http://news.10jqka.com.cn/public/index_keyboard_{0}_stock,hk,usa_5_jsonp.html";
            string reqUrl = string.Format(reqBase, "zhdz");
            XK.Common.web.HttpWebHelper webHelper = new HttpWebHelper(reqUrl);
            string res = HttpUtility.UrlDecode(webHelper.GetResponseStr());
            int firstIndex = res.IndexOf('[');
            string s = res.Substring(firstIndex).TrimEnd(')');
            var a = s.Substring(1);
            var aa = a.Substring(0, a.Length - 1);
            var r = (aa.Split(',')[0].TrimStart('[').TrimEnd(']'));
            var rarr = r.Split(',');
            List<string> stocks = new List<string>();
            foreach (string s1 in rarr) {
                var sArr = s1.Split(' ');
                string code = sArr[0];
                string name = sArr[1]  ;

                string outStr = "";
                if (!string.IsNullOrEmpty(name)) {
                    string[] strlist = name.Replace("\\", "").Split('u');
                    try {
                        for (int i = 1; i < strlist.Length; i++) {
                            //将unicode字符转为10进制整数，然后转为char中文字符  
                            outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                        }
                    }
                    catch (FormatException ex) {
                        outStr = ex.Message;
                    }
                }
                stocks.Add(code + " " + outStr);
            }
            Console.WriteLine(string.Join("\n", stocks));
            Console.WriteLine("\u4e2d\u822a\u7535\u5b50"); 
            Console.Read();
        }

    
      
    }

    public class TestM {

        #region 普通 if else 逻辑判断

        /// <summary>
        /// 普通 if else
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public string GetResult(string act) {
            if (act == "1") {
                return n1();
            }
            else if (act == "2") {
                return n2();
            }
            else if (act == "3") {
                return n3();
            }
            return "unknow";
        }

        #endregion


        #region 不用 if else 用委托进行逻辑判断（免除大量的 if else 分支判断）

        private readonly Dictionary<string, Func<string>> dicFunc = new Dictionary<string, Func<string>>();

        public TestM() {
            dicFunc.Add("1", n1);
            dicFunc.Add("2", n2);
            dicFunc.Add("3", n3);
        }

        public string GetResultBridge(string num) {
            Func<string> methodFunc;
            //bool has = dicFunc.TryGetValue(num, out methodFunc);
            //if (!has) {
            //    return "unknow";
            //}
            methodFunc = dicFunc.FirstOrDefault(d => d.Key == num).Value;
            if (methodFunc == null) {
                return "unknow";
            }
            string res = methodFunc();
            return res;
        } 

        #endregion


        private string n1() {
            return "n1";
        }

        private string n2() {
            return "n2";
        }

        private string n3() {
            return "n3";
        }

    }
 
}
