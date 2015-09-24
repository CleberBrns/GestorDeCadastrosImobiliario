using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace GestorDeCadastros
{
    public partial class PreviewImpressaoRP : Form
    {
        private int idReciboPrincipal;

        public int getIdReciboPrincipal
        {
            get { return idReciboPrincipal; }
            set { idReciboPrincipal = value; }
        }

        public PreviewImpressaoRP()
        {
            InitializeComponent();
        }

        private void PreviewImpressaoRP_Load(object sender, EventArgs e)
        {
            try
            {
                idReciboPrincipal = 1;
                if (idReciboPrincipal != 0)
                {
                    lblIdReciboPrincipal.Text = idReciboPrincipal.ToString();
                    CarregaDadosReciboPrincipal();
                }
                else
                {
                    Auxiliar.MostraMensagemAlerta("Não foi possivel carregar dados para essa pesquisa", 2);
                }

            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 2);
                Auxiliar.MostraMensagemAlerta("Não foi possivel resgatar os dados da pesquisa.", 2);
            }
        }

        #region Ações Banco de Dados

        /// <summary>
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        /// <param name="idLocatario"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int idCadastro)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();


            cmd.CommandText = "select lct.Locatario, rp.Quantidade, rp.Numero, rp.Data, rp.Periodo, rp.Iptu, rp.ParcelasIptu, rp.NumeroParcelaIptu, rp.DespesaCondominio," +
                              " rp.Luz, rp.Agua, rp.Aluguel, rp.ComplementoPagamento, rp.DescricaoComplementoPagamento, rp.FormaPagamento, rp.DataPagamento," +
                              " rp.TotalPagamento, rp.ExtensoTotalPagamento, rp.ReajusteAluguel, lcd.Locador, lcd.Cpf, lcd.Cnpj, lct.CpfLocatario, im.Endereco" +
                              " from RecibosPrincipais Rp" +
                              " inner join Locatarios lct" +
                              " on rp.fkIdLocatario = lct.Id" +
                              " inner join Imoveis im" +
                              " on lct.fkIdImovel = im.Id" +
                              " inner join Locadores lcd" +
                              " on lct.fkIdLocador = lcd.Id" +
                              " where rp.Id = " + idCadastro;

            sdaDados.SelectCommand = cmd;
            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        private DataTable CarregaDadosTabela(int idCadastro)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, idCadastro);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        #endregion

        #region Ações Gerais

        private string RetornaDataAtual()
        {
            return DateTime.Now.ToShortDateString();
        }

        #endregion

        #region Dados Recibo Principal

        private void CarregaDadosReciboPrincipal()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(Convert.ToInt32(lblIdReciboPrincipal.Text.Trim()));
                if (dadosLocatario.Rows.Count > 0)
                {
                    string quantidade = dadosLocatario.DefaultView[0]["Quantidade"].ToString() + "/" + dadosLocatario.DefaultView[0]["Numero"].ToString();
                    HabilitaCampoTexto(quantidade, lblNumeracaoRecibo);

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Periodo"].ToString()))
                    {
                        try
                        {
                            string[] aPeriodo = dadosLocatario.DefaultView[0]["Periodo"].ToString().Split('a');
                            string periodo = "de " + aPeriodo[0].ToString() + " a " + aPeriodo[1].ToString();

                            HabilitaCampoTexto(periodo, lblPeriodo);
                        }
                        catch (Exception) { }
                    }

                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["Locatario"].ToString(), lblLocatario);
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["Endereco"].ToString(), lblEndImovel);
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["Locador"].ToString(), lblLocador);

                    //VerificacaoDeDocumentoSalvo
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["Cpf"].ToString(), lblDocLocador);

                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["DescricaoComplementoPagamento"].ToString(), lblDescricaoComplemento);
                    if (lblDescricaoComplemento.Visible == true)
                    {
                        txtCampoDescComp.Visible = true;
                    }
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["FormaPagamento"].ToString(), lblFormaPagamento);
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["ExtensoTotalPagamento"].ToString(), lblTotalPorExtenso);
                    HabilitaCampoTexto(dadosLocatario.DefaultView[0]["ReajusteAluguel"].ToString(), lblReajuste);

                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), lblAlugel);
                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["Iptu"].ToString(), lblItpu);

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString()) && dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() != "0")
                    {
                        if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString()) &&
                       dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString() != "0")
                        {
                            string quatidadeIptu = dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() + "/" +
                                                   dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString();

                            HabilitaCampoTexto(quatidadeIptu, lblNumeroParcelasIptu);
                            if (lblNumeroParcelasIptu.Visible == true)
                            {
                                lblParcelasIptu.Visible = true;
                            }
                        }
                    }

                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString(), lblDespCondominio);
                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["Luz"].ToString(), lblLuz);
                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["Agua"].ToString(), lblAgua);
                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["ComplementoPagamento"].ToString(), lblComplemento);
                    HabilitaCampoValor(dadosLocatario.DefaultView[0]["TotalPagamento"].ToString(), lblTotal);

                    lblDataFormatada.Text = "São Paulo, " + DateTime.Now.ToLongDateString();
                    
                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);               
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
            }
        }

        private void HabilitaCampoTexto(string textoExibicao, Label lblExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao))
            {
                lblExibicao.Text = textoExibicao;
                lblExibicao.Visible = true;
            }
        }

        private void HabilitaCampoValor(string textoExibicao, Label lblExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao) && textoExibicao != "0")
            {
                lblExibicao.Text = Auxiliar.FormataValoresExibicao(textoExibicao);
                lblExibicao.Visible = true;
            }
        }

        #endregion
    }
}
