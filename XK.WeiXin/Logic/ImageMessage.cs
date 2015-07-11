using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XK.WeiXin.Logic {
    public class ImageMessage {
        public static string ReturnMessage(Message.Image.MessageModel messageModel) {
            //sdd
            return messageModel.MediaId;
        }
    }
}
