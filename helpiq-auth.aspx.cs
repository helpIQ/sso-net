using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

public partial class helpiq_auth : System.Web.UI.Page
{

	//Replace the API key with your HelpIQ API Key
	private string helpiq_api_key =  "64ps4xoq2ut33mzzebrb8zfqmj8vzdj6";

	// your local login page
	private string default_login_url = "default.aspx";

	//This is the remote authenication URL to call helpIQ. Do not change.
	private string helpiq_remote_url = "http://www.helpdocsonline.com/access/remote/";

    protected void Page_Load(object sender, EventArgs e)
    {
		this.do_helpiq_authorization();   
    }


    // please check your end-user has logged in here
	private bool helpiq_check_local_session()
	{
		return Convert.ToString(Session["usrid"]) != "";
	}

	//please destroy your local session data here
	private void helpiq_destroy_local_session() 
	{
		Session.Clear();
	}

	private void do_helpiq_authorization()
	{
		string redirect_url = this.default_login_url;
		if (Request.QueryString["action"] == "logout")
		{
			//please destroy your local session data here
			this.helpiq_destroy_local_session();
		    //redirect to your local login page
		    redirect_url = this.default_login_url;
		} else {
			//your helpIQ site URL
			string site = Request.QueryString["site"];
			//return_page is passed by helpIQ, it will redirect the end-user to a specific page HelpIQ
			string return_page = Request.QueryString["return_page"];

			// please check your end-user has logged in here
			if (this.helpiq_check_local_session())
			{
				redirect_url = this.helpiq_remote_url + "?hash=" + this.helpiq_md5_hash(this.helpiq_api_key) + "&site=" + site + "&return_page" + return_page;
			} else {
			    // the user does not log in
				// redirect to your local login page
				redirect_url = this.default_login_url + "?site=" + site + "&return_page=" + return_page;
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