using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // if request url contains the site parameter, save it for further redirection
            if (!string.IsNullOrEmpty(Request["site"]))
            {
                site.Value = Request["site"].ToString();
            }
            // if request url contains the redirect_page parameter, then save it for redirection
            if (!string.IsNullOrEmpty(Request["return_page"]))
            {
                return_page.Value = Request["return_page"].ToString();
            }
        }
    }

    protected void getLogin(object sender, EventArgs e)
    {
        string[] url = Request.Url.ToString().Split('?');
        List<string> split_values = url[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        split_values.RemoveAt(split_values.Count - 1);
        split_values.RemoveAt(0);
        string current_url = "http" + (((!string.IsNullOrEmpty(Request.ServerVariables["HTTPS"]) && Request.ServerVariables["HTTPS"].ToString().ToLower() == "on") ? "s" : "") + "://") + string.Join("/", split_values);
        if (Request["submit"] != null)
        {
            string uname = username.Value;
            string upass = password.Value;
            if (uname == "demo" && upass == "demo!")
            {
                //establish the local loggedin session
                Session["user_id"] = 1;
                if (!string.IsNullOrEmpty(Request["site"]))
                {
                    //if site parameters is not empty, redirect to remote log in URL, to establish the helpIQ session                    
                    Response.RedirectPermanent(current_url + "/helpiq-auth.aspx?site=" + site.Value + "&return_page=" + return_page.Value);
                }
                else
                {
                    //redirect to local app
                    Response.RedirectPermanent(current_url + "/test.aspx");
                }
            }
        }
    }
}