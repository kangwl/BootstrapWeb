using System;
using XK.Common;
using XK.WeiXin.Logic.ReturnInterface;
using XK.WeiXin.Message;

namespace XK.WeiXin.Logic {
    /// <summary>
    /// 消息进
    /// 消息出
    /// </summary>
    public class ImageMessageLogic : IReturnMessage<ImageMessage.MessageSend_Model> {
        public ImageMessageLogic(ImageMessage.MessageRecieve_Model recievedModel) {
            MessageRecieved = recievedModel;
        }

        public ImageMessage.MessageRecieve_Model MessageRecieved { get; set; }

        /// <summary>
        /// 消息出
        /// </summary>
        /// <returns></returns>
        public ImageMessage.MessageSend_Model ReturnMessage() {
            //logic
            ImageMessage.MessageSend_Model messageSend = new ImageMessage.MessageSend_Model {
                MediaId = MessageRecieved.MediaId,
                FromUserName = MessageRecieved.FromUserName,
                CreateTime = TimeConvert.GetDateTimeStamp(DateTime.Now),
                ToUserName = MessageRecieved.ToUserName
            };

            return messageSend;
        }
 
    }
}
