namespace XK.WeiXin.Logic.ReturnInterface {
    public interface IReturnMessage<out TMessageSend> where TMessageSend : class {
 
        /// <summary>
        /// 公众号接收到消息后返回消息给用户
        /// </summary>
        /// <returns></returns>
        TMessageSend ReturnMessage();
    }
}