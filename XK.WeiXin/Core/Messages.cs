﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using XK.WeiXin.Message;
using XK.WeiXin.Message.MessageInterface;

namespace XK.WeiXin.Core {
    public class Messages {
        private readonly Dictionary<string, Func<XmlDocument, string>> dicFuncMessage = new Dictionary<string, Func<XmlDocument, string>>();
        public Messages() {
            dicFuncMessage.Add(MessageTypeEnum.text.ToString(), ResponseTextMsg);
            dicFuncMessage.Add(MessageTypeEnum.image.ToString(), ResponseImageMsg);

        
        }

        /// <summary>
        /// 消息桥接
        /// 根据不同消息类型，经过不同的处理返回不同的结果
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <returns></returns>
        public string GetResponseMsg(Stream xmlStream) {
            string resMsg = "";
            XmlDocument xmlDoc = Common.XmlHelper.GetXmlDoc(xmlStream);
            string msgType = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgType");

            Func<XmlDocument, string> funcMessage = dicFuncMessage.FirstOrDefault(d => d.Key == msgType).Value;
            if (funcMessage != null) {
                resMsg = funcMessage(xmlDoc);
            }
            return resMsg;
        }

        //经过文本消息的逻辑处理后，输出
        private string ResponseTextMsg(XmlDocument xmlDoc) {

            //获取消息模型
            IMessage<Message.TextMessage.MessageRecieved_Model, Message.TextMessage.MessageSend_Model> textMessage =
                new Message.TextMessage();
            Message.TextMessage.MessageRecieved_Model messageModel = textMessage.GetMessage(xmlDoc);

            //logic
            Message.TextMessage.MessageSend_Model sendModel = new Logic.TextMessageLogic().ReturnMessage(messageModel);
 
            string txtMsg = textMessage.InitSendMessage(sendModel);
            return txtMsg;
        }

        private string ResponseImageMsg(XmlDocument xmlDoc) {
            IMessage<Message.ImageMessage.MessageRecieve_Model,Message.ImageMessage.MessageSend_Model> imageMessage = new Message.ImageMessage();
            Message.ImageMessage.MessageRecieve_Model messageModel = imageMessage.GetMessage(xmlDoc);

            Message.ImageMessage.MessageSend_Model sendModel = new Logic.ImageMessageLogic().ReturnMessage(messageModel);

            string imgMsg = imageMessage.InitSendMessage(sendModel);
            return imgMsg;
        }




    }
}
