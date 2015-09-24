using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GestorDeCadastros
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Auxiliar.CentralizaControle(panel1, this);
        }

        private void btCadastros_Click(object sender, EventArgs e)
        {            
            Cadastros formCadastros = new Cadastros();
            this.Hide();
            formCadastros.ShowDialog();
            this.Close();            
        }

        private void btConsultas_Click(object sender, EventArgs e)
        {           
            Consultas formConsultas = new Consultas();
            this.Hide();
            formConsultas.ShowDialog();            
            this.Close();
        }

    }
}
