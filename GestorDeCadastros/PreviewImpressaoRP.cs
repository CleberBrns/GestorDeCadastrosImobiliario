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
                    Falha(string.Empty);
                }

            }
            catch (Exception ex)
            {
                Falha(ex.ToString());
            }
        }

        private void Falha(string erro)
        {
            //Auxiliar.MostraMensagemAlerta(ex.ToString(), 2);
            Auxiliar.MostraMensagemAlerta("Não foi possivel resgatar os dados da pesquisa.", 2);
            pnlImpressao.Visible = false;
            btImprimir.Visible = false;
            this.Close();
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

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    //MemoryImage = new Bitmap(pnlImpressao.Width, pnlImpressao.Height);
        //    e.Graphics.DrawImage(MemoryImage, 0, 0);
        //    base.OnPaint(e);
        //}

        void printdoc1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            int larguraFinal = (pagearea.Width / 2) - (pnlImpressao.Width / 2);
            int posicaoPainel = pnlImpressao.Location.Y;
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

            if (printDlg.ShowDialog() == DialogResult.OK)
                printDoc.Print();
        }

        #endregion

        #region Ações Banco de Dados

        /// <summary>
        /// tipoTabela 1 = Recibo Principal
        /// tipoTabela 2 = Locadores por Id
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

                cmd.CommandText = "select lct.Locatario, lct.CpfLocatario, lct.Locatario2, lct.CpfLocatario2," +
                                  " rp.Quantidade, rp.Numero, rp.Data, rp.Periodo, rp.Iptu, rp.Multa, rp.ParcelasIptu, rp.NumeroParcelaIptu," +
                                  " rp.DespesaCondominio, rp.Luz, rp.Agua, rp.Aluguel, rp.ComplementoPagamento, rp.DescricaoComplementoPagamento," +
                                  " rp.Observacao, rp.DataPagamento, rp.TotalPagamento, rp.ExtensoTotalPagamento, rp.ReajusteAluguel," +
                                  " im.Endereco, im.fkIdLocador1, im.fkIdLocador2" +
                                  " from RecibosPrincipais Rp" +
                                  " inner join Locatarios lct on rp.fkIdLocatario = lct.Id" +
                                  " inner join Imoveis im on lct.fkIdImovel = im.Id" +
                                  " where rp.Id = " + idBusca;
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select Id, Locador, Cpf, Cnpj from Locadores where id in (" + idBusca + ") and Ativo = 1 order by Locador";

            }

            sdaDados.SelectCommand = cmd;
            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        private DataTable CarregaDadosTabela(int tipoTabela, string idBusca)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;
            PreencheDataset(out dsSelecao, out sdaSelecao, tipoTabela, idBusca);

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        #endregion

        #region Carrega Dados Recibo

        private void CarregaDadosReciboPrincipal()
        {
            try
            {
                DataTable dadosLocatarios = CarregaDadosTabela(1, lblIdReciboPrincipal.Text.Trim());
                if (dadosLocatarios.Rows.Count > 0)
                {
                    string quantidade = dadosLocatarios.DefaultView[0]["Numero"].ToString() + "/" + dadosLocatarios.DefaultView[0]["Quantidade"].ToString();
                    HabilitaLabelTexto(quantidade, lblQtdNumeroRecibo);

                    if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["Periodo"].ToString()))
                    {
                        try
                        {
                            string[] aPeriodo = dadosLocatarios.DefaultView[0]["Periodo"].ToString().Split('a');
                            string periodo = "de " + aPeriodo[0].ToString() + " a " + aPeriodo[1].ToString();

                            HabilitaLabelTexto(periodo, lblPeriodo);
                        }
                        catch (Exception) { }
                    }

                    try
                    {
                        string vencimento = Convert.ToDateTime(dadosLocatarios.DefaultView[0]["DataPagamento"].ToString()).ToShortDateString();
                        HabilitaLabelTexto(vencimento, lblVencimento);
                    }
                    catch (Exception ex) { }

                    CarregaDadosLocatarios(dadosLocatarios);
                    CarregaDadosLocadores(dadosLocatarios);

                    HabilitaLabelTexto(dadosLocatarios.DefaultView[0]["Endereco"].ToString().Trim(), lblEnderecoImovel);

                    if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["DescricaoComplementoPagamento"].ToString()))
                    {
                        lblDescComplemento.Text = dadosLocatarios.DefaultView[0]["DescricaoComplementoPagamento"].ToString().Trim();
                        lblDescComplemento.Visible = true;
                    }

                    HabilitaLabelTexto(dadosLocatarios.DefaultView[0]["Observacao"].ToString(), lblObservacao);
                    if (!string.IsNullOrEmpty(lblObservacao.Text.Trim()))
                    {
                        lblTextoObervecao.Visible = true;
                    }

                    HabilitaTextBox(dadosLocatarios.DefaultView[0]["ExtensoTotalPagamento"].ToString(), txtTotalExtenso);
                    HabilitaLabelTexto(dadosLocatarios.DefaultView[0]["ReajusteAluguel"].ToString(), lblReajuste);

                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["Aluguel"].ToString(), lblAluguel);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["Iptu"].ToString(), lblIptu);

                    if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["ParcelasIptu"].ToString()) && dadosLocatarios.DefaultView[0]["ParcelasIptu"].ToString() != "0")
                    {
                        if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["NumeroParcelaIptu"].ToString()) &&
                       dadosLocatarios.DefaultView[0]["NumeroParcelaIptu"].ToString() != "0")
                        {
                            string quatidadeIptu = dadosLocatarios.DefaultView[0]["NumeroParcelaIptu"].ToString() + "/" +
                                                   dadosLocatarios.DefaultView[0]["ParcelasIptu"].ToString();

                            HabilitaLabelTexto(quatidadeIptu, lblQtdNumeroIptu);
                            if (lblQtdNumeroIptu.Visible == true)
                            {
                                lblParcelasIptu.Visible = true;
                            }
                        }
                    }

                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["DespesaCondominio"].ToString(), lblDespCondominio);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["Luz"].ToString(), lblLuz);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["Agua"].ToString(), lblAgua);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["ComplementoPagamento"].ToString(), lblCompPagamento);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["Multa"].ToString(), lblMulta);
                    HabilitaLabelValor(dadosLocatarios.DefaultView[0]["TotalPagamento"].ToString(), lblTotal);

                    //lblDataFormatada.Text = "São Paulo, " + RetornaDataFormatada();

                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);               
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Recibo.", 3);
            }
        }

        private void CarregaDadosLocatarios(DataTable dadosLocatarios)
        {
            lblLocatarios.Text = dadosLocatarios.DefaultView[0]["Locatario"].ToString();
            lblLocatarios.Visible = true;

            if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["Locatario2"].ToString().Trim()))
            {
                lblLocatarios.Text += " / " + dadosLocatarios.DefaultView[0]["Locatario2"].ToString().Trim();
            }

            lblCpfLocatarios.Text = dadosLocatarios.DefaultView[0]["CpfLocatario"].ToString().Trim();
            lblCpfLocatarios.Visible = true;

            if (!string.IsNullOrEmpty(dadosLocatarios.DefaultView[0]["CpfLocatario2"].ToString().Trim()))
            {
                lblCpfLocatarios.Text += " / " + dadosLocatarios.DefaultView[0]["CpfLocatario2"].ToString().Trim();
            }
        }

        private void CarregaDadosLocadores(DataTable dadosLocatario)
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
                DataTable dtLocadores = CarregaDadosTabela(2, idLocadores);

                string locadores = string.Empty;
                string docLocadores = string.Empty;

                for (int i = 0; i < dtLocadores.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        locadores += " / " + dtLocadores.DefaultView[i]["Locador"].ToString().Trim();

                        if (!string.IsNullOrEmpty(dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim()))
                        {
                            docLocadores += " / " + dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                        }
                        else
                        {
                            docLocadores += " / " + dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                        }
                    }
                    else
                    {
                        locadores += dtLocadores.DefaultView[i]["Locador"].ToString().Trim();

                        if (!string.IsNullOrEmpty(dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim()))
                        {
                            docLocadores += dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                        }
                        else
                        {
                            docLocadores += dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                        }
                    }
                }

                lblLocadores.Text = locadores;
                lblLocadores.Visible = true;

                lblDocLocadores.Text = docLocadores;
                lblDocLocadores.Visible = true;
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
