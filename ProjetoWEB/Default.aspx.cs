using ProjetoCLS;
using System;
using System.Web.UI;

namespace ProjetoWEB
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                AppUser user = Session["ProjetoWeb"] as AppUser;

                if (user == null || user.Role.ToUpper() != AppUser.Roles.ADMIN.ToUpper())
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    welcome.InnerText = "Bem-vindo, " + user.Name;
                    
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "userLoginSuccess", $"userLoginMessage('success', 'Login', 'Bem-vindo, {user.Name}!')", true);
                }
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx");
        }
    }
}