using System.Xml;

namespace XK.WeiXin.Message.MessageInterface {
    public interface IMessage<T> where T:class {
        T GetMessage(XmlDocument xmlDoc) ;
        string InitSendMessage(T messageModel);
    }
}
