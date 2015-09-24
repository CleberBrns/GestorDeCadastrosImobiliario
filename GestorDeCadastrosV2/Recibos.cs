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

            Auxiliar.CentralizaControle(tcRecibos, this);
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
                MessageBox.Show(ex.ToString());
            }
        }

        #region Ações Banco de Dados

        /// <summary>
        /// tipoTabela 1 = Recibo Principal
        /// tipoTabela 2 = Recibo Locador
        /// tipoTabela 3 = Locatarios (Quando ainda não existem Recibos Cadastrados)
        /// tipoTabela 4 = Recibo Principal + Locadores
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        /// <param name="idLocatario"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela, int idLocatario)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select * from RecibosPrincipais";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select * from RecibosLocadores";
            }
            else if (tipoTabela == 3)
            {
                cmd.CommandText = "select Lc.Locatario, Lc.Aluguel from Locatarios Lc where Ativo = 1 and Id = " + idLocatario + "";
            }
            else
            {
                cmd.CommandText = "select top(1) Lct.Locatario, Lct.Aluguel, Rp.Quantidade, Rp.Numero, Rp.Iptu, Rp.ParcelasIptu, Rp.DespesaCondominio, Rp.Luz, Rp.Agua," +
                                  " Rp.Data, Rp.Id as IdRecibo, Rp.NumeroParcelaIptu, Lcd.Locador" +
                                  " from Locatarios Lct" +
                                  " inner join RecibosPrincipais Rp" +
                                  " on Lct.Id = Rp.fkIdLocatario" +
                                  " inner join Locadores Lcd" +
                                  " on Lct.fkIdLocador = Lcd.Id" +
                                  " where Lct.Ativo = 1 and Lct.Id = " + idLocatario + " order by Rp.Data desc";

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
            PreencheDataset(out dsInclusao, out sdaInclusao, tipoInsercao, Convert.ToInt32(lblIdLocatario.Text.Trim()));
            //Cria nova Linha para a inserção.
            dr = dsInclusao.Tables[0].NewRow();

            if (tipoInsercao == 1)
            {
                DateTime dtTestes = Convert.ToDateTime("28/08/2015");
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
                dr["ComplementoPagamento"] = RetornaValorCamposOpcionais(txtCompPagto);
                dr["DescricaoComplementoPagamento"] = txtDescricaoCompPagto.Text.Trim();
                dr["FormaPagamento"] = txtFormaPagto.Text.Trim();
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
                dr["Multa"] = RetornaValorCamposOpcionais(txtMulta);
                dr["PorcentagemComissao"] = Convert.ToInt32(nudPctComissao.Value);
                dr["Comissao"] = Auxiliar.FormataValorParaUso(txtComissao);
                dr["Complemento"] = RetornaValorCamposOpcionais(txtComplementoRL);
                dr["DescricaoComplemento"] = txtDescricaoComplementoRL.Text.Trim();
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

        private DataTable CarregaDadosTabela(int tipoPesquisa)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoPesquisa, Convert.ToInt32(lblIdLocatario.Text.Trim()));

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        #endregion

        private void CarregaDadosReciboPrincipal()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(4);
                if (dadosLocatario.Rows.Count > 0)//Já possui Recibos Cadastrados
                {
                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Data"].ToString()))
                    {
                        VerificaMesReajuste(Convert.ToDateTime(dadosLocatario.DefaultView[0]["Data"].ToString()));
                    }

                    lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                    lblLocatario.Visible = true;

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


                                   

                    txtLuz.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Luz"].ToString());
                    txtAgua.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Agua"].ToString());

                    txtAluguelRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());
                }
                else//Cadastro do primeiro recibo
                {
                    dadosLocatario = CarregaDadosTabela(3);
                    if (dadosLocatario.Rows.Count > 0)
                    {
                        lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                        lblLocatario.Visible = true;
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
            if (rbAcrescimo.Checked)
            {
                txtCompPagto.BackColor = Color.PaleGreen;
            }
            else if (rbDesconto.Checked)
            {
                txtCompPagto.BackColor = Color.PaleGreen;
            }
            else
            {
                txtCompPagto.BackColor = Color.White;
            }

            ckbDesmarcaComPagto.Enabled = true;
            ckbDesmarcaComPagto.Checked = false;
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
                if (Auxiliar.validaCampoTxt(txtAluguelRcPrincipal, errorProvider1))
                {
                    if (Auxiliar.validaCampoTxt(txtAluguelRcPrincipal, errorProvider1))
                    {
                        if (Auxiliar.validaCampoTxt(txtExtensoValorTotal, errorProvider1))
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

            if (!string.IsNullOrEmpty(txtCompPagto.Text.Trim()))
            {
                if (rbAcrescimo.Checked)
                {
                    if (Auxiliar.validaCampoTxt(txtCompPagto, errorProvider1))
                    {
                        valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtCompPagto);
                    }
                }
                else if (rbDesconto.Checked)
                {
                    if (Auxiliar.validaCampoTxt(txtCompPagto, errorProvider1))
                    {
                        valorTotal = valorTotal - Auxiliar.FormataValorParaUso(txtCompPagto);
                    }
                }
            }

            txtExtensoValorTotal.Text = Auxiliar.valorPorExtenso(valorTotal);
            txtTotalRcPrincipal.Text = Auxiliar.FormataValoresExibicao(valorTotal.ToString());
        }

        private void btTranscreveValor_Click(object sender, EventArgs e)
        {
            try
            {
                if (Auxiliar.validaCampoTxt(txtTotalRcPrincipal, errorProvider1))
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

        private void btInicio_Click(object sender, EventArgs e)
        {
            Login formInicio = new Login();
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

        private void CarregaDadosReciboLocador()
        {
            try
            {
                nudPctMulta.Value = Convert.ToDecimal(ConfigurationSettings.AppSettings["PorcentagemMulta"]);
                nudPctComissao.Value = Convert.ToDecimal(ConfigurationSettings.AppSettings["PorcentagemComissao"]);

                DataTable dadosLocatario = CarregaDadosTabela(4);
                if (dadosLocatario.Rows.Count > 0)
                {
                    lblIdRecibo.Text = dadosLocatario.DefaultView[0]["IdRecibo"].ToString();
                    lblLocador.Text = dadosLocatario.DefaultView[0]["Locador"].ToString();
                    lblLocador.Visible = true;

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

        private void btCalcularTotalLocador_Click(object sender, EventArgs e)
        {
            decimal valorTotal = 0;
            if (Auxiliar.validaCampoTxt(txtAluguelRcLocador, errorProvider1))
            {
                if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text))
                {
                    valorTotal = Auxiliar.FormataValorParaUso(txtAluguelRcLocador);
                }
            }

            if (!string.IsNullOrEmpty(txtMulta.Text))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorParaUso(txtMulta);
            }

            if (Auxiliar.validaCampoTxt(txtComissao, errorProvider1))
            {
                if (!string.IsNullOrEmpty(txtComissao.Text.Trim()))
                {
                    valorTotal = (valorTotal - Auxiliar.FormataValorParaUso(txtComissao));
                }
            }

            if (!string.IsNullOrEmpty(txtComplementoRL.Text))
            {
                valorTotal = valorTotal - Auxiliar.FormataValorParaUso(txtComplementoRL);
            }

            txtTotalRcLocador.Text = Auxiliar.FormataValoresExibicao(valorTotal.ToString());
        }

        private void btSalvarRcLocador_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdRecibo.Text.Trim()))
            {
                if (Auxiliar.validaCampoTxt(txtAluguelRcLocador, errorProvider1))
                {
                    if (Auxiliar.validaCampoTxt(txtComissao, errorProvider1))
                    {
                        if (Auxiliar.validaCampoTxt(txtTotalRcLocador, errorProvider1))
                        {
                            try
                            {
                                InsereDados(2);
                                Auxiliar.MostraMensagemAlerta("Recibo armazenado com Sucesso!", 1);
                                btSalvarRecbLc.Enabled = false;                              
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
                ModalInsereValores(txtCompPagto);
            }
            else if (sender == btTotalRcPrincipal)
            {
                ModalInsereValores(txtTotalRcPrincipal);
            }
            else if (sender == btAluguelRcLocador)
            {
                ModalInsereValores(txtAluguelRcLocador);
            }
            else if (sender == btMulta)
            {
                ModalInsereValores(txtMulta);
            }
            else if (sender == btComissao)
            {
                ModalInsereValores(txtComissao);
            }
            else if (sender == btComplementoRL)
            {
                ModalInsereValores(txtComplementoRL);
            }
        }

        private void ModalInsereValores(TextBox txtParaInsercao)
        {
            InsereValores dlgInsereValores = new InsereValores();
            if (dlgInsereValores.ShowDialog() == DialogResult.OK)
                txtParaInsercao.Text = dlgInsereValores.getValorInserido;
        }

        private void txtAluguelRcLocador_TextChanged(object sender, EventArgs e)
        {
            if (ckbComMulta.Checked)
            {
                if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text))
                {
                    txtMulta.Text = Auxiliar.FormataValoresExibicao(calculaPorcentagem(nudPctMulta.Value, txtAluguelRcLocador.Text.Trim()));

                    CalculaExibeComissaoSubTotal1();
                }
            }
            else
            {
                CalculaExibeComissaoSubTotal1();
            }

        }

        private void CalculaExibeComissaoSubTotal1()
        {
            if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && !string.IsNullOrEmpty(txtMulta.Text.Trim()))
            {
                decimal subTotal1 = Auxiliar.FormataValorParaUso(txtAluguelRcLocador) + Auxiliar.FormataValorParaUso(txtMulta);
                txtSubTotal1.Text = subTotal1.ToString();
                txtComissao.Text = calculaPorcentagem(nudPctComissao.Value, subTotal1.ToString());
            }
            else if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && string.IsNullOrEmpty(txtMulta.Text.Trim()))
            {
                txtSubTotal1.Text = txtAluguelRcLocador.Text;
                txtComissao.Text = calculaPorcentagem(nudPctComissao.Value, Auxiliar.FormataValorParaUso(txtAluguelRcLocador).ToString());
            }

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

        private void btCalculaSubTotais_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (sender == btSubTotal1)
            {
                if (Auxiliar.validaCampoTxt(txtAluguelRcLocador, errorProvider1))
                {
                    decimal valor1 = Auxiliar.FormataValorParaUso(txtAluguelRcLocador);
                    decimal valor2 = 0;

                    if (!string.IsNullOrEmpty(txtMulta.Text.Trim()))
                    {
                        valor2 = Auxiliar.FormataValorParaUso(txtMulta);
                    }

                    txtSubTotal1.Text = Auxiliar.FormataValoresExibicao((valor1 + valor2).ToString());
                }

                CalculaExibeComissaoSubTotal1();

            }
            else if (sender == btSubTotal3)
            {
                if (Auxiliar.validaCampoTxt(txtSubTotal2, errorProvider1))
                {
                    decimal subTotal2 = Auxiliar.FormataValorParaUso(txtSubTotal2);

                    CalculaSubTotal3(subTotal2);
                }
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
            decimal dComplemento = 0;

            if (!string.IsNullOrEmpty(txtComplementoRL.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(txtComplementoRL);
            }

            txtSubTotal3.Text = Auxiliar.FormataValoresExibicao((subTotal2 - dComplemento).ToString());
        }

        private void txtSubTotal3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubTotal3.Text.Trim()))
            {
                txtTotalRcLocador.Text = Auxiliar.FormataValoresExibicao(txtSubTotal3.Text.Trim());
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

        private void ckbDesmarcaComPagto_CheckedChanged(object sender, EventArgs e)
        {
            rbAcrescimo.Checked = false;
            rbDesconto.Checked = false;
            txtCompPagto.BackColor = Color.White;
            txtCompPagto.Text = string.Empty;
        }

        private void txtCompPagto_TextChanged(object sender, EventArgs e)
        {
            if (!rbAcrescimo.Checked && !rbDesconto.Checked)
            {
                rbAcrescimo.Checked = true;
            }
        }

    }
}
