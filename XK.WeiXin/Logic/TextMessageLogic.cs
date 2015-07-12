using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XK.WeiXin.Logic.ReturnInterface;

namespace XK.WeiXin.Logic {
    public class TextMessageLogic : IReturnMessage<Message.TextMessage.MessageRecieved_Model,Message.TextMessage.MessageSend_Model> {
        /// <summary>
        /// 逻辑处理后返回给发送者消息
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        public Message.TextMessage.MessageSend_Model ReturnMessage(Message.TextMessage.MessageRecieved_Model messageModel) {
      
            return default(Message.TextMessage.MessageSend_Model);
        }
 
    }
}
