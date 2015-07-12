namespace XK.WeiXin.Message.MessageBase {
    /// <summary>
    /// 发送给用户的基类
    /// </summary>
    public abstract class Send {
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public long CreateTime { get; set; }
    }
}
