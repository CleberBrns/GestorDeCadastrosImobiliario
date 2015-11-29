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
using System.Configuration;

namespace GestorDeCadastros
{
    public partial class Recibos : Form
    {
        private int idLocatario;

        public int getIdLocatario
        {
            get { return idLocatario; }
            set { idLocatario = value; }
        }

        public Recibos()
        {
            InitializeComponent();
        }

        private void Recibos_Load(object sender, EventArgs e)
        {
            try
            {
                //idLocatario = 2;
                lblIdLocatario.Text = idLocatario.ToString();
                CarregaDadosReciboPrincipal();
                tcRecibos.TabPages.Remove(tpLocador);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Auxiliar.MostraMensagemAlerta("Não foi possivel carregar dados para essa pesquisa", 2);
            }
        }

        #region Ações Banco de Dados

        /// <summary>
        /// tipoTabela 1 = Recibo Principal
        /// tipoTabela 2 = Recibo Locador
        /// tipoTabela 3 = Locatarios (Quando ainda não existem Recibos Cadastrados)
        /// tipoTabela 4 = Recibo Principal + Locadores
        /// tipoTabela 5 = Locadores por Id
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        /// <param name="idBusca"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela, string idBusca)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select top (1) * from RecibosPrincipais order by Id desc";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select top (1) * from RecibosLocadores order by Id desc";
            }
            else if (tipoTabela == 3)
            {
                cmd.CommandText = "select Lc.Locatario, Lc.Locatario2, Lc.Aluguel from Locatarios Lc where Ativo = 1 and Id = " + idBusca + "";
            }
            else if (tipoTabela == 4)
            {
                cmd.CommandText = "select top(1) Lct.Locatario, Lct.Locatario2, Lct.Aluguel, Rp.Quantidade, Rp.Numero, Rp.Iptu, Rp.ParcelasIptu, Rp.DespesaCondominio, Rp.Luz, Rp.Agua," +
                                  " Rp.Data, Rp.Id as IdRecibo, Rp.NumeroParcelaIptu, Rp.Multa, Lct.fkIdImovel, im.fkIdLocador1, im.fkIdLocador2" +
                                  " from Locatarios Lct" +
                                  " inner join RecibosPrincipais Rp on Lct.Id = Rp.fkIdLocatario" +
                                  " inner join Imoveis im on Lct.fkIdImovel = im.Id" +
                                  " where Lct.Ativo = 1 and Lct.Id = " + idBusca + " order by Rp.Data desc";

            }
            else if (tipoTabela == 5)
            {
                cmd.CommandText = "select Locador, Id from Locadores where id in (" + idBusca + ") and Ativo = 1 order by Locador";

            }

            sdaDados.SelectCommand = cmd;

