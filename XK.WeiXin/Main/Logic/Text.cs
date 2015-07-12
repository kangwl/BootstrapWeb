using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using XK.Common;
using XK.WeiXin.Author;
using XK.WeiXin.Ext;

namespace XK.WeiXin.Main.Logic {
    public class Text : IMessageLogic {

        public Text(XmlDocument xmlRecieve) {
            XmlDoc = xmlRecieve;
            AddKeyWordFunc();
            AddKeyWordStartFunc();
        }


        /// <summary>
        /// 接收到的消息
        /// </summary>
        public XmlDocument XmlDoc { get; set; }

        private string Content { get { return XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//Content"); } }

        /// <summary>
        /// 逻辑处理后返回给发送者消息
        /// </summary>
        /// <returns></returns>
        public string ResponseMessage() {
            ReturnText = Content;
            Func<string> keywordFunc = null;
            keywordFunc = keywordFuncs.FirstOrDefault(d => d.Key == Content).Value ??
                          keyWordStartFuncs.FirstOrDefault(d => Content.StartsWith(d.Key)).Value;
            return (keywordFunc == null) ? CreateSendMsg() : keywordFunc();
        }



        private readonly Dictionary<string, Func<string>> keywordFuncs = new Dictionary<string, Func<string>>();

        private void AddKeyWordFunc() {
            keywordFuncs.Add(KeyWordEnum.time.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.时间.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.token.ToString(), GetAccessToken);
        }

        private readonly Dictionary<string, Func<string>> keyWordStartFuncs = new Dictionary<string, Func<string>>();

        private void AddKeyWordStartFunc() {
            keyWordStartFuncs.Add("添加股票", AddStock);
            keyWordStartFuncs.Add("删除股票",RemoveStock);
            keyWordStartFuncs.Add("股票", GetStock);

        }


        private string AddStock() {
            Log log=new Log();
            log.WriteLog("AddStock");
            Ext.Stock stock = new Stock(XmlDoc);
            ReturnText = stock.SaveStock("添加股票");
            return CreateSendMsg();
        }

        private string RemoveStock() {
            Ext.Stock stock = new Stock(XmlDoc);
            ReturnText = stock.RemoveStock("删除股票");
            return CreateSendMsg();
        }

        private string GetStock() {
            Ext.Stock stock = new Stock(XmlDoc);
            ReturnText = stock.GetStock("股票");
            return CreateSendMsg();
        }
        //#############################################################


        private string ReturnText { get; set; }

        private string GetDateTime() {

            ReturnText = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
             return CreateSendMsg();
        }

        private string GetAccessToken() {
            ReturnText = AccessToken.Instance.Value;
            return CreateSendMsg();
        }


        public string CreateSendMsg() {
            string ToUserName = XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//ToUserName");
            string FromUserName = XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//FromUserName");

            string msg = string.Format(sendXml, FromUserName, ToUserName,
                TimeConvert.GetDateTimeStamp(DateTime.Now), ReturnText);
            return msg;
        }

        public enum KeyWordEnum {
            time,
            时间,
            token
        }

        //        /// <summary>
        //        /// 接收的文本消息格式
        //        /// </summary>
        //        private string xmlRecieve = @"<xml>
        //                            <ToUserName><![CDATA[{0}]]></ToUserName>
        //                            <FromUserName><![CDATA[{1}]]></FromUserName> 
        //                            <CreateTime>{2}</CreateTime>
        //                            <MsgType><![CDATA[text]]></MsgType>
        //                            <Content><![CDATA[{3}]]></Content>
        //                            <MsgId>{4}</MsgId>
        //                            </xml>";

        /// <summary>
        /// 发送给用户的消息格式
        /// </summary>
        private string sendXml = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
 
    }
}
