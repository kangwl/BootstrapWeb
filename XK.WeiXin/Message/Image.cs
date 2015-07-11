using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XK.WeiXin.Message.MessageInterface;

namespace XK.WeiXin.Message {
    public class Image : IMessage<Image.MessageModel> {

       public class MessageModel {

           /// <summary>
           /// 开发者微信号
           /// </summary>
           public string ToUserName { get; set; }
           /// <summary>
           /// 发送方帐号（一个OpenID）
           /// </summary>
           public string FromUserName { get; set; }
           /// <summary>
           /// 消息创建时间 （整型）
           /// </summary>
           public long CreateTime { get; set; }
           /// <summary>
           /// text
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

        public string recieveXml = @" <xml>
                                 <ToUserName><![CDATA[toUser]]></ToUserName>
                                 <FromUserName><![CDATA[fromUser]]></FromUserName>
                                 <CreateTime>1348831860</CreateTime>
                                 <MsgType><![CDATA[image]]></MsgType>
                                 <PicUrl><![CDATA[this is a url]]></PicUrl>
                                 <MediaId><![CDATA[media_id]]></MediaId>
                                 <MsgId>1234567890123456</MsgId>
                                 </xml>";

        public MessageModel GetMessage(XmlDocument xmlDoc) {
            MessageModel messageModel = new MessageModel();
            messageModel.ToUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//ToUserName");
            messageModel.FromUserName = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//FromUserName");
            messageModel.CreateTime = long.Parse(Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//CreateTime"));
            messageModel.PicUrl = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//PicUrl");
            messageModel.MediaId = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MediaId");
            messageModel.MsgId = Common.XmlHelper.GetXmlNodeTextByXpath(xmlDoc, "//MsgId");
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
        public string InitSendMessage(MessageModel messageModel) {
            string recieveMsg = "";
            if (messageModel != null) {
                recieveMsg = string.Format(sendXml, messageModel.FromUserName, messageModel.ToUserName,
                    messageModel.CreateTime, messageModel.MediaId);
            }
            return recieveMsg;
        }
    }
}
