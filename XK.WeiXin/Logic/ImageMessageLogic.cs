using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XK.WeiXin.Logic.ReturnInterface;
using XK.WeiXin.Message;

namespace XK.WeiXin.Logic {
    public class ImageMessageLogic : IReturnMessage<Message.ImageMessage.MessageRecieve_Model, Message.ImageMessage.MessageSend_Model> {
        public Message.ImageMessage.MessageSend_Model ReturnMessage(Message.ImageMessage.MessageRecieve_Model messageModel) {
            //sdd
            return default(Message.ImageMessage.MessageSend_Model);
        }

    }
}
