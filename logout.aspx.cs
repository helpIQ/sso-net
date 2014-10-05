using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class logout : System.Web.UI.Page
{
    //Replace the API key with your HelpIQ API Key
    private string helpiq_api_key = "";

    // your local login page
    private string default_login_url = "login.aspx";

    //This is the remote authenication URL to call helpIQ. Do not change.
    private string helpiq_remote_url = "http://www.helpdocsonline.com/access/remote/";

    //your helpIQ site URL
    private string custom_helpiq_site = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string helpiq_api_key = "";
            string helpiq_site_url = "";
            if (ConfigurationManager.AppSettings["api_key"] != null)
            {
                helpiq_api_key = ConfigurationManager.AppSettings["api_key"].ToString();
            }
            if (ConfigurationManager.AppSettings["site_url"] != null)
            {
                helpiq_site_url = ConfigurationManager.AppSettings["site_url"].ToString();
            }
            SetupData(helpiq_api_key, helpiq_site_url);
        }
    }

    private void SetupData(string apikey, string siteurl)
    {
        string[] url = Request.Url.ToString().Split('?');
        List<string> split_values = url[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        split_values.RemoveAt(split_values.Count - 1);
        split_values.RemoveAt(0);
        string current_url = "http" + (((!string.IsNullOrEmpty(Request.ServerVariables["HTTPS"]) && Request.ServerVariables["HTTPS"].ToString().ToLower() == "on") ? "s" : "") + "://") + string.Join("/", split_values);
        this.default_login_url = current_url + "/" + this.default_login_url;
        this.helpiq_api_key = apikey;
        this.custom_helpiq_site = siteurl;
        this.logout_helpiq();
    }
    //please destroy your local session data here
    private void helpiq_destroy_local_session()
    {
        Session.Abandon();
    }
    //logout the end-user from helpIQ, and it will redirect to your auth url
    private void logout_helpiq()
    {
        this.helpiq_destroy_local_session();
        string logout_url = this.helpiq_remote_url + "logout/?site=" + this.custom_helpiq_site;
        Response.RedirectPermanent(logout_url);
    }
    
    
}