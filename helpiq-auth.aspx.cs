using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

public partial class helpiq_auth : System.Web.UI.Page
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
        this.do_helpiq_authorization();
    }

    // please check your end-user has logged in here
	private bool helpiq_check_local_session()
	{
		bool result = (Session["user_id"]!=null) && (Session["user_id"].ToString()!="");
        return result;
	}

	//please destroy your local session data here
	private void helpiq_destroy_local_session() 
	{
        Session.Abandon();
	}

	private void do_helpiq_authorization()
	{
        //If Logout URL is entered in HelpIQ the 'log-out' link can destroy the end-users session in HelpIQ and the session on your web application. 
        string action = Request["action"]!=null ? Request["action"].ToString() : "login";
	
        string redirect_url = this.default_login_url;

        //When the Logout URL is empty, end-user logged out from HelpIQ site, 
		//it will pass the logged_out parameter to tell customer's web app don't to give the end-user access again, just redirect to local login page
        var logged_out = (Request["logged_out"]!=null)?Convert.ToBoolean(Request["logged_out"]):false;

        if (action == "logout" || action == "custom_logout")
        {
            this.helpiq_destroy_local_session();
            redirect_url = this.default_login_url;
        }
        else
        {
            ////your helpIQ site URL
            string site = (Request["site"] != null) ? Request["site"].ToString() : "";
           
            ////return_page is passed by helpIQ, it will redirect the end-user to a specific page HelpIQ
            string return_url = (Request["return_page"] != null) ? Request["return_page"].ToString() : "";
            
            //// please check your end-user has logged in here
            string url_params = "site=" + site + "&return_page=" + return_url;
            
            if (!logged_out && this.helpiq_check_local_session())
            {
                // if the end-user has logged in the customer's website/web application, call HelpIQ to estbalish a session
                redirect_url = this.helpiq_remote_url + "?hash=" + this.helpiq_md5_hash(this.helpiq_api_key) + "&" + url_params;
            }
            else
            {
                if (Request["contextual"] != null && Request["contextual"].ToString() != "")
                {
                    //if the refer page is a contextual help(lightbox/tooltip), redirect to show permission limit	
                    redirect_url = this.helpiq_remote_url + "permission_limit/?login=false&" + url_params;
                }
                else
                {
                    //redirect to your local application login page
                    redirect_url = this.default_login_url + "?" + url_params;
                }
            }
        }

       
        Response.Redirect(redirect_url);
        
	}


    //Code to create MD5 hash

    public string helpiq_md5_hash(string strInput)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strInput);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    
}