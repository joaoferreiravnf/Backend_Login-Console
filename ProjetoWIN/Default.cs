using ProjetoCLS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoWIN
{    
    public partial class Default : Form
    {
        int selectdUserIndex = 0;
        AppUser appUser = new AppUser(Program.appId);
        List<AppUser> userList;

        public Default()
        {
            InitializeComponent();
        }

        private void Default_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projetoCDataSet.tbUtilizadores' table. You can move, or remove it, as needed.
            this.Text = Program.appInfo.Title();


            //TODO: Verificar se existe algum utilizador autenticado
            // e caso não exista, chamo a janela Login

            Login login = new Login();
            login.ShowDialog();

            //Vem para esta linha depois de um login bem sucedido
            login.Dispose();

            this.tsUserStatus.Text += Program.user.Name + " Nível: " +
                Program.user.Role;

            cmbRoles.Items.Add(AppUser.Roles.SUPER);
            cmbRoles.Items.Add(AppUser.Roles.ADMIN);
            cmbRoles.Items.Add(AppUser.Roles.ANALIST);
            cmbRoles.Items.Add(AppUser.Roles.MANAGER);
            cmbRoles.Items.Add(AppUser.Roles.EDITOR);
            cmbRoles.Items.Add(AppUser.Roles.VIEWER);
            cmbRoles.Items.Add(AppUser.Roles.BLOCKED);
        }

        private void btnCarregarDataGridView_Click(object sender, EventArgs e)
        {
            //AppUser appUser = new AppUser(Program.appId);
            userList = appUser.ListUsers();
            dgUsers.DataSource = userList;
        }

        private void dgUsers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            selectdUserIndex = e.RowIndex;

            DataGridViewRow linha = dgUsers.Rows[selectdUserIndex];
            
            textBox1.Text = linha.Cells[3].Value.ToString();
            textBox2.Text = linha.Cells[4].Value.ToString();

            cmbRoles.SelectedItem= linha.Cells[5].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            userList[selectdUserIndex].Name = textBox1.Text;
            userList[selectdUserIndex].Email = textBox2.Text;
            userList[selectdUserIndex].Role = cmbRoles.SelectedItem.ToString();

            appUser.Update(userList[selectdUserIndex]);

            dgUsers.Refresh();
        }
    }
}
