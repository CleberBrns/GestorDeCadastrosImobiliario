using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
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

            printDocument1.PrintPage += new PrintPageEventHandler(printdoc1_PrintPage);
            lblInfoImobiliaria.Text = "CCM.8.548.409-1 — CRECI 15373 — SIND. 17.522 — CPF/MF. 608.750.468-00";
        }

        #region Ações Gerais

        private void ImpressaoRP_Load(object sender, EventArgs e)
        {
            try
            {
                //idReciboPrincipal = 1;//Para Testes
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

        #endregion

        #region Impressão

        private void btImprimir_Click(object sender, EventArgs e)
        {
            Print(pnlImpressao);
        }

        Bitmap MemoryImage;
        public void GetPrintArea(Panel pnl)
        {
            int pnlWidth = pnl.Width;
            int pnlHeight = pnl.Height;

            MemoryImage = new Bitmap(pnlWidth, pnlHeight);
            Rectangle rect = new Rectangle(0, 0, pnlWidth, pnlHeight);
            pnl.DrawToBitmap(MemoryImage, new Rectangle(0, 0, pnlWidth, pnlHeight));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MemoryImage = new Bitmap(pnlImpressao.Width, pnlImpressao.Height);
            e.Graphics.DrawImage(MemoryImage, 0, 0);
            base.OnPaint(e);
        }

        void printdoc1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(MemoryImage, 12, 12);
        }

        public void Print(Panel pnl)
        {
            GetPrintArea(pnl);

            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = printDocument1;
            printDoc.DocumentName = "Print Document";
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            //Call ShowDialog
            if (printDlg.ShowDialog() == DialogResult.OK)
                printDoc.Print();
        }

        #endregion

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

        #region Carrega Dados Recibo

        private void CarregaDadosReciboPrincipal()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(Convert.ToInt32(lblIdReciboPrincipal.Text.Trim()));
                if (dadosLocatario.Rows.Count > 0)
                {
                    string quantidade = dadosLocatario.DefaultView[0]["Quantidade"].ToString() + "/" + dadosLocatario.DefaultView[0]["Numero"].ToString();
                    HabilitaLabelTexto(quantidade, lblQtdNumeroRecibo);

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Periodo"].ToString()))
                    {
                        try
                        {
                            string[] aPeriodo = dadosLocatario.DefaultView[0]["Periodo"].ToString().Split('a');
                            string periodo = "de " + aPeriodo[0].ToString() + " a " + aPeriodo[1].ToString();

                            HabilitaLabelTexto(periodo, lblPeriodo);
                        }
                        catch (Exception) { }
                    }

                    try
                    {
                        string vencimento = Convert.ToDateTime(dadosLocatario.DefaultView[0]["DataPagamento"].ToString()).ToShortDateString();
                        HabilitaLabelTexto(vencimento, lblVencimento);
                    }
                    catch (Exception ex) { }

                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Locatario"].ToString().Trim(), lblLocatario);
                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["CpfLocatario"].ToString().Trim(), lblCpfLocatario);
                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Endereco"].ToString().Trim(), lblEnderecoImovel);
                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Locador"].ToString().Trim(), lblLocador);                    

                    //VerificacaoDeDocumentoSalvo
                    if (VerificaCampoDoc(dadosLocatario.DefaultView[0]["Cpf"].ToString()))
                    {
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cpf"].ToString().Trim(), lblDocLocador);
                    }
                    else if (VerificaCampoDoc(dadosLocatario.DefaultView[0]["Cnpj"].ToString()))
                    {
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cnpj"].ToString().Trim(), lblDocLocador);
                    }

                    HabilitaTextBox(dadosLocatario.DefaultView[0]["DescricaoComplementoPagamento"].ToString(), txtDescComplemento);

                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["FormaPagamento"].ToString(), lblFormaPagamento);
                    HabilitaTextBox(dadosLocatario.DefaultView[0]["ExtensoTotalPagamento"].ToString(), txtTotalExtenso);
                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["ReajusteAluguel"].ToString(), lblReajuste);

                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), lblAluguel);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Iptu"].ToString(), lblIptu);

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString()) && dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() != "0")
                    {
                        if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString()) &&
                       dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString() != "0")
                        {
                            string quatidadeIptu = dadosLocatario.DefaultView[0]["ParcelasIptu"].ToString() + "/" +
                                                   dadosLocatario.DefaultView[0]["NumeroParcelaIptu"].ToString();

                            HabilitaLabelTexto(quatidadeIptu, lblQtdNumeroIptu);
                            if (lblQtdNumeroIptu.Visible == true)
                            {
                                lblParcelasIptu.Visible = true;
                            }
                        }
                    }

                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["DespesaCondominio"].ToString(), lblDespCondominio);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Luz"].ToString(), lblLuz);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Agua"].ToString(), lblAgua);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["ComplementoPagamento"].ToString(), lblComPagamento);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["TotalPagamento"].ToString(), lblTotal);

                    lblDataFormatada.Text = "São Paulo, " + RetornaDataFormatada();

                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);               
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
            }
        }

        private string RetornaDataFormatada()
        {
            try
            {
                string longDate = DateTime.Now.ToLongDateString();
                string[] aData = longDate.Split(',');

                return aData[1].ToString().Trim();
            }
            catch (Exception)
            {
                return DateTime.Now.ToShortDateString();
            }
        }

        private bool VerificaCampoDoc(string docLocador)
        {
            bool docValidado = false;
            string docLitera = docLocador.Replace(".", string.Empty).Trim().Replace("/", string.Empty).Trim().Replace("-", string.Empty).Trim();

            if (!string.IsNullOrEmpty(docLitera.Trim()))
            {
                docValidado = true;
            }

            return docValidado;
        }

        private void HabilitaLabelTexto(string textoExibicao, Label lblExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao))
            {
                lblExibicao.Text = textoExibicao;
                lblExibicao.Visible = true;
            }
        }

        private void HabilitaTextBox(string textoExibicao, TextBox txtExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao))
            {
                txtExibicao.Text = textoExibicao;
                txtExibicao.Visible = true;
            }
        }

        private void HabilitaLabelValor(string textoExibicao, Label lblExibicao)
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
