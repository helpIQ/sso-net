using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
        site.Value = Request.QueryString["site"];
        return_page.Value = Request.QueryString["return_page"];
       
    }
    protected void login_Click(object sender, EventArgs e)
    {
        if (username.Text == "demo" && password.Text == "demo")
        {
            Session["usrid"] = "1";
           if (Request.QueryString["site"] != null)// if site parameter is available
           {
               if(Request.QueryString["site"] !="")//if its not empty
                Response.Redirect("helpiq-auth.aspx?site="+Request.QueryString["site"]+"&return_page="+Request.QueryString["return_page"]+"&action=");
            else if(Request.QueryString["site"] =="")//if its empty
               Response.Redirect("test.aspx");
           }
           else
               Response.Redirect("test.aspx");
        }
    }
}