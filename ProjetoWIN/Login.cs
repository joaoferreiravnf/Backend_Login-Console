using ProjetoCLS;
using System;
using System.Windows.Forms;

namespace ProjetoWIN
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.Text = Program.appInfo.Title();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.user == null)
                Application.Exit();            
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            #region Login
            // Autentica o utilizador enviando para o método autenticar os dados 
            // inseridos no ecrã e passando a ligação da base de dados que queremos
            // usar para obter os dados do utilizador
            AppUser userApp = new AppUser(Program.appId);
            Program.user = userApp.Login(txtUserName.Text, txtPassword.Text);

            if (Program.user == null)
            {
                //Console.WriteLine("Utilizador não encontrado!");
                MessageBox.Show("Utilizador não encontrado!", "Login");
            }
            else
            {
                this.Close();
            }
            #endregion
        }
    }
}
