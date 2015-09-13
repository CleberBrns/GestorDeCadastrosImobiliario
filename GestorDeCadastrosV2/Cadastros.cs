using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace GestorDeCadastros
{
    public partial class Cadastros : Form
    {
        public Cadastros()
        {
            //TiposDeAcao
            //1 = Imovel
            //2 = Locador
            //3 = Locatario

            InitializeComponent();

            if (tcCadastros.SelectedTab.Name == "tpLocatario")
            {
                rbTelefone.Checked = true;                
                CarregaCombos();
            }
        }

        #region Ações Banco de Dados
        

        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = MetodosAuxiliares.retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select * from Imoveis where Ativo = 1";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select * from Locadores where Ativo = 1";
            }
            else
            {
                cmd.CommandText = "select * from Locatarios where Ativo = 1";
            }

            sdaDados.SelectCommand = cmd;

            SqlCeCommandBuilder cb = new SqlCeCommandBuilder(sdaDados);         
            sdaDados.UpdateCommand = cb.GetUpdateCommand();                   

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>        
        ///tipoInsercao 1 = Imovel
        ///tipoInsercao 2 = Locador
        ///tipoInsercao 3 = Locatario
        /// </summary>
        /// <param name="tipoInsercao"></param>
        private void InsereDados(int tipoInsercao)
        {
            SqlCeDataAdapter sdaInclusao;
            DataSet dsInclusao;
            DataRow dr = null;
            PreencheDataset(out dsInclusao, out sdaInclusao, tipoInsercao);
            //Cria nova Linha para a inserção.
            dr = dsInclusao.Tables[0].NewRow();

            if (tipoInsercao == 1)
            {
                dr["Endereco"] = txtEndereco.Text.Trim();
                dr["Bairro"] = txtBairro.Text.Trim();
                dr["Cidade"] = txtCidade.Text.Trim();
                dr["Cep"] = mtxtCep.Text.Trim();
                dr["Ativo"] = 1;
            }
            else if (tipoInsercao == 2)
            {
                dr["Locador"] = txtNomeLocador.Text.Trim();
                dr["Cpf"] = FormataDocPrioritario(mtxtCpfLocador.Text.Trim(), 1);
                dr["Cnpj"] = FormataDocPrioritario(mtxtCnpjLocador.Text.Trim(), 2);
                dr["Contato"] = txtContatoLocador.Text.Trim();
                dr["Ativo"] = 1;
            }
            else
            {
                dr["Locatario"] = txtNomeLocatario.Text.Trim();
                dr["CpfLocatario"] = mtxtCpfLocatario.Text.Trim().Replace(",", ".");
                dr["Telefone"] = RetornaTelefoneSelecionado();
                dr["Email"] = txtEmail.Text.Trim();
                dr["Aluguel"] = MetodosAuxiliares.FormataValorSalvar(mtxtAluguel, 1);
                dr["fkIdImovel"] = cbImoveis.SelectedValue.ToString().Trim();
                dr["fkIdLocador"] = cbLocadores.SelectedValue.ToString().Trim();
                dr["Ativo"] = 1;
            }

            dsInclusao.Tables[0].Rows.Add(dr);
            sdaInclusao.Update(dsInclusao);
        }

        #endregion

        #region Métodos Gerais

        private void tcCadastros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcCadastros.SelectedTab.Name == "tpLocatario")
            {
                DadosDefaultLocador();
                DadosDefaultImovel();
                CarregaCombos();
            }
            else if (tcCadastros.SelectedTab.Name == "tpLocador")
            {
                DadosDefaultLocatario();
                DadosDefaultImovel();
            }
            else if (tcCadastros.SelectedTab.Name == "tpImovel")
            {
                DadosDefaultLocatario();
                DadosDefaultLocador();
            }
        }

        private string FormataDocPrioritario(string docPrioritario, int tipoDoc)
        {
            string testeCampo = string.Empty;
            if (tipoDoc == 1)
            {
                testeCampo = docPrioritario.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Trim();
                if (string.IsNullOrEmpty(testeCampo))
                {
                    docPrioritario = string.Empty;
                }
            }
            else
            {
                testeCampo = docPrioritario.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim();
                if (string.IsNullOrEmpty(testeCampo))
                {
                    docPrioritario = string.Empty;
                }
            }

            return docPrioritario;
        }

        #endregion

        #region Validações de Campos

        private bool validaCombo(ComboBox cbCampo)
        {
            bool campoValido = false;

            if (cbCampo.SelectedValue.ToString() != "0")
            {
                campoValido = true;
            }
            else
            {
                campoValido = false;
                errorProvider1.SetError(cbCampo, "O campo deve conter alguma seleção.");
            }

            return campoValido;
        }

        #endregion

        #region Ações Imóvel

        private void btCadastrarIm_Click(object sender, EventArgs e)
        {
            if (MetodosAuxiliares.validaCampoTxt(txtEndereco, errorProvider1))
            {
                if (MetodosAuxiliares.validaCampoTxt(txtBairro, errorProvider1))
                {
                    if (MetodosAuxiliares.validaCampoTxt(txtCidade, errorProvider1))
                    {
                        if (MetodosAuxiliares.validaCampoMtxt(mtxtCep, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtCep, errorProvider1))
                        {
                            try
                            {
                                IncluirImovel();
                                DadosDefaultImovel();
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

        private void IncluirImovel()
        {
            InsereDados(1);
        }

        private void btLimparIm_Click(object sender, EventArgs e)
        {
            DadosDefaultImovel();
        }

        private void DadosDefaultImovel()
        {
            txtEndereco.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCidade.Text = string.Empty;
            mtxtCep.Text = string.Empty;
        }

        #endregion

        #region Ações Locador

        private void btCadastrarLocador_Click(object sender, EventArgs e)
        {
            if (MetodosAuxiliares.validaCampoTxt(txtNomeLocador, errorProvider1))
            {
                if (verificaDocPrioritario())
                {
                    try
                    {
                        IncluirLocador();
                        MessageBox.Show("Cadastro efetuado com Sucesso!");
                        DadosDefaultLocador();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }
            }
        }

        private bool verificaDocPrioritario()
        {
            bool campoValido = false;

            if (rbCpf.Checked == false && rbCnpj.Checked == false)
            {
                errorProvider1.SetError(lblVerificaDocPri, "Favor selecionar um dos documentos para finalizar o cadastro!");
            }
            else
            {
                if (rbCpf.Checked)
                {
                    if (MetodosAuxiliares.validaCampoMtxt(mtxtCpfLocador, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtCpfLocador, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
                else if (rbCnpj.Checked)
                {
                    if (MetodosAuxiliares.validaCampoMtxt(mtxtCnpjLocador, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtCnpjLocador, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
            }

            return campoValido;
        }

        private void IncluirLocador()
        {
            InsereDados(2);
        }

        private void btLimparFromLocador_Click(object sender, EventArgs e)
        {
            DadosDefaultLocador();
        }

        private void DadosDefaultLocador()
        {
            txtNomeLocador.Text = string.Empty;

            mtxtCpfLocador.Text = string.Empty;
            mtxtCpfLocador.BackColor = Color.White;
            mtxtCpfLocador.Enabled = false;

            mtxtCnpjLocador.Text = string.Empty;
            mtxtCnpjLocador.BackColor = Color.White;
            mtxtCnpjLocador.Enabled = false;

            txtContatoLocador.Text = string.Empty;           
            rbCnpj.Checked = false;
            rbCpf.Checked = false;
        }

        private void rbDocPriritario_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCpf.Checked)
            {
                errorProvider1.Clear();
                mtxtCnpjLocador.Text = string.Empty;
                mtxtCnpjLocador.Enabled = false;
                mtxtCnpjLocador.BackColor = Color.White;

                mtxtCpfLocador.Enabled = true;
                mtxtCpfLocador.BackColor = Color.PaleGreen;
            }
            else if (rbCnpj.Checked)
            {
                errorProvider1.Clear();
                mtxtCpfLocador.Text = string.Empty;
                mtxtCpfLocador.Enabled = false;
                mtxtCpfLocador.BackColor = Color.White;

                mtxtCnpjLocador.Enabled = true;
                mtxtCnpjLocador.BackColor = Color.PaleGreen;
            }
        }

        #endregion

        #region Ações Locatário

        private void CarregaCombos()
        {
            DataTable imoveis = CarregaTabelasCombos(1);
            if (imoveis.Rows.Count > 0)
            {
                imoveis.Rows.Add(0, "Por favor, selecione um Imóvel...");
                imoveis.DefaultView.Sort = "Id";

                cbImoveis.DataSource = imoveis.DefaultView;
                cbImoveis.DisplayMember = "Endereco";
                cbImoveis.ValueMember = "Id";               
            }
            else
            {
                CarregaCombosVazios("imóveis", cbImoveis);
            }

            DataTable locadores = CarregaTabelasCombos(2);
            if (locadores.Rows.Count > 0)
            {
                locadores.Rows.Add(0, "Por favor, selecione um Locador...");
                locadores.DefaultView.Sort = "Id";

                cbLocadores.DataSource = locadores.DefaultView;
                cbLocadores.DisplayMember = "Locador";
                cbLocadores.ValueMember = "Id";                
            }
            else
            {
                CarregaCombosVazios("locadores", cbLocadores);
            }

        }

        private DataTable CarregaTabelasCombos(int tipoCombo)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoCombo);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        private void CarregaCombosVazios(string campoCadastro, ComboBox cbParaCadastro)
        {
            DataTable semValores = new DataTable();
            semValores.Columns.Add("Id");
            semValores.Columns.Add("Nome");

            semValores.Rows.Add(0, "Sem " + campoCadastro + " cadastrados. Favor Cadastrar!");

            cbParaCadastro.DataSource = semValores; ;
            cbParaCadastro.DisplayMember = "Nome";
            cbParaCadastro.ValueMember = "Id";

            cbParaCadastro.Enabled = false;
        }

        private void btCadastrarLocatario_Click(object sender, EventArgs e)
        {
            if (MetodosAuxiliares.validaCampoTxt(txtNomeLocatario, errorProvider1))
            {
                if (MetodosAuxiliares.validaCampoMtxt(mtxtCpfLocatario, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtCpfLocatario, errorProvider1))
                {
                    if (verificaTelPrioritario())
                    {
                        if (MetodosAuxiliares.validaCampoMtxt(mtxtAluguel, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtAluguel, errorProvider1))
                        {
                            if (validaCombo(cbImoveis))
                            {
                                if (validaCombo(cbLocadores))
                                {
                                    try
                                    {
                                        IncluirLocatario();
                                        DadosDefaultLocatario();
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

        private bool verificaTelPrioritario()
        {
            bool campoValido = false;

            if (rbTelefone.Checked == false && rbCelular.Checked == false)
            {
                errorProvider1.SetError(lblTelPri, "Favor selecionar e preencher um dos campos para finalizar o cadastro!");
            }
            else
            {
                if (rbTelefone.Checked)
                {
                    if (MetodosAuxiliares.validaCampoMtxt(mtxtTelefone, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtTelefone,errorProvider1))
                    {
                        campoValido = true;
                    }
                }
                else if (rbCelular.Checked)
                {
                    if (MetodosAuxiliares.validaCampoMtxt(mtxtCelular, errorProvider1) && MetodosAuxiliares.verificaPrenchimentoMtxt(mtxtCelular, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
            }

            return campoValido;
        }

        private void IncluirLocatario()
        {
            InsereDados(3);
        }

        private void btAtualizar_Click(object sender, EventArgs e)
        {

        }

        private void btExcluir_Click(object sender, EventArgs e)
        {

        }

        private void btLimparFormLocatario_Click(object sender, EventArgs e)
        {
            DadosDefaultLocatario();
        }

        private void DadosDefaultLocatario()
        {
            txtNomeLocatario.Text = string.Empty;
            mtxtCpfLocatario.Text = string.Empty;
            mtxtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            mtxtAluguel.Text = string.Empty;
            cbImoveis.SelectedValue = "0";
            cbLocadores.SelectedValue = "0";
            rbTelefone.Checked = true;
            mtxtTelefone.BackColor = Color.PaleGreen;
            rbCelular.Checked = false;
            mtxtCelular.BackColor = Color.White;
        }

        private void rbTelPriritario_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTelefone.Checked)
            {
                errorProvider1.Clear();
                mtxtCelular.Text = string.Empty;
                mtxtCelular.Enabled = false;
                mtxtCelular.BackColor = Color.White;

                mtxtTelefone.Enabled = true;
                mtxtTelefone.BackColor = Color.PaleGreen;
               
            }
            else if (rbCelular.Checked)
            {
                errorProvider1.Clear();
                mtxtTelefone.Text = string.Empty;
                mtxtTelefone.Enabled = false;
                mtxtTelefone.BackColor = Color.White;

                mtxtCelular.Enabled = true;
                mtxtCelular.BackColor = Color.PaleGreen;
            }
        }

        private string RetornaTelefoneSelecionado()
        {
            if (rbCelular.Checked)
            {
                return mtxtCelular.Text.Trim();
            }
            else
            {
                return mtxtTelefone.Text.Trim();
            }
        }

        #endregion
    }
}
