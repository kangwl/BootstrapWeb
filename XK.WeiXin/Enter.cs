using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace XK.WeiXin {
    public class Enter {

        private readonly Dictionary<string, Action> dicFunc = new Dictionary<string, Action>();

        public Enter(string token, HttpRequest request, HttpResponse response) {
            Token = token;
            Request = request;
            Response = response;

            dicFunc.Add("post", ResponsePostMessage);
            dicFunc.Add("get", Valid);
        }

        public void Start() {

            string httpmethod_lower = Request.HttpMethod.ToLower();
            Action action = dicFunc.FirstOrDefault(d => d.Key == httpmethod_lower).Value;

            if (action != null) {
                action();
            }

        }


        private string Token { get; set; }

        private HttpRequest Request { get; set; }

        private HttpResponse Response { get; set; }

        /// <summary>

        /// 验证微信签名

        /// </summary>

        /// * 将token、timestamp、nonce三个参数进行字典序排序

        /// * 将三个参数字符串拼接成一个字符串进行sha1加密

        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。

        /// <returns></returns>

        private bool CheckSignature() {

            string signature = Request.QueryString["signature"];

            string timestamp = Request.QueryString["timestamp"];

            string nonce = Request.QueryString["nonce"];

            string[] ArrTmp = { Token, timestamp, nonce };

            Array.Sort(ArrTmp);     //字典排序

            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            if (tmpStr != null) {
                tmpStr = tmpStr.ToLower();

                if (tmpStr == signature) {

                    return true;

                }

                return false;
            }
            return false;
        }
 
        /// <summary>
        /// get 验证
        /// </summary>
        private void Valid() {

            string echoStr = Request.QueryString["echoStr"];

            if (CheckSignature()) {

                if (!string.IsNullOrEmpty(echoStr)) {

                    Response.Write(echoStr);

                    Response.End();

                }

            }
        }

        /// <summary>
        /// post 处理消息
        /// </summary>
        private void ResponsePostMessage() {
           // response message;
            using (System.IO.Stream s = Request.InputStream) {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int) s.Length);
                string postStr = System.Text.Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr)) {

                    //ResponseMsg(postStr);

                    // Response.Write(ResponseMsg(postStr));
                    Response.Write(1);
                    Response.End();
                }
            }


        }
    }
}
