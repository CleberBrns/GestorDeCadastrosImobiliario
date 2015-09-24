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
        private int idLocatario;
        private int idLocador;
        private int idImovel;
        private int idTipoAcao;

        public int getIdTipoAcao
        {
            get { return idTipoAcao; }
            set { idTipoAcao = value; }
        }

        public int getIdLocatario
        {
            get { return idLocatario; }
            set { idLocatario = value; }
        }

        public int getIdLocador
        {
            get { return idLocador; }
            set { idLocador = value; }
        }

        public int getIdImovel
        {
            get { return idImovel; }
            set { idImovel = value; }
        }

        public Cadastros()
        {
            //TiposDeCadastro
            //1 = Imovel
            //2 = Locador
            //3 = Locatario

            //TiposDeAção
            //1 = Cadastro
            //2 = Atualizacao

            InitializeComponent();

            Auxiliar.CentralizaControle(tcCadastros, this);

            if (tcCadastros.SelectedTab.Name == "tpLocatario")
            {
                rbTelefone.Checked = true;
                CarregaCombos();
            }
        }

        private void Cadastros_Load(object sender, EventArgs e)
        {
            if (idTipoAcao != 0)
            {
                lblIdTipoAcao.Text = idTipoAcao.ToString();
                btInicio.Visible = false;
            }
            else
            {
                lblIdTipoAcao.Text = "1";
            }

            if (idLocatario != 0)
            {
                lblIdLocatario.Text = idLocatario.ToString();

                tcCadastros.TabPages.Remove(tpLocador);
                tcCadastros.TabPages.Remove(tpImovel);

                btCadastrarLocatario.Visible = false;
                btLimparFormLocatario.Visible = false;

                btAtualizarLocatario.Visible = true;
                btExcluirLocatario.Visible = true;

                CarregaDadosCadastro(idLocatario, 3);

            }
            else if (idLocador != 0)
            {
                lblIdLocador.Text = idLocador.ToString();

                tcCadastros.TabPages.Remove(tpLocatario);
                tcCadastros.TabPages.Remove(tpImovel);

                btCadastrarLocador.Visible = false;
                btLimparFormLocador.Visible = false;

                btAtualizarLocador.Visible = true;
                btExcluirLocador.Visible = true;

                CarregaDadosCadastro(idLocador, 2);
            }
            else if (idImovel != 0)
            {
                lblIdImovel.Text = idImovel.ToString();

                tcCadastros.TabPages.Remove(tpLocatario);
                tcCadastros.TabPages.Remove(tpLocador);

                btCadastrarImovel.Visible = false;
                btLimparFormImovel.Visible = false;

                btAtualizarImovel.Visible = true;
                btExcluirImovel.Visible = true;

                CarregaDadosCadastro(idImovel, 1);
            }

        }

        private void CarregaDadosCadastro(int idCadastro, int tipoCadastro)
        {
            DataTable dadosCadastro = CarregaDadosTabela(idCadastro, tipoCadastro);

            if (dadosCadastro.Rows.Count > 0)
            {
                if (tipoCadastro == 1)
                {
                    txtEndereco.Text = dadosCadastro.DefaultView[0]["Endereco"].ToString();
                    txtBairro.Text = dadosCadastro.DefaultView[0]["Bairro"].ToString();
                    txtCidade.Text = dadosCadastro.DefaultView[0]["Cidade"].ToString();
                    mtxtCep.Text = dadosCadastro.DefaultView[0]["Cep"].ToString();
                }
                else if (tipoCadastro == 2)
                {
                    txtNomeLocador.Text = dadosCadastro.DefaultView[0]["Locador"].ToString();

                    if (!string.IsNullOrEmpty(dadosCadastro.DefaultView[0]["Cpf"].ToString()))
                    {
                        mtxtCpfLocador.Text = dadosCadastro.DefaultView[0]["Cpf"].ToString();
                        rbCpf.Checked = true;
                    }
                    else if (!string.IsNullOrEmpty(dadosCadastro.DefaultView[0]["Cnpj"].ToString()))
                    {
                        mtxtCnpjLocador.Text = dadosCadastro.DefaultView[0]["Cnpj"].ToString();
                        rbCnpj.Checked = true;
                    }

                    txtContatoLocador.Text = dadosCadastro.DefaultView[0]["Contato"].ToString();
                }
                else if (tipoCadastro == 3)
                {
                    CarregaCombos();

                    txtNomeLocatario.Text = dadosCadastro.DefaultView[0]["Locatario"].ToString();
                    mtxtCpfLocatario.Text = dadosCadastro.DefaultView[0]["CpfLocatario"].ToString();
                    VerificaTelefoneCadastrado(dadosCadastro.DefaultView[0]["Telefone"].ToString());
                    txtEmail.Text = dadosCadastro.DefaultView[0]["Email"].ToString();
                    txtAluguel.Text = Auxiliar.FormataValoresExibicao(dadosCadastro.DefaultView[0]["Aluguel"].ToString());
                    cbImoveis.SelectedValue = dadosCadastro.DefaultView[0]["fkIdImovel"].ToString();
                    cbLocadores.SelectedValue = dadosCadastro.DefaultView[0]["fkIdLocador"].ToString();
                }
            }
        }

        private void VerificaTelefoneCadastrado(string telefone)
        {
            if (!string.IsNullOrEmpty(telefone))
            {
                string testaTelefone = telefone.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).Trim();

                if (testaTelefone.Length == 11)
                {
                    mtxtCelular.Text = telefone;
                }
                else if (testaTelefone.Length == 10)
                {
                    mtxtTelefone.Text = telefone;
                }
            }
        }

        private DataTable CarregaDadosTabela(int idCadastro, int tipoCadastro)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoCadastro, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), idCadastro);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }


        #region Ações Banco de Dados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoCadastro"></param>
        /// <param name="tipoAcao"></param>
        /// <param name="idCadastro"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoCadastro, int tipoAcao, int idCadastro)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();

            if (tipoCadastro == 1)
            {
                if (tipoAcao == 2)
                {
                    cmd.CommandText = "select * from Imoveis where Ativo = 1 and Id = " + idCadastro + "";
                }
                else
                {
                    cmd.CommandText = "select * from Imoveis where Ativo = 1";
                }
            }
            else if (tipoCadastro == 2)
            {
                if (tipoAcao == 2)
                {
                    cmd.CommandText = "select * from Locadores where Ativo = 1 and Id = " + idCadastro + "";
                }
                else
                {
                    cmd.CommandText = "select * from Locadores where Ativo = 1";
                }

            }
            else
            {
                if (tipoAcao == 2)
                {
                    cmd.CommandText = "select * from Locatarios where Ativo = 1 and Id = " + idCadastro + "";
                }
                else
                {
                    cmd.CommandText = "select * from Locatarios where Ativo = 1";
                }

            }

            sdaDados.SelectCommand = cmd;

            SqlCeCommandBuilder cb = new SqlCeCommandBuilder(sdaDados);
            sdaDados.UpdateCommand = cb.GetUpdateCommand();

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>        
        ///tipoCadastro 1 = Imovel
        ///tipoCadastro 2 = Locador
        ///tipoCadastro 3 = Locatario
        ///tipoAcao 1 = Cadastro
        ///tipoAcao 2 = Atualizacao
        /// </summary>
        /// <param name="tipoCadastro"></param>
        private void InsereDados(int tipoCadastro, int tipoAcao, int idCadastro)
        {
            try
            {
                SqlCeDataAdapter sdaInclusao;
                DataSet dsInclusao;
                DataRow dr = null;
                PreencheDataset(out dsInclusao, out sdaInclusao, tipoCadastro, tipoAcao, idCadastro);
                //Cria nova Linha para a inserção.
                dr = dsInclusao.Tables[0].NewRow();

                DataRow[] tempdata = dsInclusao.Tables[0].AsEnumerable().Where(p => p["id"].ToString() == idCadastro.ToString()).ToArray();

                if (tipoAcao == 1)
                {
                    if (tipoCadastro == 1)
                    {
                        dr["Endereco"] = txtEndereco.Text.Trim();
                        dr["Bairro"] = txtBairro.Text.Trim();
                        dr["Cidade"] = txtCidade.Text.Trim();
                        dr["Cep"] = mtxtCep.Text.Trim();
                        dr["Ativo"] = 1;
                    }
                    else if (tipoCadastro == 2)
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
                        dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguel);
                        dr["fkIdImovel"] = cbImoveis.SelectedValue.ToString().Trim();
                        dr["fkIdLocador"] = cbLocadores.SelectedValue.ToString().Trim();
                        dr["Ativo"] = 1;
                    }

                    dsInclusao.Tables[0].Rows.Add(dr);
                }
                else if (tipoAcao == 2)
                {
                    if (tipoCadastro == 1)
                    {
                        if (tempdata.Length > 0)
                        {
                            dr = tempdata[0];
                            dr["Endereco"] = txtEndereco.Text.Trim();
                            dr["Bairro"] = txtBairro.Text.Trim();
                            dr["Cidade"] = txtCidade.Text.Trim();
                            dr["Cep"] = mtxtCep.Text.Trim();
                            dr["Ativo"] = 1;
                            //dr.Delete();//Para exclusão
                        }
                    }
                    else if (tipoCadastro == 2)
                    {
                        if (tempdata.Length > 0)
                        {
                            dr = tempdata[0];
                            dr["Locador"] = txtNomeLocador.Text.Trim();
                            dr["Cpf"] = FormataDocPrioritario(mtxtCpfLocador.Text.Trim(), 1);
                            dr["Cnpj"] = FormataDocPrioritario(mtxtCnpjLocador.Text.Trim(), 2);
                            dr["Contato"] = txtContatoLocador.Text.Trim();
                            dr["Ativo"] = 1;
                            //dr.Delete();//Para exclusão
                        }
                    }
                    else
                    {
                        if (tempdata.Length > 0)
                        {
                            dr = tempdata[0];
                            dr["Locatario"] = txtNomeLocatario.Text.Trim();
                            dr["CpfLocatario"] = mtxtCpfLocatario.Text.Trim().Replace(",", ".");
                            dr["Telefone"] = RetornaTelefoneSelecionado();
                            dr["Email"] = txtEmail.Text.Trim();
                            dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguel);
                            dr["fkIdImovel"] = cbImoveis.SelectedValue.ToString().Trim();
                            dr["fkIdLocador"] = cbLocadores.SelectedValue.ToString().Trim();
                            dr["Ativo"] = 1;
                            //dr.Delete();//Para exclusão
                        }
                    }
                }

                sdaInclusao.Update(dsInclusao);

                Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
            }
            catch (Exception ex)
            {
                Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
            }

        }

        private void ExcluirCadastro(int idCadastro)
        {
            //throw new NotImplementedException();
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

        private void btInicio_Click(object sender, EventArgs e)
        {
            Login formInicio = new Login();
            this.Hide();
            formInicio.ShowDialog();
            this.Close();
        }

        private void btInsereAluguel_Click(object sender, EventArgs e)
        {
            InsereValores dlgInsereValores = new InsereValores();
            if (dlgInsereValores.ShowDialog() == DialogResult.OK)
                txtAluguel.Text = dlgInsereValores.getValorInserido;
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

        private void cadastraAtualizaImovel_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaCampoTxt(txtEndereco, errorProvider1))
            {
                if (Auxiliar.validaCampoTxt(txtBairro, errorProvider1))
                {
                    if (Auxiliar.validaCampoTxt(txtCidade, errorProvider1))
                    {
                        if (Auxiliar.validaCampoMtxt(mtxtCep, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCep, errorProvider1))
                        {
                            try
                            {
                                if (sender == btCadastrarImovel)
                                {
                                    IncluirImovel();
                                    DadosDefaultImovel();
                                }
                                else
                                {
                                    AtualizaImovel();
                                    btAtualizarImovel.Enabled = false;
                                }

                            }
                            catch (Exception ex)
                            {
                                Auxiliar.MostraMensagemAlerta("Ocorreu um erro durante a operação e não foi possível finaliza-la com sucesso", 3);
                            }
                        }
                    }
                }
            }
        }

        private void IncluirImovel()
        {
            InsereDados(1, 1, 0);
        }

        private void AtualizaImovel()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                InsereDados(1, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdImovel.Text.Trim()));
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Dados perdidos, não foi possivel atualizar o cadastro", 3);
            }
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

        private void cadastraAtualizaLocador_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaCampoTxt(txtNomeLocador, errorProvider1))
            {
                if (verificaDocPrioritario())
                {
                    try
                    {
                        if (sender == btCadastrarLocador)
                        {
                            IncluirLocador();
                            DadosDefaultLocador();
                        }
                        else
                        {
                            AtualizaLocador();
                            btAtualizarLocador.Enabled = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        Auxiliar.MostraMensagemAlerta("Ocorreu um erro durante a operação e não foi possível finaliza-la com sucesso", 3);
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
                    if (Auxiliar.validaCampoMtxt(mtxtCpfLocador, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCpfLocador, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
                else if (rbCnpj.Checked)
                {
                    if (Auxiliar.validaCampoMtxt(mtxtCnpjLocador, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCnpjLocador, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
            }

            return campoValido;
        }

        private void IncluirLocador()
        {
            InsereDados(2, 1, 0);
        }

        private void AtualizaLocador()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                InsereDados(2, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdLocador.Text.Trim()));
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Dados perdidos, não foi possivel atualizar o cadastro", 3);
            }
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

                cbImoveis.Enabled = true;
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

                cbLocadores.Enabled = true;
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
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoCombo, 0, 0);

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

        private void cadastraAtualizaLocatario_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaCampoTxt(txtNomeLocatario, errorProvider1))
            {
                if (Auxiliar.validaCampoMtxt(mtxtCpfLocatario, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCpfLocatario, errorProvider1))
                {
                    if (verificaTelPrioritario())
                    {
                        if (Auxiliar.validaCampoTxt(txtAluguel, errorProvider1))
                        {
                            if (validaCombo(cbImoveis))
                            {
                                if (validaCombo(cbLocadores))
                                {
                                    try
                                    {
                                        if (sender == btCadastrarLocatario)
                                        {
                                            IncluirLocatario();
                                            DadosDefaultLocatario();
                                        }
                                        else
                                        {
                                            AtualizaLocatario();
                                            btAtualizarLocatario.Enabled = false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Auxiliar.MostraMensagemAlerta("Ocorreu um erro durante a operação e não foi possível finaliza-la com sucesso", 3);
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
                    if (Auxiliar.validaCampoMtxt(mtxtTelefone, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtTelefone, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
                else if (rbCelular.Checked)
                {
                    if (Auxiliar.validaCampoMtxt(mtxtCelular, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCelular, errorProvider1))
                    {
                        campoValido = true;
                    }
                }
            }

            return campoValido;
        }

        private void IncluirLocatario()
        {
            InsereDados(3, 1, 0);
        }

        private void AtualizaLocatario()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                InsereDados(3, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdLocatario.Text.Trim()));
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Dados perdidos, não foi possivel atualizar o cadastro", 3);
            }
        }

        private void btExcluirLocatario_Click(object sender, EventArgs e)
        {
            //ExcluirCadastro(3);
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
            txtAluguel.Text = string.Empty;
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
                mtxtCelular.Focus();

                mtxtTelefone.Enabled = true;
                mtxtTelefone.BackColor = Color.PaleGreen;

            }
            else if (rbCelular.Checked)
            {
                errorProvider1.Clear();
                mtxtTelefone.Text = string.Empty;
                mtxtTelefone.Enabled = false;
                mtxtTelefone.BackColor = Color.White;
                mtxtTelefone.Focus();

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
