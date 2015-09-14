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
                MessageBox.Show(ex.ToString());
            }

        }

        #region Ações Banco de Dados

        /// <summary>
        /// tipoTabela 1 = Recibo Principal
        /// tipoTabela 2 = Recibo Locador
        /// tipoTabela 3 = Locatarios (Quando ainda não existem Recibos Cadastrados)
        /// tipoTabela 4 = Recibo Locatário + Locatarios
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
                                  " Rp.Data, Rp.Id as IdRecibo, Lcd.Locador" +
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
                dr["fkIdLocatario"] = lblIdLocatario.Text.Trim();
                dr["Quantidade"] = nudQtdRecibos.Value.ToString().Trim();
                dr["Numero"] = nudNumRecibo.Value.ToString();
                dr["Data"] = DateTime.Now;
                dr["Periodo"] = Convert.ToDateTime(dtpInicial.Value).ToShortDateString() + " a " + Convert.ToDateTime(dtpFinal.Value).ToShortDateString();
                dr["Iptu"] = RetornaValorCamposOpcionais(mtxtIptu, 2);
                dr["ParcelasIptu"] = nudParcelasIptu.Value.ToString();
                dr["DespesaCondominio"] = RetornaValorCamposOpcionais(mtxtDespCondominio, 2);
                dr["Luz"] = Auxiliar.FormataValorSalvar(mtxtLuz, 2);
                dr["Agua"] = Auxiliar.FormataValorSalvar(mtxtAgua, 2);
                dr["Aluguel"] = Auxiliar.FormataValorSalvar(mtxtAluguel, 1);
                dr["ComplementoPagamento"] = RetornaValorCamposOpcionais(mtxtCompPagamento, 2);
                dr["DescricaoComplementoPagamento"] = txtDescricaoCompPagto.Text.Trim();
                dr["FormaPagamento"] = txtFormaPagto.Text.Trim();
                dr["DataPagamento"] = Convert.ToDateTime(dtpVencimento.Value).ToShortDateString();
                dr["TotalPagamento"] = Auxiliar.FormataValorSalvar(mtxtValorTotal, 2);
                dr["ExtensoTotalPagamento"] = txtExtensoValorTotal.Text.Trim();

            }
            else if (tipoInsercao == 2)
            {
                dr["Aluguel"] = Auxiliar.FormataValorSalvar(mtxtAluguelRecbLc, 1);
                dr["Multa"] = RetornaValorCamposOpcionais(mtxtMultaRecbLc, 2);
                dr["PorcentagemMulta"] = nudPorcentMulta.Value;
                dr["Comissao"] = Auxiliar.FormataValorSalvar(mtxtComissaoRecbLc, 1);
                dr["DespesaCondominio"] = RetornaValorCamposOpcionais(mtxtDespCondRecbLc, 2); ;
                dr["Total"] = Auxiliar.FormataValorSalvar(mtxtTotalRecbLc, 1);
                dr["fkIdRecibo"] = Convert.ToInt32(lblIdRecibo.Text.Trim());
            }

            dsInclusao.Tables[0].Rows.Add(dr);
            sdaInclusao.Update(dsInclusao);
        }

        /// <summary>
        /// tipoValor 1 = Campo Aluguel, Campo Valor Total
        /// tipoValor 2 = Outros Campos
        /// </summary>
        /// <param name="maskedTxt"></param>
        /// <param name="tipoValor"></param>
        /// <returns></returns>
        private object RetornaValorCamposOpcionais(MaskedTextBox maskedTxt, int tipoValor)
        {
            if (string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtIptu)))
            {
                return 0;
            }
            else
            {
                return Auxiliar.FormataValorSalvar(maskedTxt, tipoValor);
            }
        }

        private decimal RetornaNumeroReciboAtual(int numeroReciboAnterior)
        {
            return numeroReciboAnterior + 1;
        }

        private string RetornaDataAtual()
        {
            string dataAtual = DateTime.Now.ToShortDateString();

            return dataAtual;
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
                if (dadosLocatario.Rows.Count > 0)
                {
                    lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();

                    nudQtdRecibos.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["Quantidade"].ToString());
                    nudQtdRecibos.Enabled = false;

                    nudNumRecibo.Value = RetornaNumeroReciboAtual(Convert.ToInt32(dadosLocatario.DefaultView[0]["Numero"].ToString()));

                    if (dadosLocatario.DefaultView[0]["Iptu"].ToString() != "0")
                    {
                        mtxtIptu.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Iptu"].ToString(), 2);
                    }

                    if (dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() != "0")
                    {
                        nudParcelasIptu.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString()) - 1;
                    }

                    mtxtLuz.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Luz"].ToString(), 2);
                    mtxtAgua.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Agua"].ToString(), 2);

                    mtxtAluguel.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), 1);
                }
                else
                {
                    dadosLocatario = CarregaDadosTabela(3);
                    if (dadosLocatario.Rows.Count > 0)
                    {
                        lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                        mtxtAluguel.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show("Falha ao carregador os dados do Usuário.");
            }

        }

        private void rbComplementoPagto_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAcrescimo.Checked)
            {
                mtxtCompPagamento.Enabled = true;
                mtxtCompPagamento.BackColor = Color.PaleGreen;
            }
            else if (rbDesconto.Checked)
            {
                mtxtCompPagamento.Enabled = true;
                mtxtCompPagamento.BackColor = Color.PaleGreen;
            }

            mtxtCompPagamento.Text = string.Empty;
        }

        private void ckbEditNumRecibo_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEditNumRecibo.Checked)
            {
                nudNumRecibo.Enabled = true;
                nudNumRecibo.BackColor = Color.PaleGreen;
            }
            else
            {
                nudNumRecibo.Enabled = false;
                nudNumRecibo.BackColor = Color.White;
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

        private void btConcluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdLocatario.Text.Trim()))
            {
                if (Auxiliar.validaCampoMtxt(mtxtLuz, errorProvider1))
                {
                    if (Auxiliar.validaCampoMtxt(mtxtAgua, errorProvider1))
                    {
                        if (Auxiliar.validaCampoMtxt(mtxtAluguel, errorProvider1))
                        {
                            if (Auxiliar.validaCampoTxt(txtFormaPagto, errorProvider1))
                            {
                                if (Auxiliar.validaCampoMtxt(mtxtValorTotal, errorProvider1))
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
                                                    MessageBox.Show("Recibo armazenado com Sucesso!");
                                                    btMostraReciboLc.Visible = true;
                                                    btConcluir.Enabled = false;
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                InsereDados(1);
                                                MessageBox.Show("Recibo armazenado com Sucesso!");
                                                btMostraReciboLc.Visible = true;
                                                btConcluir.Enabled = false;
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

        private void btCalcular_Click(object sender, EventArgs e)
        {
            CalcularValorTotal();
        }

        private void CalcularValorTotal()
        {
            decimal valorTotal = 0;
            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtIptu)))
            {
                valorTotal = Auxiliar.FormataValorSalvar(mtxtIptu, 2);
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtDespCondominio)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtDespCondominio, 2);
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtLuz)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtLuz, 2);
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtAgua)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtAgua, 2);
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtAluguel)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtAluguel, 1);
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtCompPagamento)))
            {
                if (rbAcrescimo.Checked)
                {
                    if (Auxiliar.validaCampoMtxt(mtxtCompPagamento, errorProvider1))
                    {
                        valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtCompPagamento, 1);
                    }
                }
                else if (rbDesconto.Checked)
                {
                    if (Auxiliar.validaCampoMtxt(mtxtCompPagamento, errorProvider1))
                    {
                        valorTotal = valorTotal - Auxiliar.FormataValorSalvar(mtxtCompPagamento, 1);
                    }
                }
            }

            txtExtensoValorTotal.Text = Auxiliar.valorPorExtenso(valorTotal);
            mtxtValorTotal.Text = Auxiliar.FormataValorArmazenado(valorTotal.ToString(), 0);
        }

        private void btTranscreveValor_Click(object sender, EventArgs e)
        {
            try
            {
                if (Auxiliar.validaCampoMtxt(mtxtValorTotal, errorProvider1))
                {
                    decimal valorTotal = Auxiliar.FormataValorSalvar(mtxtValorTotal, 1);
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
            Inicio formInicio = new Inicio();
            this.Hide();
            formInicio.ShowDialog();
            this.Close();
        }

        private void btReciboLc_Click(object sender, EventArgs e)
        {
            tcRecibos.TabPages.Add(tpLocador);

            tcRecibos.SelectedTab = tpLocador;
        }

        private void tcRecibos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcRecibos.SelectedTab == tpPrincipal)
            {
                tcRecibos.TabPages.Remove(tpLocador);
                btMostraReciboLc.Visible = false;
            }
            else
            {
                CarregaDadosReciboLocador();
            }
        }

        private void CarregaDadosReciboLocador()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(4);
                if (dadosLocatario.Rows.Count > 0)
                {
                    lblIdRecibo.Text = dadosLocatario.DefaultView[0]["IdRecibo"].ToString();
                    lblNomeLocador.Text = dadosLocatario.DefaultView[0]["Locador"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString()) && 
                        dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString() != "0")
                    {
                        mtxtDespCondRecbLc.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString(), 2);
                    }
                    mtxtAluguelRecbLc.Text = Auxiliar.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show("Falha ao carregador os dados do Usuário.");
            }
        }

        private void btCalcularTotalLc_Click(object sender, EventArgs e)
        {
            decimal valorTotal = 0;
            if (Auxiliar.validaCampoMtxt(mtxtAluguelRecbLc, errorProvider1))
            {
                if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtAluguelRecbLc)))
                {
                    valorTotal = Auxiliar.FormataValorSalvar(mtxtAluguelRecbLc, 1);
                }
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtMultaRecbLc)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtMultaRecbLc, 2);
            }

            if (Auxiliar.validaCampoMtxt(mtxtComissaoRecbLc, errorProvider1))
            {

                if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtComissaoRecbLc)))
                {
                    valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtComissaoRecbLc, 1);
                }
            }

            if (!string.IsNullOrEmpty(Auxiliar.VerificaMtxtVazio(mtxtDespCondRecbLc)))
            {
                valorTotal = valorTotal + Auxiliar.FormataValorSalvar(mtxtDespCondRecbLc, 2);
            }

            mtxtTotalRecbLc.Text = Auxiliar.FormataValorArmazenado(valorTotal.ToString(), 0);
        }

        private void btSalvarRecbLc_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdRecibo.Text.Trim()))
            {
                if (Auxiliar.validaCampoMtxt(mtxtLuz, errorProvider1))
                {
                    if (Auxiliar.validaCampoMtxt(mtxtAgua, errorProvider1))
                    {
                        if (Auxiliar.validaCampoMtxt(mtxtAluguel, errorProvider1))
                        {
                            try
                            {
                                InsereDados(2);
                                MessageBox.Show("Recibo armazenado com Sucesso!");
                                btMostraReciboLc.Visible = true;
                                btSalvarRecbLc.Enabled = false;
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
