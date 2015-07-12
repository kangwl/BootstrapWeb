using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using XK.WeiXin.Message.MessageBase;
using XK.WeiXin.Message.MessageInterface;

namespace XK.WeiXin.Message {
    public class TextMessage:IMessage<Message.TextMessage.MessageRecieved_Model,Message.TextMessage.MessageSend_Model> {

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


 
        public MessageRecieved_Model GetMessage(XmlDocument xmlDoc) {
            MessageRecieved_Model messageModel = new MessageRecieved_Model {
                ToUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//ToUserName"),
                Content = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//Content"),
                CreateTime = long.Parse(Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//CreateTime")),
                FromUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//FromUserName"),
                MsgId = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgId")
            };
            return messageModel;
        }
 

        public string InitSendMessage(MessageSend_Model messageModel) {
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

        /// <summary>
        /// 公众号接收到的消息模型
        /// </summary>
        public class MessageRecieved_Model:Recieve  {

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
        /// <summary>
        /// 公众号发给用户消息的模型
        /// </summary>
        public class MessageSend_Model :Send {

            /// <summary>
            /// 文本消息内容
            /// </summary>
            public string Content { get; set; } 

        }


    }
}
