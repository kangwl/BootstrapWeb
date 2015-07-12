using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using XK.Common;
using XK.WeiXin.Logic;
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
            XmlDocument xmlDoc = XmlHelper.GetXmlDoc(xmlStream);
            string msgType = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgType");

            Func<XmlDocument, string> funcMessage = dicFuncMessage.FirstOrDefault(d => d.Key == msgType).Value;
            if (funcMessage != null) {
                resMsg = funcMessage(xmlDoc);
            }
            return resMsg;
        }

        //经过文本消息的逻辑处理后，输出
        private string ResponseTextMsg(XmlDocument xmlDoc) {

            //获取消息模型
            IMessage<TextMessage.MessageRecieved_Model, TextMessage.MessageSend_Model> textMessage =
                new TextMessage();
            TextMessage.MessageRecieved_Model recievedModel = textMessage.GetRecievedMessage(xmlDoc);

            //logic
            TextMessage.MessageSend_Model sendModel = new TextMessageLogic(recievedModel).ReturnMessage();
 
            string txtMsg = textMessage.InitSendMessage(sendModel);
            return txtMsg;
        }

        private string ResponseImageMsg(XmlDocument xmlDoc) {
            IMessage<ImageMessage.MessageRecieve_Model,ImageMessage.MessageSend_Model> imageMessage = new ImageMessage();
            ImageMessage.MessageRecieve_Model recieveModel = imageMessage.GetRecievedMessage(xmlDoc);

            ImageMessage.MessageSend_Model sendModel = new ImageMessageLogic(recieveModel).ReturnMessage();

            string imgMsg = imageMessage.InitSendMessage(sendModel);
            return imgMsg;
        }




    }
}
