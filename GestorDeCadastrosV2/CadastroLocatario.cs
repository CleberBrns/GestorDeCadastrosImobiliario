using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;


namespace GestorDeCadastros_V2
{
    public partial class CadastroLocatario : Form
    {
        public static SqlCeDataAdapter sda = null;        

        public CadastroLocatario()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            //CurdOperation();
        }

        private static SqlCeConnection retornaConexao()
        {
            SqlCeConnection conexao = new SqlCeConnection("Data Source="
               + System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "GestorCadastros.sdf"));

            return conexao;
        }

        private static void CurdOperation()
        {           
            sda = new SqlCeDataAdapter();
            SqlCeCommand cmd = retornaConexao().CreateCommand();
            cmd.CommandText = "select * from Cadastros where Ativo = 1";
            sda.SelectCommand = cmd;

            SqlCeCommandBuilder cb = new SqlCeCommandBuilder(sda);
            sda.InsertCommand = cb.GetInsertCommand();
            sda.UpdateCommand = cb.GetUpdateCommand();
            sda.DeleteCommand = cb.GetDeleteCommand();
        }

        private void btCadastrar_Click(object sender, EventArgs e)
        {
            if (validaCampoTxt(txtNomeLocatario))
            {
                if (validaCampoMtxt(mtxtCpfLocatario))
                {
                    if (validaCampoTxt(txtEnderecoImovel))
                    {
                        if (validaCampoTxt(txtCidade))
                        {
                            if (validaCampoMtxt(mtxtCep))
                            {
                                if (validaCampoMtxt(mtxtTelefone))
                                {
                                    if (validaCampoMtxt(mtxtAluguel))
                                    {
                                        if (validaCampoTxt(txtNomeLocador))
                                        {
                                            if (validaCampoMtxt(mtxtCpfLocador))
                                            {
                                                try
                                                {
                                                    Incluir();
                                                    DadosDefault();
                                                    MessageBox.Show("Cadastro efetuado com Sucesso!");
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private bool validaCampoTxt(TextBox txtCampo)
        {
            bool campoValido = false;

            if (!string.IsNullOrEmpty(txtCampo.Text))
            {
                campoValido = true;
                errorProvider1.SetError(txtCampo, string.Empty);
            }
            else
            {
                campoValido = false;
                errorProvider1.SetError(txtCampo, "O campo não pode ser salvo vazio");
            }

            return campoValido;
        }

        private bool validaCampoMtxt(MaskedTextBox MtxtCampo)
        {
            bool campoValido = false;
            string valorCampo = MtxtCampo.Text.Trim();

            if (MtxtCampo.Text.Contains(',') || MtxtCampo.Text.Contains('.') || MtxtCampo.Text.Contains('-'))
            {
                valorCampo = valorCampo.Replace(",", "").Replace(".", string.Empty).Replace("-", string.Empty).Trim();
            }

            if (!string.IsNullOrEmpty(valorCampo))
            {
                campoValido = true;
                errorProvider1.SetError(MtxtCampo, string.Empty);
            }
            else
            {
                campoValido = false;
                errorProvider1.SetError(MtxtCampo, "O campo deve ser preenchido");
            }

            return campoValido;
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            DadosDefault();
        }

        private void Incluir()
        {
            DataSet oldData;
            DataRow dr;
            PreencheDataset(out oldData, out dr);
            //Cria nova Linha para a inserção.
            dr = oldData.Tables[0].NewRow();

            dr["Locatario"] = txtNomeLocatario.Text.Trim();
            dr["CpfLocatario"] = mtxtCpfLocatario.Text.Trim().Replace(",", ".");
            dr["EnderecoImovel"] = txtEnderecoImovel.Text.Trim();
            dr["Cidade"] = txtCidade.Text.Trim();
            dr["Cep"] = mtxtCep.Text.Trim();
            dr["Telefone"] = mtxtTelefone.Text.Trim();
            dr["Email"] = txtEmail.Text.Trim();
            dr["Locador"] = txtNomeLocador.Text.Trim();
            dr["CpfLocador"] = mtxtCpfLocador.Text.Trim().Replace(",", ".");
            dr["Aluguel"] = FormataValor(mtxtAluguel.Text.Trim());
            dr["Ativo"] = 1;

            oldData.Tables[0].Rows.Add(dr);
            sda.Update(oldData);
        }

        private object FormataValor(string valorFormatar)
        {
            decimal dValor = 0;
            string valorFinal = string.Empty;

            valorFinal = valorFormatar.Replace(".", string.Empty).Replace(",", string.Empty).Trim();

            if (valorFinal.Length == 5)
            {
                valorFinal = valorFormatar.Replace(".", string.Empty).Trim();
            }
            else
            {
                valorFinal = valorFormatar.Trim();
            }

            dValor = Convert.ToDecimal(valorFinal);

            return dValor;
        }

        private static void PreencheDataset(out DataSet oldData, out DataRow dr)
        {
            oldData = new DataSet();
            dr = null;
            sda.Fill(oldData);
        }

        private static void PreencheDadosCadastro(out DataSet oldData, int idCadastro)
        {
            SqlCeDataAdapter sdaCadastro = null;
     
            sdaCadastro = new SqlCeDataAdapter();
            SqlCeCommand cmd = retornaConexao().CreateCommand();
            cmd.CommandText = "select * from Cadastros where Id = " + idCadastro + " and Ativo = 1";
            sdaCadastro.SelectCommand = cmd;

            oldData = new DataSet();
            sdaCadastro.Fill(oldData);
        }

        private void DadosDefault()
        {
            txtNomeLocador.Text = string.Empty;
            mtxtCpfLocatario.Text = string.Empty;
            txtEnderecoImovel.Text = string.Empty;
            txtCidade.Text = string.Empty;
            mtxtCep.Text = string.Empty;
            mtxtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            mtxtAluguel.Text = string.Empty;
            txtNomeLocador.Text = string.Empty;
            mtxtCpfLocador.Text = string.Empty;

            lblIdCadastro.Text = string.Empty;

            btCadastrar.Visible = true;
            btLimparFormCadastro.Visible = true;

            btAtualizar.Visible = false;
            btExcluir.Visible = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tpConsulta")
            {
                CarregaGrid();
                DadosDefault();
            }

        }

        private void CarregaGrid()
        {
            SqlCeDataAdapter sdaGrid = new SqlCeDataAdapter();
            SqlCeCommand cmd = retornaConexao().CreateCommand();
            cmd.CommandText = "select Id, Locatario, CpfLocatario, EnderecoImovel, Cidade, Telefone, Locador, Aluguel from Cadastros where Ativo = 1";
            sdaGrid.SelectCommand = cmd;

            DataSet _ds = new DataSet();
            sdaGrid.Fill(_ds);
            if (_ds.Tables.Count > 0)
            {
                dgvCadastros.DataSource = _ds.Tables[0];
            }
        }

        private void dgvCadastros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCadastros.Rows.Count > 0 && e.RowIndex != -1)
            {
                if (dgvCadastros.Rows[e.RowIndex].Cells[0].Selected)
                {
                    string sIdCadastro = dgvCadastros.Rows[e.RowIndex].Cells[1].Value.ToString();
                    //txtAge.Text = dgvCadastros.Rows[e.RowIndex].Cells[2].Value.ToString();                  

                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "-1")
                    {
                        btCadastrar.Visible = false;
                        btLimparFormCadastro.Visible = false;

                        btAtualizar.Visible = true;
                        btExcluir.Visible = true;

                        CarregaDadosCadastro(Convert.ToInt32(sIdCadastro.Trim()));
                        tabControl1.SelectTab("tpCadastro");
                    }

                    //MessageBox.Show(sIdCadastro);

                }
            }
        }

        private void CarregaDadosCadastro(int idCadastro)
        {
            DataSet oldData;
            PreencheDadosCadastro(out oldData, idCadastro);

            DataView dvCadastro = oldData.Tables[0].DefaultView;
            if (dvCadastro.Count > 0)
            {
                txtNomeLocatario.Text = dvCadastro[0]["Locatario"].ToString();
                mtxtCpfLocatario.Text = dvCadastro[0]["CpfLocatario"].ToString();
                txtEnderecoImovel.Text = dvCadastro[0]["EnderecoImovel"].ToString();
                txtCidade.Text = dvCadastro[0]["Cidade"].ToString();
                mtxtCep.Text = dvCadastro[0]["Cep"].ToString();
                mtxtTelefone.Text = dvCadastro[0]["Telefone"].ToString();
                txtEmail.Text = dvCadastro[0]["Email"].ToString();
                mtxtAluguel.Text = dvCadastro[0]["Aluguel"].ToString();
                txtNomeLocador.Text = dvCadastro[0]["Locador"].ToString();
                mtxtCpfLocador.Text = dvCadastro[0]["CpfLocador"].ToString();

                lblIdCadastro.Text = idCadastro.ToString();
            }

        }

        private void btAtualizar_Click(object sender, EventArgs e)
        {

        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblIdCadastro.Text.Trim()))
                {
                    SqlCeDataAdapter sdaExclusao = new SqlCeDataAdapter();
                    SqlCeCommand cmd = retornaConexao().CreateCommand();
                    cmd.CommandText = "update Cadastros set Ativo = 0 where Id = " + lblIdCadastro.Text.Trim() + "";
                    sdaExclusao.SelectCommand = cmd;

                    DadosDefault();
                    MessageBox.Show("Cadastro excluído com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
