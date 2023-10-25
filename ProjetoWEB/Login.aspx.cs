using ProjetoCLS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoWEB
{
    public partial class Login : System.Web.UI.Page
    {
        private string appId = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void AutenticaUtilizador_Click(object sender, EventArgs e)
        {
            
            AppConfig appInfo = AppConfig.AppInfo();

            if (appInfo != null )
            {
                //var readUser = username.Value;
                var readUser = Utilizador.Text;
                var readPass = password.Value;

                #region Login
                // Autentica o utilizador enviando para o método autenticar os dados 
                // inseridos no ecrã e passando a ligação da base de dados que queremos
                // usar para obter os dados do utilizador
                AppUser userApp = new AppUser(appId);
                AppUser user = userApp.Login(readUser, readPass);

                if (user == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "userLoginFailed", "userLoginMessage('warning', 'Login', 'Utilizador não encontrado')", true);
                    mensagem.InnerText = "Utilizador não encontrado!";
                }
                else
                {
                    if (user.Role.ToUpper() == AppUser.Roles.ADMIN.ToUpper())
                    {
                        Session["ProjetoWEB"] = user;
                        Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "userLoginMessage('warning', 'Login', 'Não tem permissões para aceder!')", true);
                        mensagem.InnerText = "Não tem permissões para aceder!";
                    } 
                }
                #endregion
            }
            else
            {
                mensagem.InnerText = "Não existe ligação à base de dados! Tente mais tarde...";
            }
        }
    }
}