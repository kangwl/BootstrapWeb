using System;
using System.Collections.Generic;
using System.Linq;
using XK.Common;
using XK.WeiXin.Logic.ReturnInterface;
using XK.WeiXin.Message;

namespace XK.WeiXin.Logic {
    public class TextMessageLogic : IReturnMessage<TextMessage.MessageSend_Model> {

        public TextMessageLogic(TextMessage.MessageRecieved_Model recievedModel) {
            MessageRecieved = recievedModel;
            keywordFuncs.Add(KeyWordEnum.time.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.时间.ToString(), GetDateTime);
            keywordFuncs.Add(KeyWordEnum.token.ToString(), GetAccessToken);
        }

        public TextMessage.MessageRecieved_Model MessageRecieved { get; set; }

        /// <summary>
        /// 逻辑处理后返回给发送者消息
        /// </summary>
        /// <returns></returns>
        public TextMessage.MessageSend_Model ReturnMessage() {
            Func<TextMessage.MessageSend_Model> keywordFunc =
                keywordFuncs.FirstOrDefault(d => d.Key == MessageRecieved.Content).Value;
            if (keywordFunc == null) {
                return new TextMessage.MessageSend_Model();
            }
            return keywordFunc();
        }

        private readonly Dictionary<string, Func<TextMessage.MessageSend_Model>> keywordFuncs =
            new Dictionary<string, Func<TextMessage.MessageSend_Model>>();



        private Message.TextMessage.MessageSend_Model GetDateTime() {
             string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             return CreateSendModel(datetime);
        }

        private Message.TextMessage.MessageSend_Model GetAccessToken() {
            string accessToken = Author.AccessToken.Instance.Value;
            return CreateSendModel(accessToken);
        }

        public enum KeyWordEnum {
            time,
            时间,
            token
        }

        private Message.TextMessage.MessageSend_Model CreateSendModel(string content) {
            Message.TextMessage.MessageSend_Model messageSend = new TextMessage.MessageSend_Model();
            messageSend.Content = content;
            messageSend.FromUserName = MessageRecieved.FromUserName;
            messageSend.CreateTime = Common.TimeConvert.GetDateTimeStamp(DateTime.Now);
            messageSend.ToUserName = MessageRecieved.ToUserName;

            return messageSend;
        }
 
    }
}
