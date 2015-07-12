using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XK.Common;
using XK.Common.web;

namespace XK.WeiXin.Ext {
    public class Stock {
        public Stock(XmlDocument _xmlDoc) {
            XmlDoc = _xmlDoc;
        }
          public class StockModel {
              public StockModel() {
                  StockCodes = new List<string>();
              }
              /// <summary>
              /// 用户唯一标识
              /// </summary>
              public string OpenID { get;set; }
              /// <summary>
              /// 股票代码
              /// </summary>
              public List<string> StockCodes { get; set; } 
          }
          /// <summary>
          /// 接收到的消息
          /// </summary>
          public XmlDocument XmlDoc { get; set; }
          private string Content { get { return XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//Content"); } }
          private string FromUserName { get { return XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//FromUserName"); } }

          private string JsonPath1 { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stock"); } }

          private string JsonPath { get { return  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("stock\\{0}.txt", FromUserName)); } }

        public string SaveStock(string startWords) {
            Log log = new Log();
            log.WriteLog("223sdd");
            bool success=true;
            try {

            StockModel stockModel = new StockModel();
            stockModel.OpenID = FromUserName;
            string codes = Content.Substring(startWords.Length).Trim();
            string[] arr = codes.Split(',');
            foreach (string code in arr) {
              stockModel.StockCodes.Add(code.Trim());
            }

                log.WriteLog(JsonPath);
                if (!Directory.Exists(JsonPath1)) {
                    Directory.CreateDirectory(JsonPath1);
                }

                Common.json.JsonHelper<StockModel>.Serialize2File(stockModel, JsonPath);

            log.WriteLog("ok");
            }
            catch (Exception ex) {
                success = false;
                log.WriteLog(ex.Message);
            }

            return success ? "添加成功" : "添加失败";
        }

        public string RemoveStock(string startWords) {
            string[] arr = Content.Substring(startWords.Length).Trim().Split(',');
            StockModel stockModel = Common.json.JsonHelper<StockModel>.DeserializeFromFile(JsonPath);
            List<string> codes = stockModel.StockCodes;
            foreach (string code in arr) {
                codes.Remove(code.Trim());
            }
            bool success = true;
            try {

            Common.json.JsonHelper<StockModel>.Serialize2File(stockModel, JsonPath);

            }
            catch (Exception) {
                success = false;
            }
            return success ? "删除成功" : "删除失败";
        }

        public string GetStock(string startWords) {
            StockModel stockModel = Common.json.JsonHelper<StockModel>.DeserializeFromFile(JsonPath);
            List<string> codeList = stockModel.StockCodes;
            List<string> liststock = new List<string>();
            foreach (string code in codeList) {
                string codeReq = GetCodeStr(code.Trim());
                string reqUrl = string.Format(StockJS, codeReq);

                string stockStr = GetWebreq(reqUrl);
                liststock.Add(stockStr);
            }

            string stocks = string.Join(",", liststock);
            return stocks;
        }

        private string GetWebreq(string reqUrl) {
            Common.web.HttpWebHelper webHelper = new HttpWebHelper(reqUrl);
            string res = webHelper.GetResponseStr();
            int firstIndex = res.IndexOf('{');
            string s = res.Substring(firstIndex).TrimEnd(')');

            JObject jo = (JObject) JsonConvert.DeserializeObject(s);
            string zhangjia = jo["items"]["264648"].ToString();
            string zhuoshou = jo["items"]["6"].ToString();
            string jinkai = jo["items"]["7"].ToString();
            string xianjia = jo["items"]["10"].ToString();
            string zuigao = jo["items"]["8"].ToString();
            string retStock = string.Format("昨收：{0},涨价：{1},今开：{2},现价：{3}", zhuoshou, zhangjia, jinkai, xianjia);
            return retStock;
        }

        //hs_,sz_
        public string StockJS = @"http://d.10jqka.com.cn/v2/realhead/{0}/last.js";

        private string GetCodeStr(string code) {
            string codeStr = "{0}_{1}";
            string codeW = "hs_600372";
            if (code.StartsWith("6")) {
                codeW = string.Format(codeStr, "hs_", code);
            }
            else if (code.StartsWith("0")) {
                codeW = string.Format(codeStr, "sz_", code);
            }
            return codeW;
        }

        private class items {
            
        }
        //public class StockShowModel {
        //    /// <summary>
        //    /// key:代码，value:名字
        //    /// </summary>
        //    public Dictionary<string, string> DicStock { get; set; }  
        //}
    }
}
