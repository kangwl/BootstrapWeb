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
           // Log log = new Log();
           // log.WriteLog("223sdd");
            bool success = true;
            try {
                List<string> codeList = GetCodeList();

                StockModel stockModel = new StockModel();
                stockModel.StockCodes.AddRange(codeList);
                stockModel.OpenID = FromUserName;
                string codes = Content.Substring(startWords.Length).Trim();
                string[] arr = codes.Split(',');
                foreach (string code in arr) {
                    stockModel.StockCodes.Add(code.Trim());
                }

                //log.WriteLog(JsonPath);
                if (!Directory.Exists(JsonPath1)) {
                    Directory.CreateDirectory(JsonPath1);
                }

                Common.json.JsonHelper<StockModel>.Serialize2File(stockModel, JsonPath);

               // log.WriteLog("ok");
            }
            catch (Exception ex) {
                success = false;
               // log.WriteLog(ex.Message);
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

        private List<string> GetCodeList() {
            List<string> codeList = new List<string>();
            if (Directory.Exists(JsonPath1)) {
                StockModel stockModel = Common.json.JsonHelper<StockModel>.DeserializeFromFile(JsonPath);

                codeList = stockModel.StockCodes;
            }
            return codeList;
        }

        public string GetStock(string startWords) {
            string stocks = "";
            Log log = new Log();
            try {

                List<string> codeList = GetCodeList();
                List<string> liststock = new List<string>();
                foreach (string code in codeList) {
                    string codeReq = GetCodeStr(code.Trim());
                    string reqUrl = string.Format(StockJS, codeReq);
                    log.WriteLog(reqUrl);
                    string stockStr = GetWebreq(reqUrl);
                    liststock.Add(code + stockStr);
                }

                stocks = string.Join("\n", liststock);

            }
            catch (Exception ex) {
         
                log.WriteLog(ex.ToString());
            }
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
            string name = jo["items"]["name"].ToString();//name
                        float zhangjiaF = zhangjia.ToFloat();
            float percentF = (zhangjiaF/jinkai.ToFloat())*100;
            string percentStr = percentF.ToString() + "%";
            string retStock = string.Format("-{4}\n 涨价：{0},今开：{1} \n 现价：{2},涨幅：{3}", zhangjia, jinkai, xianjia,percentStr, name);

            return retStock;
        }

        //hs_,sz_
        public string StockJS = @"http://d.10jqka.com.cn/v2/realhead/{0}/last.js";

        private string GetCodeStr(string code) {
            string codeStr = "{0}_{1}";
            string codeW = "hs_600372";
            if (code.StartsWith("6")) {
                codeW = string.Format(codeStr, "hs", code);
            }
            else if (code.StartsWith("0")) {
                codeW = string.Format(codeStr, "sz", code);
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
