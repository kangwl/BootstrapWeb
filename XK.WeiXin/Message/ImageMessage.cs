using System.Xml;
using XK.Common;
using XK.WeiXin.Message.MessageBase;
using XK.WeiXin.Message.MessageInterface;

namespace XK.WeiXin.Message {
    public class ImageMessage : IMessage<ImageMessage.MessageRecieve_Model, ImageMessage.MessageSend_Model> {

        public class MessageRecieve_Model : Recieve {

            /// <summary>
            /// image
            /// </summary>
            public string MsgType = "image";

            /// <summary>
            /// 图片链接
            /// </summary>
            public string PicUrl { get; set; }

            /// <summary>
            /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
            /// </summary>
            public string MediaId { get; set; }

            /// <summary>
            /// 消息id，64位整型
            /// </summary>
            public string MsgId { get; set; }

        }

        public class MessageSend_Model:Send {
            /// <summary>
            /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
            /// </summary>
            public string MediaId { get; set; }
        }

        public string recieveXml = @" <xml>
                                 <ToUserName><![CDATA[toUser]]></ToUserName>
                                 <FromUserName><![CDATA[fromUser]]></FromUserName>
                                 <CreateTime>1348831860</CreateTime>
                                 <MsgType><![CDATA[image]]></MsgType>
                                 <PicUrl><![CDATA[this is a url]]></PicUrl>
                                 <MediaId><![CDATA[media_id]]></MediaId>
                                 <MsgId>1234567890123456</MsgId>
                                 </xml>";

        public MessageRecieve_Model GetMessage(XmlDocument xmlDoc) {
            MessageRecieve_Model messageModel = new MessageRecieve_Model();
            messageModel.ToUserName = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//ToUserName");
            messageModel.FromUserName = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//FromUserName");
            messageModel.CreateTime = long.Parse(XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//CreateTime"));
            messageModel.PicUrl = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//PicUrl");
            messageModel.MediaId = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MediaId");
            messageModel.MsgId = XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgId");
            return messageModel;
        }


        public string sendXml = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[image]]></MsgType>
                                <Image>
                                <MediaId><![CDATA[{3}]]></MediaId>
                                </Image>
                                </xml>";
 

        public string InitSendMessage(MessageSend_Model messageModel) {
            string recieveMsg = "";
            if (messageModel != null) {
                recieveMsg = string.Format(sendXml, messageModel.FromUserName, messageModel.ToUserName,
                    messageModel.CreateTime, messageModel.MediaId);
            }
            return recieveMsg;
        }
    }
}
