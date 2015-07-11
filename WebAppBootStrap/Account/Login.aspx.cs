using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XK.Authentication;
using XK.Bll;

namespace WebAppBootStrap.Account {
    public partial class Login : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            //bool success = User_Bll.CreateTable();
            //if (success) {
            Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
            dic.Add("Name","kangwl");
            dic.Add("UserID","kangwl");
            dic.Add("UserPassword","123456");
            dic.Add("Age",12);
            dic.Add("Sex",1);
            dic.Add("MobilePhone","15869165656");
            dic.Add("Email","kangwl2009@163.com");
            dic.Add("AddDateTime",DateTime.Now);
            dic.Add("UpdateDateTime",DateTime.Now);
            dic.Add("UserType",1);
            bool success = User_Bll.Insert(dic);
            if (success) {
                int count = User_Bll.GetRecordCount();
                Response.Write(count);
            }
            //}
            //test
            //if (LoginLogic.HasLogin) {
            //    Response.Redirect(LoginLogic.DefaultUrl);
            //}
        }

        public string RetutnUrl { get { return Request.QueryString["returl"]; } } 
    }
}