            if (tipoTabela == 1 || tipoTabela == 2)
            {
                SqlCeCommandBuilder cb = new SqlCeCommandBuilder(sdaDados);
                sdaDados.UpdateCommand = cb.GetUpdateCommand();
            }

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>        
        ///tipoInsercao 1 = Recibo Principal
        ///tipoInsercao 2 = Recibo Locador
        /// </summary>
        /// <param name="tipoInsercao"></param>
        private void InsereDados(int tipoInsercao)
        {
            SqlCeDataAdapter sdaInclusao;
            DataSet dsInclusao;
            DataRow dr = null;
            PreencheDataset(out dsInclusao, out sdaInclusao, tipoInsercao, lblIdLocatario.Text.Trim());
            //Cria nova Linha para a inserção.
            dr = dsInclusao.Tables[0].NewRow();

            if (tipoInsercao == 1)
            {
                dr["fkIdLocatario"] = lblIdLocatario.Text.Trim();
                dr["Quantidade"] = nudQtdRecibos.Value.ToString().Trim();
                dr["Numero"] = nudNumRecibo.Value.ToString();
                dr["Data"] = DateTime.Now;
                dr["Periodo"] = Convert.ToDateTime(dtpInicial.Value).ToShortDateString() + " a " + Convert.ToDateTime(dtpFinal.Value).ToShortDateString();
                dr["Iptu"] = RetornaValorCamposOpcionais(txtIptuRcPrincipal);
                dr["ParcelasIptu"] = nudParcelasIptu.Value.ToString();
                dr["NumeroParcelaIptu"] = nudNumeroParcIptu.Value.ToString();
                dr["DespesaCondominio"] = RetornaValorCamposOpcionais(txtDespCondRcPrincipal);
                dr["Luz"] = Auxiliar.FormataValorParaUso(txtLuz);
                dr["Agua"] = Auxiliar.FormataValorParaUso(txtAgua);
                dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguelRcPrincipal);
                dr["ComplementoPagamento"] = RetornaValorCamposOpcionais(txtCompPagtoRP);
                dr["DescricaoComplementoPagamento"] = txtDescricaoCompPagto.Text.Trim();
                dr["Multa"] = RetornaValorCamposOpcionais(txtMultaRP);
                dr["Observacao"] = txtObservacao.Text.Trim();
                dr["DataPagamento"] = Convert.ToDateTime(dtpVencimento.Value).ToShortDateString();
                dr["TotalPagamento"] = Auxiliar.FormataValorParaUso(txtTotalRcPrincipal);
                dr["ExtensoTotalPagamento"] = txtExtensoValorTotal.Text.Trim();

                if (pnlReajuste.Visible == true)
                {
                    dr["ReajusteAluguel"] = lblReajuste.Text.Trim() + " " + Convert.ToDateTime(dtpReajusteAluguel.Value).ToShortDateString();
                }
                else
                {
                    dr["ReajusteAluguel"] = string.Empty;
                }

            }
            else if (tipoInsercao == 2)
            {
                dr["Aluguel"] = Auxiliar.FormataValorParaUso(txtAluguelRcLocador);
                dr["PorcentagemMulta"] = Convert.ToInt32(nudPctMulta.Value);
                dr["Multa"] = RetornaValorCamposOpcionais(txtMultaRL);
                dr["PorcentagemComissao"] = Convert.ToInt32(nudPctComissao.Value);
                dr["Comissao"] = Auxiliar.FormataValorParaUso(txtComissao);
                dr["Complemento"] = RetornaValorCamposOpcionais(txtComp1RL);
                dr["DescricaoComplemento"] = txtDescComp1RL.Text.Trim();
                dr["Complemento2"] = RetornaValorCamposOpcionais(txtComp2RL);
                dr["DescricaoComplemento2"] = txtDescComp2RL.Text.Trim();
                dr["Complemento3"] = RetornaValorCamposOpcionais(txtComp3RL);
                dr["DescricaoComplemento3"] = txtDescComp3RL.Text.Trim();
                dr["Total"] = Auxiliar.FormataValorParaUso(txtTotalRcLocador);
                dr["fkIdRecibo"] = Convert.ToInt32(lblIdRecibo.Text.Trim());
            }

