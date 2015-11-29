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

            lblComplemento1.Text = string.Empty;
            lblComplemento2.Text = string.Empty;   
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
        //    MemoryImage = new Bitmap(pnlImpressao.Width, pnlImpressao.Height);
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
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela, string idBusca)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = Auxiliar.retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select rl.Aluguel, rl.PorcentagemMulta, rl.Multa, rl.PorcentagemComissao, rl.Comissao, rl.Complemento, rl.DescricaoComplemento,"+
                                  " rl.Complemento2, rl.DescricaoComplemento2, rl.Complemento3, rl.DescricaoComplemento3, rl.Total, im.fkIdLocador1, im.fkIdLocador2" +
                                  " from RecibosLocadores rl" +
                                  " inner join RecibosPrincipais rp on rl.fkIdRecibo = rp.Id" +
                                  " inner join Locatarios lct on rp.fkIdLocatario = lct.Id" +
                                  " inner join Imoveis im on lct.fkIdImovel = im.Id" +
                                  " where rl.Id = " + idBusca + "";
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

        private void CarregaDadosReciboLocador()
        {
            try
            {
                DataTable dadosRecibo = CarregaDadosTabela(1, lblIdReciboLocador.Text.Trim());
                if (dadosRecibo.Rows.Count > 0)
                {
                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Aluguel"].ToString(), lblAluguel);
                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Aluguel"].ToString(), lblAluguel2);

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["PorcentagemMulta"].ToString()) &&
                        dadosRecibo.DefaultView[0]["PorcentagemMulta"].ToString() == "0")
                    {
                        lblPorcentMulta.Text = "Multa " + dadosRecibo.DefaultView[0]["PorcentagemMulta"].ToString().Trim() + " % R$:";
                        lblPorcentMulta2.Text = "Multa " + dadosRecibo.DefaultView[0]["PorcentagemMulta"].ToString().Trim() + " % R$:";
                    }

                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Multa"].ToString(), lblMulta);
                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Multa"].ToString(), lblMulta2);

                    CalculaExibeSubTotal1();

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["PorcentagemComissao"].ToString()) &&
                      dadosRecibo.DefaultView[0]["PorcentagemComissao"].ToString() == "0")
                    {
                        lblPorcentComissao.Text = "- Comissão " + dadosRecibo.DefaultView[0]["PorcentagemComissao"].ToString().Trim() + " % R$:";
                        lblPorcentComissao2.Text = "- Comissão " + dadosRecibo.DefaultView[0]["PorcentagemComissao"].ToString().Trim() + " % R$:";
                    }

                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Comissao"].ToString(), lblComissao);
                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Comissao"].ToString(), lblComissao2);

                    CalculaExibeSubTotal2();

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["Complemento"].ToString()) && dadosRecibo.DefaultView[0]["Complemento"].ToString() != "0")
                    {
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento"].ToString(), lblComplemento1);
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento"].ToString(), lblComplemento1_2);
                    }

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["Complemento2"].ToString()) && dadosRecibo.DefaultView[0]["Complemento2"].ToString() != "0")
                    {
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento2"].ToString(), lblComplemento2);
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento2"].ToString(), lblComplemento2_2);
                    }

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["Complemento3"].ToString()) && dadosRecibo.DefaultView[0]["Complemento3"].ToString() != "0")
                    {
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento3"].ToString(), lblComplemento3);
                        HabilitaLabelValor(dadosRecibo.DefaultView[0]["Complemento3"].ToString(), lblComplemento3_2);
                    }

                    //CalculaSubTotal3(Auxiliar.FormataValorParaUso(lblSubTotal2));

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["DescricaoComplemento"].ToString()))
                    {
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento"].ToString(), lblDescComp1);
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento"].ToString(), lblDescComp1_2);
                    }

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["DescricaoComplemento2"].ToString()))
                    {
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento2"].ToString(), lblDescComp2);
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento2"].ToString(), lblDescComp2_2);
                    }

                    if (!string.IsNullOrEmpty(dadosRecibo.DefaultView[0]["DescricaoComplemento3"].ToString()))
                    {
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento3"].ToString(), lblDescComp3);
                        HabilitaLabelTexto(dadosRecibo.DefaultView[0]["DescricaoComplemento3"].ToString(), lblDescComp3_2);
                    }


                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Total"].ToString(), lblTotal);
                    HabilitaLabelValor(dadosRecibo.DefaultView[0]["Total"].ToString(), lblTotal_2);

                    CarregaDadosLocadores(dadosRecibo);                    
                }
            }
            catch (Exception ex)
            {
                //Auxiliar.MostraMensagemAlerta(ex.ToString(), 3);
                Auxiliar.MostraMensagemAlerta("Falha ao carregador os dados do Usuário.", 3);
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
                        lblLocador2.Text = dtLocadores.DefaultView[i]["Locador"].ToString().Trim();
                        lblLocador2_2.Text = dtLocadores.DefaultView[i]["Locador"].ToString().Trim();

                        if (!string.IsNullOrEmpty(dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim()))
                        {
                            lblDocLocador2.Text = dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                            lblDocLocador2_2.Text = dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                        }
                        else
                        {
                            lblDocLocador2.Text = dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                            lblDocLocador2_2.Text = dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                        }

                        lblTextoLocador2.Visible = true;
                        lblTextoLocador2_2.Visible = true;

                        lblLocador2.Visible = true;
                        lblLocador2_2.Visible = true;

                        lblTextoDocs2.Visible = true;
                        lblTextoDocs2_2.Visible = true;

                        lblDocLocador2.Visible = true;
                        lblDocLocador2_2.Visible = true;
                    }
                    else
                    {
                        lblLocador.Text = dtLocadores.DefaultView[i]["Locador"].ToString().Trim();
                        lblLocador_2.Text = dtLocadores.DefaultView[i]["Locador"].ToString().Trim();                      

                        if (!string.IsNullOrEmpty(dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim()))
                        {
                            lblDocLocador.Text = dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                            lblDocLocador_2.Text = dtLocadores.DefaultView[i]["Cnpj"].ToString().Trim();
                        }
                        else
                        {
                            lblDocLocador.Text = dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                            lblDocLocador_2.Text = dtLocadores.DefaultView[i]["Cpf"].ToString().Trim();
                        }
                    }
                }                          
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

            if (!string.IsNullOrEmpty(lblComplemento1.Text.Trim()) && IsDecimalValido(lblComplemento1.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(lblComplemento1);
            }

            if (!string.IsNullOrEmpty(lblComplemento2.Text.Trim()) && IsDecimalValido(lblComplemento2.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(lblComplemento2) + dComplemento;
            }

            if (!string.IsNullOrEmpty(lblComplemento3.Text.Trim()) && IsDecimalValido(lblComplemento3.Text.Trim()))
            {
                dComplemento = Auxiliar.FormataValorParaUso(lblComplemento3) + dComplemento;
            }          
        }

        private bool IsDecimalValido(string valor)
        {
            try
            {
                decimal dTeste = Convert.ToDecimal(valor);
                return true;
            }
            catch (Exception ex)
            {
                return false;
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
