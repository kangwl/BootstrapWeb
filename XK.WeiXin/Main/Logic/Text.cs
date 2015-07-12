using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using XK.Common;
using XK.WeiXin.Author;

namespace XK.WeiXin.Main.Logic {
    public class Text {

        public Text(XmlDocument xmlRecieve) {
            XmlDoc = xmlRecieve;
            keywordFuncs.Add(KeyWordEnum.time.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.时间.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.token.ToString(), GetAccessToken);
        }


        /// <summary>
        /// 接收到的消息
        /// </summary>
        private XmlDocument XmlDoc { get; set; }

        private string Content { get { return XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//Content"); } }

        /// <summary>
        /// 逻辑处理后返回给发送者消息
        /// </summary>
        /// <returns></returns>
        public string ResponseMessage() {
            Func<string> keywordFunc =
                keywordFuncs.FirstOrDefault(d => d.Key == Content).Value;
            if (keywordFunc == null) {
                return "";
            }
            return keywordFunc();
        }

        private readonly Dictionary<string, Func<string>> keywordFuncs =
            new Dictionary<string, Func<string>>();



        private string GetDateTime() {
           
             string datetime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
             return CreateSendMsg(datetime);
        }

        private string GetAccessToken() {
            string accessToken = AccessToken.Instance.Value;
            return CreateSendMsg(accessToken);
        }



        private string CreateSendMsg(string content) {
            string ToUserName = XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//ToUserName");
            string FromUserName = XmlHelper.GetXmlNodeTextByXpath(XmlDoc, "//FromUserName");
            string msg = string.Format(sendXml, FromUserName, ToUserName,
                TimeConvert.GetDateTimeStamp(DateTime.Now), content);
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
