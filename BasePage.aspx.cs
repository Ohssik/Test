using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Collections;
using System.Configuration;


    public partial class BasePage : System.Web.UI.Page
    {
        public BasePage() { }
        private void SetActionStamp()
        {
            Session["actionStamp"] = Server.UrlEncode(DateTime.Now.ToString());
        }
        void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetActionStamp();
            }
			if(Session["actionStamp"]!=null)
				ClientScript.RegisterHiddenField("actionStamp", Session["actionStamp"].ToString());
			else
				//ClientScript.RegisterHiddenField("actionStamp", Server.UrlEncode(DateTime.Now.ToString()));
				Response.Redirect(Request.FilePath);
        }
        /// 取得值，指出網頁是否經由重新整理動作回傳 (PostBack)
        /// </summary>
        protected bool IsRefresh
        {
            get
            {
                if (HttpContext.Current.Request["actionStamp"] as string == Session["actionStamp"] as string)
                {
                    SetActionStamp();
                    return false;
                }

                return true;
            }
        }
        
    }
