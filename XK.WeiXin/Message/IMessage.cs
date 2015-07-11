using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XK.WeiXin.Message {
    public interface IMessage<T> where T:class {
        T GetMessage(XmlDocument xmlDoc) ;
        string InitSendMessage(T messageModel);
    }
}
