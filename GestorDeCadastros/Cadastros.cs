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

            try
            {
                if (tcCadastros.SelectedTab.Name == "tpImovel")
                {
                    if (idTipoAcao == 1)
                        CarregaCombosImovel(0);
                }
                else if (tcCadastros.SelectedTab.Name == "tpLocatario")
                {
                    CarregaCombosLocatario();
                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                Auxiliar.MostraMensagemAlerta("Falha ao carregar os dados.", 3);
                //Auxiliar.MostraFormDeErros(ex.ToString());
            }

        }

        #region Métodos Gerais

        private void Cadastros_Load(object sender, EventArgs e)
        {
            try
            {
                #region Carrega Cadastros

                if (idTipoAcao != 0)
                {
                    lblIdTipoAcao.Text = idTipoAcao.ToString();
                    btInicio.Visible = false;
                    btInicioLcd.Visible = false;
                    btInicioIM.Visible = false;
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

                #endregion
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                Auxiliar.MostraMensagemAlerta("Falha ao carregar os dados.", 3);
                //Auxiliar.MostraFormDeErros(ex.ToString());
            }
        }

        private void CarregaDadosCadastro(int idCadastro, int tipoCadastro)
        {
            DataTable dadosCadastro = CarregaDadosTabela(idCadastro, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), tipoCadastro);

            if (dadosCadastro.Rows.Count > 0)
            {
                if (tipoCadastro == 1)
                {
                    bool com2Locadores = false;
                    if (!string.IsNullOrEmpty(dadosCadastro.DefaultView[0]["fkIdLocador2"].ToString().Trim()) &&
                       dadosCadastro.DefaultView[0]["fkIdLocador2"].ToString().Trim() != "0")
                    {
                        CarregaCombosImovel(Convert.ToInt32(dadosCadastro.DefaultView[0]["fkIdLocador2"].ToString().Trim()));
                        com2Locadores = true;
                    }
                    else
                    {
                        CarregaCombosImovel(0);
                    }

                    txtEndereco.Text = dadosCadastro.DefaultView[0]["Endereco"].ToString();
                    txtBairro.Text = dadosCadastro.DefaultView[0]["Bairro"].ToString();
                    txtCidade.Text = dadosCadastro.DefaultView[0]["Cidade"].ToString();
                    mtxtCep.Text = dadosCadastro.DefaultView[0]["Cep"].ToString();

                    if (!string.IsNullOrEmpty(dadosCadastro.DefaultView[0]["fkIdLocador1"].ToString().Trim()) &&
                        dadosCadastro.DefaultView[0]["fkIdLocador1"].ToString().Trim() != "0")
                    {
                        cbLocadoresImovel1.SelectedValue = dadosCadastro.DefaultView[0]["fkIdLocador1"].ToString();
                    }

                    if (com2Locadores)
                    {
                        cbLocadoresImovel2.SelectedValue = dadosCadastro.DefaultView[0]["fkIdLocador2"].ToString();
                    }

                }
                else if (tipoCadastro == 2)
                {
                    txtNomeLocador.Text = dadosCadastro.DefaultView[0]["Locador"].ToString();
                    mtxtCpfLocador.Text = dadosCadastro.DefaultView[0]["Cpf"].ToString();
                    mtxtCnpjLocador.Text = dadosCadastro.DefaultView[0]["Cnpj"].ToString();
                    mtxtTelResLocador.Text = dadosCadastro.DefaultView[0]["TelResidencial"].ToString();
                    mtxtTelCelLocador.Text = dadosCadastro.DefaultView[0]["Celular"].ToString();

                    CarregaFixoOuCelular(rbFixoComLocador, rbCelComLocador, mtxtFixoComLocador, mtxtCelComLocador,
                                         dadosCadastro.DefaultView[0]["TelComercial"].ToString());

                    txtEmailLocador.Text = dadosCadastro.DefaultView[0]["Email"].ToString();
                }
                else if (tipoCadastro == 3)
                {
                    CarregaCombosLocatario();

                    txtNomeLocatario.Text = dadosCadastro.DefaultView[0]["Locatario"].ToString();
                    mtxtCpfLocatario.Text = dadosCadastro.DefaultView[0]["CpfLocatario"].ToString();
                    mtxtTelefone.Text = dadosCadastro.DefaultView[0]["TelResidencial"].ToString();
                    mtxtCelular.Text = dadosCadastro.DefaultView[0]["Celular"].ToString();

                    CarregaFixoOuCelular(rbFixoCom, rbCelCom, mtxtFixoCom, mtxtCelCom, dadosCadastro.DefaultView[0]["TelComercial"].ToString());

                    CarregaFixoOuCelular(rbCelOutro, rbCelOutro, mtxtFixoOutro, mtxtCelOutro, dadosCadastro.DefaultView[0]["TelComercial"].ToString());

                    txtEmail.Text = dadosCadastro.DefaultView[0]["Email"].ToString();
                    txtAluguel.Text = Auxiliar.FormataValoresExibicao(dadosCadastro.DefaultView[0]["Aluguel"].ToString());
                    cbImoveisLct.SelectedValue = dadosCadastro.DefaultView[0]["fkIdImovel"].ToString();
                }
            }

        }
        private void tcCadastros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcCadastros.SelectedTab.Name == "tpLocatario")
            {
                DadosDefaultLocador();
                DadosDefaultImovel();
                CarregaCombosLocatario();
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

                if (idTipoAcao == 1)
                    CarregaCombosImovel(0);
            }
        }

        /// <summary>
        /// tipoDoc 1 = CPF
        /// tipoDoc 2 = CNPJ
        /// tipoDoc 3 = Telefone
        /// </summary>
        /// <param name="campoSalvar"></param>
        /// <param name="tipoDoc"></param>
        /// <returns></returns>
        private string FormataCampoSalvar(string campoSalvar, int tipoDoc)
        {
            string testeCampo = string.Empty;
            if (tipoDoc == 1)
            {
                testeCampo = campoSalvar.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Trim();
                if (string.IsNullOrEmpty(testeCampo))
                {
                    campoSalvar = string.Empty;
                }
            }
            else if (tipoDoc == 2)
            {
                testeCampo = campoSalvar.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim();
                if (string.IsNullOrEmpty(testeCampo))
                {
                    campoSalvar = string.Empty;
                }
            }
            else
            {
                testeCampo = campoSalvar.Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                if (string.IsNullOrEmpty(testeCampo))
                {
                    campoSalvar = string.Empty;
                }
            }

            return campoSalvar;
        }

        private void btInicio_Click(object sender, EventArgs e)
        {
            Inicio formInicio = new Inicio();
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

        private void btExcluirCadastro_Click(object sender, EventArgs e)
        {
            if (sender == btExcluirLocatario)
            {
                ExcluiCadastro(3, lblIdLocatario.Text.Trim());
            }
            else if (sender == btExcluirLocador)
            {
                ExcluiCadastro(2, lblIdLocador.Text.Trim());
            }
            else if (sender == btExcluirImovel)
            {
                ExcluiCadastro(1, lblIdImovel.Text.Trim());
            }
        }

        private void ExcluiCadastro(int tipoCadastro, string sIdCadastro)
        {
            try
            {
                if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                {
                    if (DialogResult.Yes == MessageBox.Show("O Cadastro será inutilizável após esse processo. Deseja continuar mesmo assim?", "Confirmação",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        RotinaDeDados(tipoCadastro, 3, Convert.ToInt32(sIdCadastro.Trim()));
                        Auxiliar.MostraMensagemAlerta("Cadastro excluído com sucesso!", 1);
                        this.Close();
                    }
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Os dados para exclusão foram perdidos e o procedimento não pode ser completado.", 3);
                }
            }
            catch (Exception ex)
            {
                Auxiliar.MostraMensagemAlerta("Ocorreu um erro e não foi possivel excluir o cadastro.", 3);
            }
        }

        #endregion

        #region Ações Banco de Dados

        private DataTable CarregaDadosTabela(int idCadastro, int idTipoAcao, int tipoCadastro)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoCadastro, idTipoAcao, idCadastro);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

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
                else if (tipoAcao == 4)
                {
                    cmd.CommandText = "select * from Locadores where Ativo = 1 and Id <> " + idCadastro + "";
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
        ///tipoAcao 3 = Exclusão
        ///tipoAcao 4 = Cadastro menos cadastro especifico
        /// </summary>
        /// <param name="tipoCadastro"></param>
        private void RotinaDeDados(int tipoCadastro, int tipoAcao, int idCadastro)
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
                    dr["fkIdLocador1"] = cbLocadoresImovel1.SelectedValue.ToString().Trim();

                    if (cbLocadoresImovel2.SelectedValue.ToString().Trim() != "0")
                    {
                        dr["fkIdLocador2"] = cbLocadoresImovel2.SelectedValue.ToString().Trim();
                    }

                    dr["Ativo"] = 1;
                }
                else if (tipoCadastro == 2)
                {
                    dr["Locador"] = txtNomeLocador.Text.Trim();
                    dr["Cpf"] = FormataCampoSalvar(mtxtCpfLocador.Text.Trim(), 1);
                    dr["Cnpj"] = FormataCampoSalvar(mtxtCnpjLocador.Text.Trim(), 2);
                    dr["TelResidencial"] = FormataCampoSalvar(mtxtTelResLocador.Text.Trim(), 3);
                    dr["Celular"] = FormataCampoSalvar(mtxtTelCelLocador.Text.Trim(), 3);
                    dr["TelComercial"] = RetornaFixoOuCelular(rbFixoComLocador, rbCelComLocador, mtxtFixoComLocador, mtxtCelComLocador);
                    dr["Email"] = txtEmailLocador.Text.Trim();
                    dr["Ativo"] = 1;
                }
                else
                {
                    dr["Locatario"] = txtNomeLocatario.Text.Trim();
                    dr["CpfLocatario"] = mtxtCpfLocatario.Text.Trim().Replace(",", ".");
                    dr["Locatario2"] = txtNomeLocatario2.Text.Trim();
                    dr["CpfLocatario2"] = mtxtCpfLocatario2.Text.Trim().Replace(",", ".");
                    dr["TelResidencial"] = FormataCampoSalvar(mtxtTelefone.Text.Trim(), 3);
                    dr["Celular"] = FormataCampoSalvar(mtxtCelular.Text.Trim(), 3);
                    dr["TelComercial"] = RetornaFixoOuCelular(rbFixoCom, rbCelCom, mtxtFixoCom, mtxtCelCom);
                    dr["TelOutro"] = RetornaFixoOuCelular(rbFixoOutro, rbCelOutro, mtxtFixoOutro, mtxtCelOutro);
                    dr["Email"] = txtEmail.Text.Trim();
                    dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguel);
                    dr["fkIdImovel"] = cbImoveisLct.SelectedValue.ToString().Trim();
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
                        dr["fkIdLocador1"] = cbLocadoresImovel1.SelectedValue.ToString().Trim();

                        if (cbLocadoresImovel2.SelectedValue.ToString().Trim() != "0")
                        {
                            dr["fkIdLocador2"] = cbLocadoresImovel2.SelectedValue.ToString().Trim();
                        }
                        else
                        {
                            dr["fkIdLocador2"] = DBNull.Value;
                        }

                        dr["Ativo"] = 1;
                    }
                }
                else if (tipoCadastro == 2)
                {
                    if (tempdata.Length > 0)
                    {
                        dr = tempdata[0];
                        dr["Locador"] = txtNomeLocador.Text.Trim();
                        dr["Cpf"] = FormataCampoSalvar(mtxtCpfLocador.Text.Trim(), 1);
                        dr["Cnpj"] = FormataCampoSalvar(mtxtCnpjLocador.Text.Trim(), 2);
                        dr["TelResidencial"] = FormataCampoSalvar(mtxtTelResLocador.Text.Trim(), 3);
                        dr["Celular"] = FormataCampoSalvar(mtxtTelCelLocador.Text.Trim(), 3);
                        dr["TelComercial"] = RetornaFixoOuCelular(rbFixoComLocador, rbCelComLocador, mtxtFixoComLocador, mtxtCelComLocador);
                        dr["Email"] = txtEmailLocador.Text.Trim();
                        dr["Ativo"] = 1;
                    }
                }
                else
                {
                    if (tempdata.Length > 0)
                    {
                        dr = tempdata[0];
                        dr["Locatario"] = txtNomeLocatario.Text.Trim();
                        dr["CpfLocatario"] = mtxtCpfLocatario.Text.Trim().Replace(",", ".");
                        dr["Locatario2"] = txtNomeLocatario2.Text.Trim();
                        dr["CpfLocatario2"] = mtxtCpfLocatario2.Text.Trim().Replace(",", ".");
                        dr["TelResidencial"] = FormataCampoSalvar(mtxtTelefone.Text.Trim(), 3);
                        dr["Celular"] = FormataCampoSalvar(mtxtCelular.Text.Trim(), 3);
                        dr["TelComercial"] = RetornaFixoOuCelular(rbFixoCom, rbCelCom, mtxtFixoCom, mtxtCelCom);
                        dr["TelOutro"] = RetornaFixoOuCelular(rbFixoOutro, rbCelOutro, mtxtFixoOutro, mtxtCelOutro);
                        dr["Email"] = txtEmail.Text.Trim();
                        dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguel);
                        dr["fkIdImovel"] = cbImoveisLct.SelectedValue.ToString().Trim();
                        dr["Ativo"] = 1;
                    }
                }
            }
            else if (tipoAcao == 3)
            {
                if (tipoCadastro == 1)
                {
                    if (tempdata.Length > 0)
                    {
                        dr = tempdata[0];
                        dr["Ativo"] = 0;
                        //dr.Delete();//Para exclusão definitiva
                    }
                }
                else if (tipoCadastro == 2)
                {
                    if (tempdata.Length > 0)
                    {
                        dr = tempdata[0];
                        dr["Ativo"] = 0;
                        //dr.Delete();//Para exclusão definitiva
                    }
                }
                else
                {
                    if (tempdata.Length > 0)
                    {
                        dr = tempdata[0];
                        dr["Ativo"] = 0;
                        //dr.Delete();//Para exclusão definitiva
                    }
                }
            }

            sdaInclusao.Update(dsInclusao);

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

        #region Ações Locador

        private void cadastraAtualizaLocador_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaTextBox(txtNomeLocador, errorProvider1))
            {
                if (verificaDocLocador())
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

        private bool verificaDocLocador()
        {
            bool campoValido = false;

            if (Auxiliar.validaMaskedTextBox(mtxtCpfLocador, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCpfLocador, errorProvider1))
            {
                campoValido = true;
            }

            return campoValido;
        }

        private void IncluirLocador()
        {
            try
            {
                RotinaDeDados(2, 1, 0);
                Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
            }
            catch (Exception ex)
            {
                Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
            }
        }

        private void AtualizaLocador()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                try
                {
                    RotinaDeDados(2, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdLocador.Text.Trim()));
                    Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
                }
                catch (Exception ex)
                {
                    Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
                }
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
            mtxtCnpjLocador.Text = string.Empty;
            mtxtTelResLocador.Text = string.Empty;
            mtxtTelCelLocador.Text = string.Empty;
            mtxtFixoComLocador.Text = string.Empty;
            mtxtCelComLocador.Text = string.Empty;
            txtEmailLocador.Text = string.Empty;
        }

        #endregion

        #region Ações Imóvel

        private void CarregaCombosImovel(int idLocador2)
        {
            DataTable locadores = new DataTable();

            if (idLocador2 == 0)
            {
                locadores = CarregaTabelasCombos(2);
            }
            else
            {
                locadores = CarregaDadosTabela(idLocador2, 4, 2);
            }

            if (locadores.Rows.Count > 0)
            {
                locadores.Rows.Add(0, "Por favor, selecione um Locador...");
                locadores.DefaultView.Sort = "Id";

                cbLocadoresImovel1.DataSource = locadores.DefaultView;
                cbLocadoresImovel1.DisplayMember = "Locador";
                cbLocadoresImovel1.ValueMember = "Id";

                cbLocadoresImovel1.Enabled = true;
            }
            else
            {
                CarregaCombosVazios("locadores", cbLocadoresImovel1);
            }
        }

        private void cbLocadoresImovel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool carregarDados = false;
            if (!string.IsNullOrEmpty(cbLocadoresImovel1.SelectedValue.ToString()) && cbLocadoresImovel1.SelectedValue.ToString() != "0"
                && cbLocadoresImovel1.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                string teste = cbLocadoresImovel1.SelectedItem.ToString();
                DataTable locadores2 = CarregaDadosTabela(Convert.ToInt32(cbLocadoresImovel1.SelectedValue.ToString().Trim()), 4, 2);
                if (locadores2.Rows.Count > 0)
                {
                    locadores2.Rows.Add(0, "Caso deseje, selecione um novo Locador...");
                    locadores2.DefaultView.Sort = "Id";

                    cbLocadoresImovel2.DataSource = locadores2.DefaultView;
                    cbLocadoresImovel2.DisplayMember = "Locador";
                    cbLocadoresImovel2.ValueMember = "Id";

                    cbLocadoresImovel2.Enabled = true;

                    carregarDados = true;
                }
            }

            if (!carregarDados)
            {
                DataTable semValores = new DataTable();
                semValores.Columns.Add("Id");
                semValores.Columns.Add("Nome");

                semValores.Rows.Add(0, "Sem cadastros para exibir!");

                cbLocadoresImovel2.DataSource = semValores; ;
                cbLocadoresImovel2.DisplayMember = "Nome";
                cbLocadoresImovel2.ValueMember = "Id";

                cbLocadoresImovel2.Enabled = false;
            }

        }

        private void cadastraAtualizaImovel_Click(object sender, EventArgs e)
        {
            if (Auxiliar.validaTextBox(txtEndereco, errorProvider1))
            {
                if (Auxiliar.validaTextBox(txtBairro, errorProvider1))
                {
                    if (Auxiliar.validaTextBox(txtCidade, errorProvider1))
                    {
                        if (Auxiliar.validaMaskedTextBox(mtxtCep, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCep, errorProvider1))
                        {
                            if (Auxiliar.validaComboBox(cbLocadoresImovel1, errorProvider1))
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
        }

        private void IncluirImovel()
        {
            RotinaDeDados(1, 1, 0);
            Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
        }

        private void AtualizaImovel()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                try
                {
                    RotinaDeDados(1, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdImovel.Text.Trim()));
                    Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
                }
                catch (Exception ex)
                {
                    Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
                }
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
            cbLocadoresImovel1.SelectedValue = "0";

            rbFixoComLocador.Checked = false;
            rbCelComLocador.Checked = false;

            if (cbLocadoresImovel2.Items.Count > 0)
                cbLocadoresImovel2.SelectedValue = "0";
        }

        #endregion

        #region Ações Locatário

        private void CarregaCombosLocatario()
        {
            DataTable imoveis = CarregaTabelasCombos(1);
            if (imoveis.Rows.Count > 0)
            {
                imoveis.Rows.Add(0, "Por favor, selecione um Imóvel...");
                imoveis.DefaultView.Sort = "Id";

                cbImoveisLct.DataSource = imoveis.DefaultView;
                cbImoveisLct.DisplayMember = "Endereco";
                cbImoveisLct.ValueMember = "Id";

                cbImoveisLct.Enabled = true;
            }
            else
            {
                CarregaCombosVazios("imóveis", cbImoveisLct);
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
            if (Auxiliar.validaTextBox(txtNomeLocatario, errorProvider1))
            {
                if (Auxiliar.validaMaskedTextBox(mtxtCpfLocatario, errorProvider1) && Auxiliar.verificaPrenchimentoMtxt(mtxtCpfLocatario, errorProvider1))
                {
                    if (Auxiliar.validaTextBox(txtAluguel, errorProvider1))
                    {
                        if (validaCombo(cbImoveisLct))
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

        private void IncluirLocatario()
        {
            try
            {
                RotinaDeDados(3, 1, 0);
                Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
            }
            catch (Exception ex)
            {
                Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
            }
        }

        private void AtualizaLocatario()
        {
            if (!string.IsNullOrEmpty(lblIdTipoAcao.Text.Trim()) && !string.IsNullOrEmpty(lblIdImovel.Text.Trim()))
            {
                try
                {
                    RotinaDeDados(3, Convert.ToInt32(lblIdTipoAcao.Text.Trim()), Convert.ToInt32(lblIdLocatario.Text.Trim()));
                    Auxiliar.MostraMensagemAlerta("Cadastro concluído com Sucesso!", 1);
                }
                catch (Exception ex)
                {
                    Auxiliar.MostraMensagemAlerta("Ocorreu um erro ao efetuar o Cadastro", 3);
                }

            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Dados perdidos, não foi possivel atualizar o cadastro", 3);
            }
        }

        private void btLimparFormLocatario_Click(object sender, EventArgs e)
        {
            DadosDefaultLocatario();
        }

        private void DadosDefaultLocatario()
        {
            txtNomeLocatario.Text = string.Empty;
            mtxtCpfLocatario.Text = string.Empty;
            txtNomeLocatario2.Text = string.Empty;
            mtxtCpfLocatario2.Text = string.Empty;

            mtxtTelefone.Text = string.Empty;
            mtxtCelular.Text = string.Empty;
            mtxtFixoCom.Text = string.Empty;
            mtxtCelCom.Text = string.Empty;
            mtxtFixoOutro.Text = string.Empty;
            mtxtCelOutro.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAluguel.Text = string.Empty;
            cbImoveisLct.SelectedValue = "0";

            rbFixoCom.Checked = false;
            rbCelCom.Checked = false;

            rbFixoOutro.Checked = false;
            rbCelOutro.Checked = false;
        }

        private void rbFixoOuCelular_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rbFixoCom || sender == rbCelCom)
            {
                SecionaFixoOuCelular(rbFixoCom, rbCelCom, mtxtFixoCom, mtxtCelCom);
            }
            else if (sender == rbFixoOutro || sender == rbCelOutro)
            {
                SecionaFixoOuCelular(rbFixoOutro, rbCelOutro, mtxtFixoOutro, mtxtCelOutro);
            }
            else if (sender == rbFixoComLocador || sender == rbCelComLocador)
            {
                SecionaFixoOuCelular(rbFixoComLocador, rbCelComLocador, mtxtFixoComLocador, mtxtCelComLocador);
            }

        }

        private void SecionaFixoOuCelular(RadioButton rbFixo, RadioButton rbCelular, MaskedTextBox mtxtFixo, MaskedTextBox mtxtCelular)
        {
            if (rbFixo.Checked)
            {
                mtxtFixo.Enabled = true;
                mtxtFixo.BackColor = Color.PaleGreen;

                mtxtCelular.Enabled = false;
                mtxtCelular.BackColor = Color.White;
                mtxtCelular.Text = string.Empty;
            }
            else
            {
                mtxtFixo.Enabled = false;
                mtxtFixo.BackColor = Color.White;
                mtxtFixo.Text = string.Empty;

                mtxtCelular.Enabled = true;
                mtxtCelular.BackColor = Color.PaleGreen;
            }
        }

        private string RetornaFixoOuCelular(RadioButton rbFixo, RadioButton rbCelular, MaskedTextBox mtxtFixo, MaskedTextBox mtxtCelular)
        {
            string celOuFixo = string.Empty;

            if (rbFixo.Checked)
            {
                celOuFixo = mtxtFixo.Text.Trim();
            }
            else if (rbCelular.Checked)
            {
                celOuFixo = mtxtCelular.Text.Trim();
            }

            return FormataCampoSalvar(celOuFixo, 3);
        }

        private void CarregaFixoOuCelular(RadioButton rbFixo, RadioButton rbCelular, MaskedTextBox mtxtFixo, MaskedTextBox mtxtCelular, string celOuFixo)
        {
            if (!string.IsNullOrEmpty(celOuFixo.Trim()))
            {
                if (celOuFixo.Length == 11)
                {
                    mtxtFixo.Text = celOuFixo;
                    rbFixo.Checked = true;

                    mtxtCelular.Text = string.Empty;
                    rbCelular.Checked = false;
                }
                else if (celOuFixo.Length == 12)
                {
                    mtxtCelular.Text = celOuFixo;
                    rbCelular.Checked = true;

                    mtxtFixo.Text = string.Empty;
                    rbFixo.Checked = false;
                }
            }
        }

        #endregion

    }
}
