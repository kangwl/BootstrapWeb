using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XK.WeiXin;

namespace WebAppBootStrap.weixin {
    public partial class Index : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            XK.WeiXin.Enter enter = new Enter(XK.WeiXin.Author.AppConfig.Instance.Token, Request, Response);
            enter.Start();
        }
    }
}