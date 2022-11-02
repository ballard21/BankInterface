using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobAssignment
{
    public partial class Interview : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Session["fullname"].ToString();
                fullname.InnerText = name;
            }
            else
            {
                //Response.Redirect("Login.aspx");
            }
        }
    }
}