            dsInclusao.Tables[0].Rows.Add(dr);
            sdaInclusao.Update(dsInclusao);
        }

        private object RetornaValorCamposOpcionais(TextBox txtValor)
        {
            if (string.IsNullOrEmpty(txtValor.Text.Trim()))
            {
                return 0;
            }
            else
            {
                return Auxiliar.FormataValorParaUso(txtValor);
            }
        }

        private decimal RetornaNumeroContagemAtual(int numeroAnterior)
        {
            return numeroAnterior + 1;
        }

        private string RetornaDataAtual()
        {
            return DateTime.Now.ToShortDateString();
        }

        private DataTable CarregaDadosTabela(int tipoPesquisa, string idPesquisa)
        {
            DataTable tabelaDados = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoPesquisa, idPesquisa);

            tabelaDados = dsSelecao.Tables[0];

            return tabelaDados;
        }

        #endregion

        #region Ações Gerais

        private void ckbDesmarcaComplementos_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == ckbDesmarcaCompRP)
            {
                rbAcrescimoRP.Checked = false;
                rbDescontoRP.Checked = false;
                txtCompPagtoRP.BackColor = Color.White;
                txtCompPagtoRP.Text = string.Empty;
            }
        }

        private decimal RetornaAcrescimoOuDesconto(RadioButton rbAcrescimo, RadioButton rbDesconto, TextBox txtValorInserir, decimal valorCalular)
        {
            if (!string.IsNullOrEmpty(txtValorInserir.Text.Trim()))
            {
                if (rbAcrescimo.Checked)
                {
                    if (Auxiliar.validaTextBox(txtCompPagtoRP, errorProvider1))
                    {
                        valorCalular = valorCalular + Auxiliar.FormataValorParaUso(txtValorInserir);
                    }
                }
                else if (rbDesconto.Checked)
                {
                    if (Auxiliar.validaTextBox(txtCompPagtoRP, errorProvider1))
                    {
                        valorCalular = valorCalular - Auxiliar.FormataValorParaUso(txtValorInserir);
                    }
                }
            }
            return valorCalular;
        }

        private decimal RetornaAcrescimoOuDesconto(CheckBox ckbDesconto, TextBox txtValorInserir, decimal valorCalular)
        {
            if (!string.IsNullOrEmpty(txtValorInserir.Text.Trim()))
            {
                if (ckbDesconto.Checked)
                {
                    if (Auxiliar.validaTextBox(txtValorInserir, errorProvider1))
                    {
                        valorCalular = valorCalular - Auxiliar.FormataValorParaUso(txtValorInserir);
                    }
                }
                else
                {
                    if (Auxiliar.validaTextBox(txtValorInserir, errorProvider1))
                    {
                        valorCalular = valorCalular + Auxiliar.FormataValorParaUso(txtValorInserir);
                    }
                }
            }
            return valorCalular;
        }

        private string calculaPorcentagem(decimal porcentagem, string valorCalcular)
        {
            decimal valorPorcentagem = 0;

            if (valorCalcular.Contains("."))
                valorCalcular = valorCalcular.Replace(".", string.Empty).Trim();

            valorPorcentagem = Convert.ToDecimal(valorCalcular);
            valorPorcentagem = (valorPorcentagem * porcentagem / 100);

            return Auxiliar.FormataValoresExibicao(valorPorcentagem.ToString());
        }

        /// <summary>
        /// tipoPreview 1 = Preview RP
        /// tipoPreview 2 = Preview RL
        /// </summary>
        /// <param name="tipoPreview"></param>
        private void HabilitaPreviewImpressao(int tipoPreview)
        {
            try
            {
                if (tipoPreview == 1)
                {
                    DataTable dtRP = CarregaDadosTabela(1, string.Empty);
                    lblIdReciboPrincipal.Text = dtRP.DefaultView[0]["Id"].ToString();
                    btPreviewRP.Visible = true;
                }
                else
                {
                    DataTable dtRL = CarregaDadosTabela(2, string.Empty);
                    lblIdReciboLocador.Text = dtRL.DefaultView[0]["Id"].ToString();
                    btPreviewRL.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void ModalInsereValores(TextBox txtParaInsercao)
        {
            InsereValores dlgInsereValores = new InsereValores();
            if (dlgInsereValores.ShowDialog() == DialogResult.OK)
                txtParaInsercao.Text = dlgInsereValores.getValorInserido;
        }

        private void btInicio_Click(object sender, EventArgs e)
        {
            Inicio formInicio = new Inicio();
            this.Hide();
            formInicio.ShowDialog();
            this.Close();
        }

        private void tcRecibos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcRecibos.SelectedTab == tpLocador)
            {
                CarregaDadosReciboLocador();
            }
        }

        private void btInsereValores_Click(object sender, EventArgs e)
        {
            if (sender == btIptuRcPrincipal)
            {
                ModalInsereValores(txtIptuRcPrincipal);
            }
            else if (sender == btAluguelRcPrincipal)
            {
                ModalInsereValores(txtAluguelRcPrincipal);
            }
            else if (sender == btLuz)
            {
                ModalInsereValores(txtLuz);
            }
            else if (sender == btAgua)
            {
                ModalInsereValores(txtAgua);
            }
            else if (sender == btCompPagto)
            {
                ModalInsereValores(txtCompPagtoRP);
            }
            else if (sender == btTotalRcPrincipal)
            {
                ModalInsereValores(txtTotalRcPrincipal);
            }
            else if (sender == btAluguelRcLocador)
            {
                ModalInsereValores(txtAluguelRcLocador);
            }
            else if (sender == btMultaRL)
            {
                ModalInsereValores(txtMultaRL);
            }
            else if (sender == btComissao)
            {
                ModalInsereValores(txtComissao);
            }
            else if (sender == btComp1RL)
            {
                ModalInsereValores(txtComp1RL);
            }
            else if (sender == btComp2RL)
            {
                ModalInsereValores(txtComp2RL);
            }
            else if (sender == btComp3RL)
            {
                ModalInsereValores(txtComp3RL);
            }
        }

        /// <summary>
        /// Verifica se o mês atual é multiplo de 12
        /// O 12º mês representa um reajuste no valor do Aluguel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private void VerificaMesReajuste(DateTime dataCadastroRecibo)
        {
            DateTime dataAnoReajuste = dataCadastroRecibo.AddYears(1);
            if (dataCadastroRecibo.Month == dataAnoReajuste.Month && dataCadastroRecibo.Year == dataAnoReajuste.Year)
            {
                pnlReajuste.Visible = true;
            }
        }

        private void dtpInicial_ValueChanged(object sender, EventArgs e)
        {
            if (dtpInicial.Value.Month != dtpFinal.Value.Month)
            {
                DateTime proximoMes = dtpInicial.Value.AddMonths(1);
                dtpFinal.Value = proximoMes;
            }
        }

        private void txtCompPagto_TextChanged(object sender, EventArgs e)
        {
            if (!rbAcrescimoRP.Checked && !rbDescontoRP.Checked)
            {
                rbAcrescimoRP.Checked = true;
            }
        }

        private void fechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Ações Recibo Principal

        private void CarregaDadosReciboPrincipal()
        {
            dtpInicial.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFinal.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            try
            {
                nudPctMulta.Value = Convert.ToDecimal(ConfigurationSettings.AppSettings["PorcentagemMulta"]);

                DataTable dadosLocatario = CarregaDadosTabela(4, lblIdLocatario.Text.Trim());
                if (dadosLocatario.Rows.Count > 0)//Já possui Recibos Cadastrados
                {
                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Data"].ToString()))
                    {
                        VerificaMesReajuste(Convert.ToDateTime(dadosLocatario.DefaultView[0]["Data"].ToString()));
                    }

                    lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                    lblLocatario.Visible = true;

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Locatario2"].ToString().Trim()))
                    {
                        lblLocatario.Text += " / " + dadosLocatario.DefaultView[0]["Locatario2"].ToString().Trim();
                    }

                    nudQtdRecibos.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["Quantidade"].ToString());
                    nudParcelasIptu.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString());

                    if (nudQtdRecibos.Value != 0)
                    {
                        nudQtdRecibos.Enabled = false;
                        if (nudQtdRecibos.Value == Convert.ToInt32(dadosLocatario.DefaultView[0]["Numero"].ToString()))
                        {
                            nudNumRecibo.Value = 0;
                        }
                        else
                        {
                            nudNumRecibo.Value = RetornaNumeroContagemAtual(Convert.ToInt32(dadosLocatario.DefaultView[0]["Numero"].ToString()));
                        }

                    }

                    if (dadosLocatario.DefaultView[0]["Iptu"].ToString() != "0")
                    {
                        txtIptuRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Iptu"].ToString());
                    }

                    if (nudParcelasIptu.Value != 0)
                    {
                        nudParcelasIptu.Enabled = false;
                        if (nudParcelasIptu.Value == Convert.ToInt32(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString()))
                        {
                            nudNumeroParcIptu.Value = 0;
                            txtIptuRcPrincipal.Text = string.Empty;
                        }
                        else
                        {
                            nudNumeroParcIptu.Value = RetornaNumeroContagemAtual(Convert.ToInt32(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString()));
                        }
                    }

                    //txtLuz.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Luz"].ToString());
                    //txtAgua.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Agua"].ToString());
                    //txtMultaRP.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Multa"].ToString());

                    txtAluguelRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());
                }
                else//Cadastro do primeiro recibo
                {
                    dadosLocatario = CarregaDadosTabela(3, lblIdLocatario.Text.Trim());
                    if (dadosLocatario.Rows.Count > 0)
                    {
                        lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                        lblLocatario.Visible = true;

                        if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Locatario2"].ToString().Trim()))
                        {
                            lblLocatario.Text += " / " + dadosLocatario.DefaultView[0]["Locatario2"].ToString().Trim();
                        }

                        txtAluguelRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());                
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
            }

        }

        private void rbComplementoPagto_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAcrescimoRP.Checked)
            {
                txtCompPagtoRP.BackColor = Color.PaleGreen;
            }
            else if (rbDescontoRP.Checked)
            {
                txtCompPagtoRP.BackColor = Color.PaleGreen;
            }
            else
            {
                txtCompPagtoRP.BackColor = Color.White;
            }

            ckbDesmarcaCompRP.Enabled = true;
            ckbDesmarcaCompRP.Checked = false;
        }

        private void ckbEdicaoNumeros_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == ckbEditNumRecibo)
            {
                if (ckbEditNumRecibo.Checked)
                {
                    nudQtdRecibos.Enabled = true;
                    nudNumRecibo.Enabled = true;
                    nudNumRecibo.BackColor = Color.PaleGreen;
                }
                else
                {
                    nudQtdRecibos.Enabled = false;
                    nudNumRecibo.Enabled = false;
                    nudNumRecibo.BackColor = Color.White;
                }
            }
            else if (sender == ckbEdicaoIptu)
            {
                if (ckbEdicaoIptu.Checked)
                {
                    nudParcelasIptu.Enabled = true;
                    nudNumeroParcIptu.Enabled = true;
                    nudNumeroParcIptu.BackColor = Color.PaleGreen;
                }
                else
                {
                    nudParcelasIptu.Enabled = false;
                    nudNumeroParcIptu.Enabled = false;
                    nudNumeroParcIptu.BackColor = Color.White;
                }
            }
        }

        private void txtAluguelRcPrincipal_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAluguelRcPrincipal.Text.Trim()))
            {
                ckbComMulta.Enabled = true;
            }
            else
            {
                ckbComMulta.Enabled = false;
            }
        }

        private void ckbComMulta_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAluguelRcPrincipal.Text))
            {
                if (ckbComMulta.Checked)
                {
                    txtMultaRP.Text = Auxiliar.FormataValoresExibicao(calculaPorcentagem(nudPctMulta.Value, txtAluguelRcPrincipal.Text.Trim()));
                }
                else
                {
                    txtMultaRP.Text = string.Empty;
                }
            }
        }

        private void ckbEditaValorExtenso_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEditaValorExtenso.Checked)
            {
                txtExtensoValorTotal.Enabled = true;
                txtExtensoValorTotal.BackColor = Color.PaleGreen;
            }
            else
            {
                txtExtensoValorTotal.Enabled = false;
                txtExtensoValorTotal.BackColor = Color.White;
            }
        }

        private void btConcluirRcPrincipal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdLocatario.Text.Trim()))
            {
                if (Auxiliar.validaTextBox(txtAluguelRcPrincipal, errorProvider1))
                {
                    if (Auxiliar.validaTextBox(txtExtensoValorTotal, errorProvider1))
                    {
                        if (nudQtdRecibos.Value == 0)
                        {
                            if (DialogResult.Yes == MessageBox.Show("O campo Quatidade de Recibos está zerado. Deseja continuar mesmo assim?", "Confirmação",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                            {
                                try
                                {
                                    InsereDados(1);
                                    Auxiliar.MostraMensagemAlerta("Recibo armazenado com Sucesso!", 1);
                                    btConcluir.Enabled = false;
                                    btFechaRP.Visible = true;

                                    HabilitaPreviewImpressao(1);

                                    tcRecibos.TabPages.Add(tpLocador);
                                    tcRecibos.SelectedTab = tpLocador;
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show(ex.ToString());                                        
                                    Auxiliar.MostraMensagemAlerta("Falha ao salvar os dados do Recibo", 3);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                InsereDados(1);
                                Auxiliar.MostraMensagemAlerta("Recibo armazenado com Sucesso!", 1);
                                btConcluir.Enabled = false;
                                btFechaRP.Visible = true;

                                HabilitaPreviewImpressao(1);

                                tcRecibos.TabPages.Add(tpLocador);
                                tcRecibos.SelectedTab = tpLocador;
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());                                    
                                Auxiliar.MostraMensagemAlerta("Falha ao salvar os dados do Recibo", 3);
                            }
                        }
                    }
                }
            }
        }

        private void btCalcularRcPrincipal_Click(object sender, EventArgs e)
        {
            CalcularValorTotalRcPrincipal();
        }

        private void CalcularValorTotalRcPrincipal()
        {
            decimal valorTotal = 0;
            if (!string.IsNullOrEmpty(txtIptuRcPrincipal.Text.Trim()))
            {
                valorTotal = Auxiliar.FormataValorParaUso(txtIptuRcPrincipal);
            }

            if (!string.IsNullOrEmpty(txtDespCondRcPrincipal.Text.Trim()))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtDespCondRcPrincipal);
            }

            if (!string.IsNullOrEmpty(txtLuz.Text.Trim()))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtLuz);
            }

            if (!string.IsNullOrEmpty(txtAgua.Text.Trim()))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtAgua);
            }

            if (!string.IsNullOrEmpty(txtAluguelRcPrincipal.Text.Trim()))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtAluguelRcPrincipal);
            }

            if (!string.IsNullOrEmpty(txtCompPagtoRP.Text.Trim()))
            {
                valorTotal = RetornaAcrescimoOuDesconto(rbAcrescimoRP, rbDescontoRP, txtCompPagtoRP, valorTotal);
            }

            if (!string.IsNullOrEmpty(txtMultaRP.Text.Trim()))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtMultaRP);
            }

            txtExtensoValorTotal.Text = Auxiliar.valorPorExtenso(valorTotal);
            txtTotalRcPrincipal.Text = Auxiliar.FormataValoresExibicao(valorTotal.ToString());
        }

        private void btTranscreveValor_Click(object sender, EventArgs e)
        {
            try
            {
                if (Auxiliar.validaTextBox(txtTotalRcPrincipal, errorProvider1))
                {
                    decimal valorTotal = Auxiliar.FormataValorParaUso(txtTotalRcPrincipal);
                    txtExtensoValorTotal.Text = Auxiliar.valorPorExtenso(valorTotal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btPreviewRP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdReciboPrincipal.Text.Trim()) && lblIdReciboPrincipal.Text.Trim() != "0")
            {
                this.Hide();
                Auxiliar.PreviewReciboImpressao(1, Convert.ToInt32(lblIdReciboPrincipal.Text.Trim()));
            }
        }

        #endregion

        #region Ações Recibo Locador

        private void CarregaDadosReciboLocador()
        {
            try
            {
                nudPctComissao.Value = Convert.ToDecimal(ConfigurationSettings.AppSettings["PorcentagemComissao"]);

                DataTable dadosLocatario = CarregaDadosTabela(4, lblIdLocatario.Text.Trim());
                if (dadosLocatario.Rows.Count > 0)
                {
                    lblIdRecibo.Text = dadosLocatario.DefaultView[0]["IdRecibo"].ToString();

                    CarregaLocadores(dadosLocatario);

                    txtMultaRL.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Multa"].ToString());
                    txtAluguelRcLocador.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Usuário.", 3);
                //MessageBox.Show("Falha ao carregador os dados do Usuário.");
            }
        }

        private void CarregaLocadores(DataTable dadosLocatario)
        {
            string idLocadores = string.Empty;

            if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["fkIdLocador1"].ToString()) && dadosLocatario.DefaultView[0]["fkIdLocador1"].ToString() != "0")
            {
                idLocadores = dadosLocatario.DefaultView[0]["fkIdLocador1"].ToString().Trim();
            }

            if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["fkIdLocador2"].ToString()) && dadosLocatario.DefaultView[0]["fkIdLocador2"].ToString() != "0")
            {
                idLocadores += "," + dadosLocatario.DefaultView[0]["fkIdLocador2"].ToString().Trim();
            }

            if (!string.IsNullOrEmpty(idLocadores))
            {
                DataTable dtLocadores = CarregaDadosTabela(5, idLocadores);

                string locadores = string.Empty;

                for (int i = 0; i < dtLocadores.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        locadores += " / " + dtLocadores.DefaultView[i]["Locador"].ToString().Trim();
                    }
                    else
                    {
                        locadores += dtLocadores.DefaultView[i]["Locador"].ToString().Trim();
                    }
                }

                lblLocadores.Text = locadores;
                lblLocadores.Visible = true;
            }

        }

        private void btCalcularTotalLocador_Click(object sender, EventArgs e)
        {
            decimal valorTotal = 0;
            if (Auxiliar.validaTextBox(txtAluguelRcLocador, errorProvider1))
            {
                if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text))
                {
                    valorTotal = Auxiliar.FormataValorParaUso(txtAluguelRcLocador);
                }
            }

            if (!string.IsNullOrEmpty(txtMultaRL.Text))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtMultaRL);
            }

            if (Auxiliar.validaTextBox(txtComissao, errorProvider1))
            {
                if (!string.IsNullOrEmpty(txtComissao.Text.Trim()))
                {
                    valorTotal = (valorTotal - Auxiliar.FormataValorParaUso(txtComissao));
                }
            }

            if (!string.IsNullOrEmpty(txtComp1RL.Text))
            {
                valorTotal = valorTotal - Auxiliar.FormataValorParaUso(txtComp1RL);
            }

            txtTotalRcLocador.Text = Auxiliar.FormataValoresExibicao(valorTotal.ToString());
        }

        private void btSalvarRcLocador_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdRecibo.Text.Trim()))
            {
                if (Auxiliar.validaTextBox(txtAluguelRcLocador, errorProvider1))
                {
                    if (Auxiliar.validaTextBox(txtComissao, errorProvider1))
                    {
                        if (Auxiliar.validaTextBox(txtTotalRcLocador, errorProvider1))
                        {
                            try
                            {
                                InsereDados(2);
                                Auxiliar.MostraMensagemAlerta("Recibo armazenado com Sucesso!", 1);
                                btFecharRL.Visible = true;
                                btSalvarRecbLc.Enabled = false;

                                HabilitaPreviewImpressao(2);
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                                Auxiliar.MostraMensagemAlerta("Falha ao salvar os dados do Recibo", 3);
                            }
                        }
                    }
                }
            }
        }

        private void txtAluguelRcLocador_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text))
            {
                CalculaExibeComissaoSubTotal1();
            }
        }

        private void CalculaExibeComissaoSubTotal1()
        {
            if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && !string.IsNullOrEmpty(txtMultaRL.Text.Trim()))
            {
                decimal subTotal1 = Auxiliar.FormataValorParaUso(txtAluguelRcLocador) + Auxiliar.FormataValorParaUso(txtMultaRL);
                txtSubTotal1.Text = subTotal1.ToString();
                txtComissao.Text = calculaPorcentagem(nudPctComissao.Value, subTotal1.ToString());
            }
            else if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && string.IsNullOrEmpty(txtMultaRL.Text.Trim()))
            {
                txtSubTotal1.Text = txtAluguelRcLocador.Text;
                txtComissao.Text = calculaPorcentagem(nudPctComissao.Value, Auxiliar.FormataValorParaUso(txtAluguelRcLocador).ToString());
            }

            txtSubTotal1.Text = Auxiliar.FormataValoresExibicao(txtSubTotal1.Text.Trim());
        }

        private void btCalculaSubTotais_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (sender == btSubTotal1)
            {
                if (Auxiliar.validaTextBox(txtAluguelRcLocador, errorProvider1))
                {
                    decimal valor1 = Auxiliar.FormataValorParaUso(txtAluguelRcLocador);
                    decimal valor2 = 0;

                    if (!string.IsNullOrEmpty(txtMultaRL.Text.Trim()))
                    {
                        valor2 = Auxiliar.FormataValorParaUso(txtMultaRL);
                    }

                    txtSubTotal1.Text = Auxiliar.FormataValoresExibicao((valor1 + valor2).ToString());
                }

                CalculaExibeComissaoSubTotal1();
            }
        }

        private void txtComissao_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubTotal1.Text.Trim()))
            {
                decimal valor1 = Auxiliar.FormataValorParaUso(txtSubTotal1);
                decimal valor2 = 0;

                if (!string.IsNullOrEmpty(txtComissao.Text.Trim()))
                {
                    valor2 = Auxiliar.FormataValorParaUso(txtComissao);
                }

                txtSubTotal2.Text = Auxiliar.FormataValoresExibicao((valor1 - valor2).ToString());
                txtTotalRcLocador.Text = txtSubTotal2.Text.Trim();
            }
        }

        private void somaSubTotal3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubTotal2.Text))
            {
                decimal subTotal2 = Auxiliar.FormataValorParaUso(txtSubTotal2);
                CalculaSubTotal3(subTotal2);
            }
        }

        private void CalculaSubTotal3(decimal subTotal2)
        {
            decimal dComplemento = subTotal2;

            if (!string.IsNullOrEmpty(txtComp1RL.Text.Trim()))
            {
                dComplemento = RetornaAcrescimoOuDesconto(ckbDescontoComp1RL, txtComp1RL, dComplemento);
            }

            if (!string.IsNullOrEmpty(txtComp2RL.Text.Trim()))
            {
                dComplemento = RetornaAcrescimoOuDesconto(ckbDescontoComp2RL, txtComp2RL, dComplemento);
            }

            if (!string.IsNullOrEmpty(txtComp3RL.Text.Trim()))
            {
                dComplemento = RetornaAcrescimoOuDesconto(ckbDescontoComp3RL, txtComp3RL, dComplemento);
            }           
        }

        private void btPreviewRL_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdReciboLocador.Text.Trim()) && lblIdReciboLocador.Text.Trim() != "0")
            {
                this.Hide();
                Auxiliar.PreviewReciboImpressao(2, Convert.ToInt32(lblIdReciboLocador.Text.Trim()));
            }
        }

        private void ckbDescontoComp_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == ckbDescontoComp1RL)
            {
                FormataCampoComplemento(ckbDescontoComp1RL, txtDescComp1RL);
            }
            else if (sender == ckbDescontoComp2RL)
            {
                FormataCampoComplemento(ckbDescontoComp2RL, txtDescComp2RL);
            }
            else if (sender == ckbDescontoComp3RL)
            {
                FormataCampoComplemento(ckbDescontoComp3RL, txtDescComp3RL);
            }
        }

        private void FormataCampoComplemento(CheckBox ckbDescontoCompRL, TextBox txtDescCompRL)
        {
            if (ckbDescontoCompRL.Checked)
            {
                if (!string.IsNullOrEmpty(txtDescCompRL.Text.Trim()))
                {
                    txtDescCompRL.Text = "Desconto; " + txtDescCompRL.Text;
                }
                else
                {
                    txtDescCompRL.Text = "Desconto; ";                    
                }
            }
            else
            {
                txtDescCompRL.Text = txtDescCompRL.Text.Replace("Desconto;", string.Empty).Trim();
            }
        }

        #endregion      

    }
}
