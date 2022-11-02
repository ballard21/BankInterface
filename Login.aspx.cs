using JobAssignment.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobAssignment
{
    public partial class Login : System.Web.UI.Page
    {
        private int t;
        HttpCookie userCookie, userCookies;   // Declare a cookie variable      
        DataTable table = new DataTable();
      
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        string loginApi = System.Configuration.ConfigurationManager.AppSettings["InterviewLogin"];


        protected void Page_Init(object sender, EventArgs e)
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches in the view state form field in the Page.PreLoad
            //method.
            if (requestCookie != null
            && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;
                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL &&
                Request.IsSecureConnection)
                    responseCookie.Secure = true;
                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] =
                Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti - XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                try
                {

                }
                catch (Exception ee)
                {
                    Response.Redirect("Login.aspx", true);

                }
            }
        }
        public static class AntiforgeryChecker
        {
            public static void Check(Page page, HiddenField antiforgery)
            {
                if (!page.IsPostBack)
                {
                    Guid antiforgeryToken = Guid.NewGuid();
                    page.Session["AntiforgeryToken"] = antiforgeryToken;
                    antiforgery.Value = antiforgeryToken.ToString();
                }
                else
                {
                    Guid stored = (Guid)page.Session["AntiforgeryToken"];
                    Guid sent = new Guid(antiforgery.Value);
                    if (sent != stored)
                    {
                        throw new SecurityException("XSRF Attack Detected!");
                    }
                }
            }
        }
        protected void onLogin(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPasswords.Text;
            Encryptor enc = new Encryptor();
            CallLogin(username, enc.ConvertStringtoMD5(password));
            // IsAuthenticated(ldap, username, password);

        }


        public void CallLogin(string username, string password)
        {
            LoginResp resp = new LoginResp();

            try
            {
                HttpClient client1 = new HttpClient();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string urlParameters = "";
                client1.BaseAddress = new Uri(loginApi);

                object bodyData = new
                {
                    username = username,
                    password = password
                };

                var json = JsonConvert.SerializeObject(bodyData);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage response = client1.PostAsync(urlParameters, content).Result;// Blocking call! Program will wait here until a response is received or a timeout occurs.

                if (response.IsSuccessStatusCode)
                {

                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;

                    var result = JsonConvert.DeserializeObject<LoginResp>(finalResult);
                    Session["AccList"] = result.accounts;
                    Session["fullname"] = txtUserName.Text;
                    Response.Redirect("Homes.aspx");
                }

                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    lblmsg.Text = "Error code: 403 Invalid Credentials";
                }
                else
                {
                }

                client1.Dispose();
            }

            catch (Exception ex)
            {
                lblmsg.Text = "" + ex.Message;
            }
        }

        private async Task<string> GetjsonString(HttpResponseMessage response)
        {
            string responseBody = "";
            try
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseBody;
        }
    }
}