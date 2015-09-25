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
    public partial class PreviewImpressaoRL : Form
    {
        private int idReciboLocador;

        public int getIdReciboLocador
        {
            get { return idReciboLocador; }
            set { idReciboLocador = value; }
        }

        public PreviewImpressaoRL()
        {
            InitializeComponent();

            printDocument1.PrintPage += new PrintPageEventHandler(printdoc1_PrintPage);
        }

        #region Ações Gerais

        private void ImpressaoRL_Load(object sender, EventArgs e)
        {
            try
            {
                //idReciboLocador = 1;//Para Testes
                if (idReciboLocador != 0)
                {
                    lblIdReciboLocador.Text = idReciboLocador.ToString();
                    LimpaCamposDefault();
                    CarregaDadosReciboLocador();
                }
                else
                {
                    //Auxiliar.MostraMensagemAlerta(ex.ToString(), 2);
                    Auxiliar.MostraMensagemAlerta("Não foi possivel carregar dados para essa pesquisa", 2);
                }

            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 2);
                Auxiliar.MostraMensagemAlerta("Não foi possivel resgatar os dados da pesquisa.", 2);
            }
        }

        private void LimpaCamposDefault()
        {
            lblAluguel.Text = string.Empty;
            lblAluguel2.Text = string.Empty;

            lblMulta.Text = string.Empty;
            lblMulta2.Text = string.Empty;

            lblSubTotal1.Text = string.Empty;
            lblSubTotal1_2.Text = string.Empty;

            lblComissao.Text = string.Empty;
            lblComissao2.Text = string.Empty;

            lblSubTotal2.Text = string.Empty;
            lblSubTotal2_2.Text = string.Empty;

            lblComplemento.Text = string.Empty;
            lblComplemento2.Text = string.Empty;

            lblSubTotal3.Text = string.Empty;
            lblSubTotal3_2.Text = string.Empty;
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

            cmd.CommandText = "select lcd.Locador, lcd.Cpf, lcd.Cnpj, rl.Aluguel, rl.PorcentagemMulta, rl.Multa, rl.PorcentagemComissao, rl.Comissao, rl.Complemento," +
                              " rl.DescricaoComplemento, rl.Total" +
                              " from RecibosLocadores rl" +
                              " inner join RecibosPrincipais rp" +
                              " on rl.fkIdRecibo = rp.Id" +
                              " inner join Locatarios lct" +
                              " on rp.fkIdLocatario = lct.Id" +
                              " inner join Locadores lcd" +
                              " on lct.fkIdLocador = lcd.Id" +
                              " where rl.Id = " + idCadastro + "";

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

        private void CarregaDadosReciboLocador()
        {
            try
            {
                DataTable dadosLocatario = CarregaDadosTabela(Convert.ToInt32(lblIdReciboLocador.Text.Trim()));
                if (dadosLocatario.Rows.Count > 0)
                {
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), lblAluguel);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), lblAluguel2);              
                    
                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["PorcentagemMulta"].ToString()) &&
                        dadosLocatario.DefaultView[0]["PorcentagemMulta"].ToString() == "0")
                    {
                        lblPorcentMulta.Text = "Multa " + dadosLocatario.DefaultView[0]["PorcentagemMulta"].ToString().Trim() + " % R$:";
                        lblPorcentMulta2.Text = "Multa " + dadosLocatario.DefaultView[0]["PorcentagemMulta"].ToString().Trim() + " % R$:";
                    }

                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Multa"].ToString(), lblMulta);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Multa"].ToString(), lblMulta2);

                    CalculaExibeSubTotal1();

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString()) &&
                      dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString() == "0")
                    {
                        lblPorcentComissao.Text = "- Comissão " + dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString().Trim() + " % R$:";
                        lblPorcentComissao2.Text = "- Comissão " + dadosLocatario.DefaultView[0]["PorcentagemComissao"].ToString().Trim() + " % R$:";
                    }

                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Comissao"].ToString(), lblComissao);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Comissao"].ToString(), lblComissao2);                    

                    CalculaExibeSubTotal2();                    

                    if (!string.IsNullOrEmpty(dadosLocatario.DefaultView[0]["Complemento"].ToString()) && dadosLocatario.DefaultView[0]["Complemento"].ToString() != "0")
                    {
                        pnlSubTotal3.Visible = true;
                        pnlSubTotal3_2.Visible = true;
                        HabilitaLabelValor(dadosLocatario.DefaultView[0]["Complemento"].ToString(), lblComplemento);
                        HabilitaLabelValor(dadosLocatario.DefaultView[0]["Complemento"].ToString(), lblComplemento2);

                        CalculaSubTotal3(Auxiliar.FormataValorParaUso(lblComplemento));
                    }

                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Total"].ToString(), lblTotal);
                    HabilitaLabelValor(dadosLocatario.DefaultView[0]["Total"].ToString(), lblTotal2); 

                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Locador"].ToString(), lblLocador);
                    HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Locador"].ToString(), lblLocador2);

                    if (VerificaCampoDoc(dadosLocatario.DefaultView[0]["Cpf"].ToString()))
                    {
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cpf"].ToString(), lblDocLocador);
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cpf"].ToString(), lblDocLocador2);
                    }
                    else if (VerificaCampoDoc(dadosLocatario.DefaultView[0]["Cnpj"].ToString()))
                    {
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cnpj"].ToString(), lblDocLocador);
                        HabilitaLabelTexto(dadosLocatario.DefaultView[0]["Cnpj"].ToString(), lblDocLocador2);
                    }

                    HabilitaTextBox(dadosLocatario.DefaultView[0]["DescricaoComplemento"].ToString(), txtDescricComplemento, pnlDescricComplemento);
                    HabilitaTextBox(dadosLocatario.DefaultView[0]["DescricaoComplemento"].ToString(), txtDescricComplemento2, pnlDescricComplemento2);
                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Usuário.", 3);
            }
        }

        private void CalculaExibeSubTotal1()
        {
            if (!string.IsNullOrEmpty(lblAluguel.Text.Trim()) && !string.IsNullOrEmpty(lblMulta.Text.Trim()))
            {
                decimal subTotal1 = Auxiliar.FormataValorParaUso(lblAluguel) + Auxiliar.FormataValorParaUso(lblMulta);
                lblSubTotal1.Text = subTotal1.ToString();
                lblSubTotal1_2.Text = subTotal1.ToString();
            }
            else if (!string.IsNullOrEmpty(lblAluguel.Text.Trim()) && string.IsNullOrEmpty(lblMulta.Text.Trim()))
            {
                lblSubTotal1.Text = lblAluguel.Text;
                lblSubTotal1_2.Text = lblAluguel.Text;
            }
        }

        private void CalculaExibeSubTotal2()
        {
            if (!string.IsNullOrEmpty(lblSubTotal1.Text.Trim()))
            {
                decimal valor1 = Auxiliar.FormataValorParaUso(lblSubTotal1);
                decimal valor2 = 0;

                if (!string.IsNullOrEmpty(lblComissao.Text.Trim()))
                {
                    valor2 = Auxiliar.FormataValorParaUso(lblComissao);
                }

                lblSubTotal2.Text = Auxiliar.FormataValoresExibicao((valor1 - valor2).ToString());
                lblSubTotal2_2.Text = Auxiliar.FormataValoresExibicao((valor1 - valor2).ToString());
            }
        }

        private void CalculaSubTotal3(decimal subTotal2)
        {
            decimal dComplemento = 0;

            if (!string.IsNullOrEmpty(lblComplemento.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(lblComplemento);
            }

            lblSubTotal3.Text = Auxiliar.FormataValoresExibicao((subTotal2 - dComplemento).ToString());
            lblSubTotal3_2.Text = Auxiliar.FormataValoresExibicao((subTotal2 - dComplemento).ToString());
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

        private void HabilitaLabelValor(string textoExibicao, Label lblExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao) && textoExibicao != "0")
            {
                lblExibicao.Text = Auxiliar.FormataValoresExibicao(textoExibicao);
                lblExibicao.Visible = true;
            }
        }

        private void HabilitaTextBox(string textoExibicao, TextBox txtExibicao, Panel pnlExibicao)
        {
            if (!string.IsNullOrEmpty(textoExibicao))
            {
                txtExibicao.Text = textoExibicao;
                pnlExibicao.Visible = true;
            }
        }

        #endregion

    }
}
