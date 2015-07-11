using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XK.WeiXin.Message {
    public class Text:IMessage<Text.MessageModel> {

        /// <summary>
        /// 接收的文本消息格式
        /// </summary>
        private string xmlRecieve = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName> 
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            <MsgId>{4}</MsgId>
                            </xml>";


        /// <summary>
        /// 获取用户发过来的消息
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        MessageModel IMessage<MessageModel>.GetMessage(XmlDocument xmlDoc) {
            MessageModel messageModel = new MessageModel();
            messageModel.ToUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//ToUserName");
            messageModel.Content = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//Content");
            messageModel.CreateTime = long.Parse(Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//CreateTime"));
            messageModel.FromUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//FromUserName");
            messageModel.MsgId = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgId");
            return messageModel;
        }

        public string InitSendMessage(MessageModel messageModel) {
            string recieveMsg = "";
            if (messageModel != null) {
                recieveMsg = string.Format(xmlSend, messageModel.FromUserName, messageModel.ToUserName,
                    messageModel.CreateTime, messageModel.Content);
            }
            return recieveMsg;
        }

        /// <summary>
        /// 发送给用户的消息格式
        /// </summary>
        private string xmlSend = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";


        public class MessageModel:MessageModelBase {

            /// <summary>
            /// 开发者微信号
            /// </summary>
            public string ToUserName { get; set; }
            /// <summary>
            /// 发送方帐号（一个OpenID）
            /// </summary>
            public string FromUserName { get; set; }
            /// <summary>
            /// 消息创建时间 （整型）
            /// </summary>
            public long CreateTime { get; set; }
            /// <summary>
            /// text
            /// </summary>
            public string MsgType = "text";
            /// <summary>
            /// 文本消息内容
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 消息id，64位整型
            /// </summary>
            public string MsgId { get; set; }

        }


  
    }
}
