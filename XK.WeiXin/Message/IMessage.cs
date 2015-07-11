using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XK.WeiXin.Message {
    public interface IMessage<T> where T:class {
        T GetMessage(Stream xmlStream) ;
        string InitSendMessage(T messageModel);
    }
}
