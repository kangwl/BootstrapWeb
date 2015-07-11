using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XK.WeiXin.Logic {
    public class TextMessage {
        public static string ReturnMessage(Message.Text.MessageModel messageModel) {
            string retText = "err";
            if (messageModel.Content.Length < 5) {
                retText = "字数太少";
            }
            else {
                retText = messageModel.Content;
            }
            return retText;
        }
    }
}
