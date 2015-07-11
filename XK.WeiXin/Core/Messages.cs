using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using XK.WeiXin.Message.MessageInterface;

namespace XK.WeiXin.Core {
    public class Messages {
        private readonly Dictionary<string, Func<XmlDocument, string>> dicFuncMessage = new Dictionary<string, Func<XmlDocument, string>>();
        public Messages() {
            dicFuncMessage.Add(MessageTypeEnum.text.ToString(), ResponseTextMsg);
            dicFuncMessage.Add(MessageTypeEnum.image.ToString(), ResponseImageMsg);

        
        }

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

        private string ResponseTextMsg(XmlDocument xmlDoc) {

            IMessage<Message.Text.MessageModel> textMessage = new Message.Text();
            Message.Text.MessageModel messageModel = textMessage.GetMessage(xmlDoc);
            //logic
            string retText = Logic.TextMessage.ReturnMessage(messageModel);
            messageModel.Content = retText;
            messageModel.CreateTime = Common.TimeConvert.GetDateTimeStamp(DateTime.Now);
            string txtMsg = textMessage.InitSendMessage(messageModel);

            return txtMsg;
        }

        private string ResponseImageMsg(XmlDocument xmlDoc) {
            IMessage<Message.Image.MessageModel> imageMessage = new Message.Image();
            Message.Image.MessageModel messageModel = imageMessage.GetMessage(xmlDoc);
            string picUrl = Logic.ImageMessage.ReturnMessage(messageModel);
            messageModel.PicUrl = picUrl;
            messageModel.CreateTime = Common.TimeConvert.GetDateTimeStamp(DateTime.Now);
            string imgMsg = imageMessage.InitSendMessage(messageModel);
            return imgMsg;
        }
    }
}
