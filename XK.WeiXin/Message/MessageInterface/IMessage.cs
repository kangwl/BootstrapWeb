using System.Xml;

namespace XK.WeiXin.Message.MessageInterface {
    public interface IMessage<out T1, in T2> where T1 : class where T2 : class {
        /// <summary>
        /// 获取用户发过来的消息
        /// </summary>
        /// <param name="xmlDoc">接收到的消息（已处理为xml文档）</param>
        /// <returns></returns>
        T1 GetRecievedMessage(XmlDocument xmlDoc);

        /// <summary>
        /// 组装要发送的消息xml
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        string InitSendMessage(T2 messageModel);
    }
}
