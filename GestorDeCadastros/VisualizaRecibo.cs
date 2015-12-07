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
    public partial class VisualizaRecibo : Form
    {
        private bool ExecucaoParaTestes = false;

        private int idReciboPrincipal;
        private int idReciboLocador;

        public int getIdReciboPrincipal
        {
            get { return idReciboPrincipal; }
            set { idReciboPrincipal = value; }
        }

        public int getIdReciboLocador
        {
            get { return idReciboLocador; }
            set { idReciboLocador = value; }
        }


        public VisualizaRecibo()
        {
            InitializeComponent();
            //Auxiliar.CentralizaControle(tcRecibos, this);
        }

        private void Recibos_Load(object sender, EventArgs e)
        {
            try
            {
                ExecucaoParaTestes = Auxiliar.VerificaTipoExecucao();

                //idReciboPrincipal = 1;
                if (idReciboPrincipal != 0)
                {
                    lblIdReciboPrincipal.Text = idReciboPrincipal.ToString();
                    CarregaDadosReciboPrincipal();
                    tcRecibos.TabPages.Remove(tpLocador);
                }
                else if (idReciboLocador != 0)
                {
                    lblIdReciboLocador.Text = idReciboLocador.ToString();
                    CarregaDadosReciboLocador();
                    tcRecibos.TabPages.Remove(tpPrincipal);
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Não foi possivel carregar dados para essa pesquisa", 2);
                }

            }
            catch (Exception ex)
            {
                if (ExecucaoParaTestes)
                {
                    Auxiliar.MostraMensagemAlerta(ex.ToString(), 2);
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Não foi possivel resgatar os dados da pesquisa.", 2);
                }
            }
        }

        #region Ações Banco de Dados

        /// <summary>
        /// tipoTabela 1 = Recibo Principal
        /// tipoTabela 2 = Recibo Locador
        /// tipoTabela 3 = Locadores por Id
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        /// <param name="idLocatario"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela, string idBusca)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select lct.Locatario, lct.Locatario2, rp.Quantidade, rp.Numero, rp.Data, rp.Periodo, rp.Iptu, rp.ParcelasIptu, rp.NumeroParcelaIptu," +
                                  " rp.DespesaCondominio, rp.Luz, rp.Agua, rp.Aluguel, rp.ComplementoPagamento, rp.DescricaoComplementoPagamento, rp.Observacao," +
                                  " rp.DataPagamento, rp.TotalPagamento, rp.ExtensoTotalPagamento, rp.ReajusteAluguel, rp.Multa, rl.Id as IdRL" +
                                  " from RecibosPrincipais Rp" +
                                  " inner join Locatarios lct on rp.fkIdLocatario = lct.Id" +
                                  " inner join RecibosLocadores rl on rp.Id = rl.fkIdRecibo" +
                                  " where rp.Id = " + idBusca;
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select rl.Aluguel, rl.PorcentagemMulta, rl.Multa, rl.PorcentagemComissao, rl.Comissao, rl.Complemento, rl.DescricaoComplemento," +
                                  " rl.Complemento2, rl.DescricaoComplemento2, rl.Complemento3, rl.DescricaoComplemento3, rl.Total, rp.Id as IdRP," +
                                  " im.fkIdLocador1, im.fkIdLocador2" +
                                  " from RecibosLocadores rl" +
                                  " inner join RecibosPrincipais rp on rl.fkIdRecibo = rp.Id" +
                                  " inner join Locatarios lct on rp.fkIdLocatario = lct.Id" +
                                  " inner join Imoveis im on lct.fkIdImovel = im.Id" +
                                  " where rl.Id = " + idBusca + "";
            }
            else if (tipoTabela == 3)
            {
                cmd.CommandText = "select Locador, Id from Locadores where id in (" + idBusca + ") and Ativo = 1 order by Locador";

            }

            sdaDados.SelectCommand = cmd;
            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        private string RetornaDataAtual()
        {
            return DateTime.Now.ToShortDateString();
        }

        private DataTable CarregaDadosTabela(int tipoPesquisa, string idBusca)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoPesquisa, idBusca);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        #endregion

        #region Dados Recibo Principal

        private void CarregaDadosReciboPrincipal()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(1, lblIdReciboPrincipal.Text.Trim());
                if (dadosLocatario.Rows.Count > 0)
                {
                    lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                    lblLocatario.Visible = true;

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["IdRL"].ToString()))
                    {
                        lblIdReciboLocador.Text = dadosLocatario.DefaultView[0]["IdRL"].ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Quantidade"].ToString()) && dadosLocatario.DefaultView[0]["Quantidade"].ToString() != "0")
                        nudQtdRecibos.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["Quantidade"].ToString());

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Numero"].ToString()) && dadosLocatario.DefaultView[0]["Numero"].ToString() != "0")
                        nudNumRecibo.Value = Convert.ToInt32(dadosLocatario.DefaultView[0]["Numero"].ToString());

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Periodo"].ToString()))
                    {
                        try
                        {
                            string[] aPeriodo = dadosLocatario.DefaultView[0]["Periodo"].ToString().Split('a');
                            txtPeriodo.Text = "de " + aPeriodo[0].ToString() + " a " + aPeriodo[1].ToString();
                        }
                        catch (Exception) { }
                    }


                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Iptu"].ToString()) && dadosLocatario.DefaultView[0]["Iptu"].ToString() != "0")
                        txtIptuRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Iptu"].ToString());

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString()) && dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() != "0")
                        nudParcelasIptu.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString());

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString()) &&
                        dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString() != "0")
                    {
                        nudNumeroParcIptu.Value = Convert.ToInt32(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Aluguel"].ToString()) &&
                        dadosLocatario.DefaultView[0]["Aluguel"].ToString() != "0")
                    {
                        txtAluguelRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Luz"].ToString()) &&
                       dadosLocatario.DefaultView[0]["Luz"].ToString() != "0")
                    {
                        txtLuz.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Luz"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Agua"].ToString()) &&
                       dadosLocatario.DefaultView[0]["Agua"].ToString() != "0")
                    {
                        txtAgua.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Agua"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString()) &&
                       dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString() != "0")
                    {
                        txtDespCondRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["ComplementoPagamento"].ToString()) &&
                       dadosLocatario.DefaultView[0]["ComplementoPagamento"].ToString() != "0")
                    {
                        txtCompPagto.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["ComplementoPagamento"].ToString());
                    }

                    txtDescricaoCompPagto.Text = dadosLocatario.DefaultView[0]["DescricaoComplementoPagamento"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Multa"].ToString()) &&
                      dadosLocatario.DefaultView[0]["Multa"].ToString() != "0")
                    {
                        txtMultaRP.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Multa"].ToString());
                    }

                    txtObservacao.Text = dadosLocatario.DefaultView[0]["Observacao"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["DataPagamento"].ToString()))
                    {
                        try
                        {
                            txtVencimento.Text = Convert.ToDateTime(dadosLocatario.DefaultView[0]["DataPagamento"].ToString()).ToShortDateString();
                        }
                        catch (Exception) { }
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Data"].ToString()))
                    {
                        try
                        {

                            txtDataCadastro.Text = Convert.ToDateTime(dadosLocatario.DefaultView[0]["Data"].ToString()).ToShortDateString();
                        }
                        catch (Exception) { }
                    }

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["TotalPagamento"].ToString()) &&
                        dadosLocatario.DefaultView[0]["TotalPagamento"].ToString() != "0")
                    {
                        txtTotalRcPrincipal.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["TotalPagamento"].ToString());
                    }


                    txtExtensoValorTotal.Text = dadosLocatario.DefaultView[0]["ExtensoTotalPagamento"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["ReajusteAluguel"].ToString()))
                    {
                        pnlReajuste.Visible = true;
                        txtRejuste.Text = dadosLocatario.DefaultView[0]["ReajusteAluguel"].ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                if (ExecucaoParaTestes)
                {
                    Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
                }
            }
        }

        private void btPVImpressaoRP_Click(object sender, EventArgs e)
        {
            this.Hide();
            Auxiliar.PreviewReciboImpressao(1, Convert.ToInt32(lblIdReciboPrincipal.Text.Trim()));
            //this.Close();
        }

        private void btRL_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdReciboLocador.Text.Trim()))
            {
                CarregaDadosReciboLocador();
                if (!tcRecibos.TabPages.Contains(tpLocador))
                {
                    tcRecibos.TabPages.Add(tpLocador);
                }

                tcRecibos.SelectedTab = tpLocador;
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Não foi possivel carregar o Recibo do Locador pois o mesmo não foi salvo no processo inicial.", 2);
            }
        }

        #endregion

        #region Dados Recibo Locador

        private void CarregaDadosReciboLocador()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(2, lblIdReciboLocador.Text.Trim());
                if (dadosLocatario.Rows.Count > 0)
                {
                    CarregaLocadores(dadosLocatario);

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["IdRP"].ToString().Trim()))
                    {
                        lblIdReciboPrincipal.Text = dadosLocatario.DefaultView[0]["IdRP"].ToString().Trim();
                    }

                    txtAluguelRcLocador.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Aluguel"].ToString());

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Multa"].ToString()) && dadosLocatario.DefaultView[0]["Multa"].ToString() != "0")
                    {
                        txtMultaRL.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Multa"].ToString());
                    }

                    CalculaExibeSubTotal1();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString()))
                    {
                        try
                        {
                            nudPctComissao.Value = Convert.ToDecimal(dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString().Trim());
                        }
                        catch (Exception)
                        {
                            nudPctComissao.Value = Convert.ToDecimal(ConfigurationSettings.AppSettings["PorcentagemComissao"]);
                        }
                    }

                    txtComissao.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Comissao"].ToString());

                    CalculaExibeSubTotal2();

                    txtDescComp1RL.Text = dadosLocatario.DefaultView[0]["DescricaoComplemento"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Complemento"].ToString()) && dadosLocatario.DefaultView[0]["Complemento"].ToString() != "0")
                    {
                        txtComp1RL.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Complemento"].ToString());
                    }

                    txtDescComp2RL.Text = dadosLocatario.DefaultView[0]["DescricaoComplemento2"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Complemento2"].ToString()) && dadosLocatario.DefaultView[0]["Complemento2"].ToString() != "0")
                    {
                        txtComp2RL.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Complemento2"].ToString());
                    }

                    txtDescComp3RL.Text = dadosLocatario.DefaultView[0]["DescricaoComplemento3"].ToString();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Complemento3"].ToString()) && dadosLocatario.DefaultView[0]["Complemento3"].ToString() != "0")
                    {
                        txtComp3RL.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Complemento3"].ToString());
                    }

                    CalculaSubTotal3(Auxiliar.FormataValorParaUso(txtSubTotal2));

                    txtTotalRcLocador.Text = Auxiliar.FormataValoresExibicao(dadosLocatario.DefaultView[0]["Total"].ToString());

                }
            }
            catch (Exception ex)
            {
                if (ExecucaoParaTestes)
                {
                    Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
                }
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
                DataTable dtLocadores = CarregaDadosTabela(3, idLocadores);

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

        private void CalculaExibeSubTotal1()
        {
            if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && !string.IsNullOrEmpty(txtMultaRL.Text.Trim()))
            {
                decimal subTotal1 = Auxiliar.FormataValorParaUso(txtAluguelRcLocador) + Auxiliar.FormataValorParaUso(txtMultaRL);
                txtSubTotal1.Text = subTotal1.ToString();
            }
            else if (!string.IsNullOrEmpty(txtAluguelRcLocador.Text.Trim()) && string.IsNullOrEmpty(txtMultaRL.Text.Trim()))
            {
                txtSubTotal1.Text = txtAluguelRcLocador.Text;
            }
        }

        private void CalculaExibeSubTotal2()
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
            }
        }

        private void CalculaSubTotal3(decimal subTotal2)
        {
            decimal dComplemento = 0;

            if (!string.IsNullOrEmpty(txtComp1RL.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(txtComp1RL);
            }

            if (!string.IsNullOrEmpty(txtComp2RL.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(txtComp2RL) + dComplemento;
            }

            if (!string.IsNullOrEmpty(txtComp3RL.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(txtComp3RL) + dComplemento;
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

        private void btPVImpressaoRL_Click(object sender, EventArgs e)
        {
            this.Hide();
            Auxiliar.PreviewReciboImpressao(2, Convert.ToInt32(lblIdReciboLocador.Text.Trim()));
            //this.Close();
        }

        private void btRP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblIdReciboPrincipal.Text.Trim()))
            {
                CarregaDadosReciboPrincipal();
                if (!tcRecibos.TabPages.Contains(tpPrincipal))
                {
                    tcRecibos.TabPages.Add(tpPrincipal);
                }

                tcRecibos.SelectedTab = tpPrincipal;
            }
            else
            {
                Auxiliar.MostraMensagemAlerta("Por conta de algum erro não foi possivel carregar o Recibo.", 2);
            }
        }

        #endregion

        private void fechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
