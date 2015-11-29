using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace GestorDeCadastros
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btAcessar_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaTextBox(txtLogin, errorProvider1))
            {
                if (Auxiliar.validaTextBox(txtSenha, errorProvider1))
                {
                    ValidaLogin(txtLogin.Text.Trim(), txtSenha.Text.Trim());
                }
            }
        }

        private void ValidaLogin(string loginAcesso, string senhaAcesso)
        {
            string usuario = Convert.ToString(ConfigurationSettings.AppSettings["Usuario"]);
            string senha = Convert.ToString(ConfigurationSettings.AppSettings["Senha"]);

            if (usuario == loginAcesso && senha == senhaAcesso)
            {
                Inicio formInicio = new Inicio();
                this.Hide();
                formInicio.ShowDialog();
                this.Close();
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Login e/ou Senha inválidas", 3);
            }
           
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            txtLogin.Text = string.Empty;
            txtSenha.Text = string.Empty;
        }



    }
}
