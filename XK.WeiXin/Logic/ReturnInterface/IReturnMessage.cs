namespace XK.WeiXin.Logic.ReturnInterface {
    public interface IReturnMessage<in T1, out T2> where T1 : class where T2:class {
        /// <summary>
        /// 公众号接收到消息后返回消息给用户
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        T2 ReturnMessage(T1 messageModel);
    }
}