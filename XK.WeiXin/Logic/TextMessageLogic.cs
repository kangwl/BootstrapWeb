using System;
using XK.Common;
using XK.WeiXin.Logic.ReturnInterface;
using XK.WeiXin.Message;

namespace XK.WeiXin.Logic {
    public class TextMessageLogic : IReturnMessage<TextMessage.MessageRecieved_Model,TextMessage.MessageSend_Model> {
        /// <summary>
        /// 逻辑处理后返回给发送者消息
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        public TextMessage.MessageSend_Model ReturnMessage(TextMessage.MessageRecieved_Model messageModel) {
            TextMessage.MessageSend_Model sendModel = new TextMessage.MessageSend_Model();
            sendModel.Content = messageModel.Content;
            sendModel.CreateTime = TimeConvert.GetDateTimeStamp(DateTime.Now);
            sendModel.FromUserName = messageModel.FromUserName;
            sendModel.ToUserName = messageModel.ToUserName;

            return sendModel;
        }
 
    }
}
