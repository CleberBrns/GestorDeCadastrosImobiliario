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
    public partial class Erro : Form
    {
        private string msgErro;

        public string getMsgErro
        {
            get { return msgErro; }
            set { msgErro = value; }
        }

        public Erro()
        {
            InitializeComponent();            
        }

        private void Erro_Load(object sender, EventArgs e)
        {
            txtDescricaoErro.Text = Erro;
        }       

    }
}
