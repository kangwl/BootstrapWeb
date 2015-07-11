using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using XK.WeiXin.Message;

namespace XK.WeiXin.Core {
    public class Messages {
        private readonly Dictionary<string, Func<Stream, string>> dicFuncMessage = new Dictionary<string, Func<Stream, string>>();
        public Messages() {
            dicFuncMessage.Add(MessageTypeEnum.text.ToString(), ResponseTextMsg);
            dicFuncMessage.Add(MessageTypeEnum.image.ToString(), ResponseImageMsg);

        
        }

        public string GetResponseMsg(Stream xmlStream) {
            string resMsg = "";
            XmlDocument xmlDoc = Common.XmlHelper.GetXmlDoc(xmlStream);
            string msgType = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgType");

            Func<Stream, string> funcMessage = dicFuncMessage.FirstOrDefault(d => d.Key == msgType).Value;
            if (funcMessage != null) {
                resMsg = funcMessage(xmlStream);
            }
            return resMsg;
        }

        private string ResponseTextMsg(Stream xmlStream) {
       
            IMessage<Message.Text.MessageModel> textMessage = new Text();
            Text.MessageModel messageModel = textMessage.GetMessage(xmlStream);
            string fromUser = messageModel.FromUserName;
            string toUser = messageModel.ToUserName;
            //logic
            string retText = Logic.TextMessage.ReturnMessage(messageModel);
            messageModel.Content = retText;
            messageModel.ToUserName = fromUser;
            messageModel.FromUserName = toUser;
            messageModel.CreateTime = Common.TimeConvert.GetDateTimeStamp(DateTime.Now);
            string txtMsg = textMessage.InitSendMessage(messageModel);

            return txtMsg;
        }

        private string ResponseImageMsg(Stream xmlStream) {
            return "";
        }
    }
}
