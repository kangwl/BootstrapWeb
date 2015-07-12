using System;
using XK.Common;
using XK.WeiXin.Logic.ReturnInterface;
using XK.WeiXin.Message;

namespace XK.WeiXin.Logic {
    /// <summary>
    /// 消息进
    /// 消息出
    /// </summary>
    public class ImageMessageLogic : IReturnMessage<ImageMessage.MessageRecieve_Model, ImageMessage.MessageSend_Model> {
        /// <summary>
        /// 消息出
        /// </summary>
        /// <param name="messageModel">进来的消息</param>
        /// <returns></returns>
        public ImageMessage.MessageSend_Model ReturnMessage(ImageMessage.MessageRecieve_Model messageModel) {
            //logic
            ImageMessage.MessageSend_Model messageSend = new ImageMessage.MessageSend_Model();
            messageSend.MediaId = messageModel.MediaId;
            messageSend.FromUserName = messageModel.FromUserName;
            messageSend.CreateTime = TimeConvert.GetDateTimeStamp(DateTime.Now);
            messageSend.ToUserName = messageModel.ToUserName;

            return messageSend;
        }

    }
}
