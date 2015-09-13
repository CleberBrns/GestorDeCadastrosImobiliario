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
                idLocatario = 2;
                lblIdLocatario.Text = idLocatario.ToString();
                CarregaDadosReciboPrincipal();
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

            SqlCeCommand cmd = MetodosAuxiliares.retornaConexao().CreateCommand();

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
                cmd.CommandText = "select Lc.Locatario, Lc.Aluguel, Rp.Quantidade, Rp.Iptu, Rp.ParcelasIptu, Rp.DespesaCondominio, Rp.Luz, Rp.Agua" +
                                  " from Locatarios Lc" +
                                  " inner join RecibosPrincipais Rp" +
                                  " on Lc.Id = Rp.fkIdLocatario" +
                                  " where Lc.Ativo = 1 and Lc.Id = " + idLocatario + "";
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
                dr["Data"] = RetornaDataAtual();
                dr["Periodo"] = dtpInicial.ToString() + " a " + dtpFinal.ToString();
                dr["Iptu"] = RetornaValorCamposOpcionais(mtxtIptu, 2);
                dr["ParcelasIptu"] = nudParcelasIptu.Value.ToString();
                dr["DespesaCondominio"] = RetornaValorCamposOpcionais(mtxtDespCondominio, 2);
                dr["Luz"] = MetodosAuxiliares.FormataValorSalvar(mtxtLuz, 2);
                dr["Agua"] = MetodosAuxiliares.FormataValorSalvar(mtxtAgua, 2);
                dr["Aluguel"] = MetodosAuxiliares.FormataValorSalvar(mtxtAluguel, 1);
                dr["ComplementoPagamento"] = RetornaValorCamposOpcionais(mtxtCompPagamento, 2);
                dr["DescricaoComplementoPagamento"] = txtDescricaoCompPagto.Text.Trim();
                dr["FormaPagamento"] = txtFormaPagto.Text.Trim();
                dr["DataPagamento"] = dtpVencimento.Text.Trim();
                dr["TotalPagamento"] = MetodosAuxiliares.FormataValorSalvar(mtxtValorTotal, 2);
                dr["ExtensoTotalPagamento"] = txtExtensoValorTotal.Text.Trim();

            }
            else if (tipoInsercao == 2)
            {
                //dr["Locador"] = txtNomeLocador.Text.Trim();
                //dr["Cpf"] = FormataDocPrioritario(mtxtCpfLocador.Text.Trim(), 1);
                //dr["Cnpj"] = FormataDocPrioritario(mtxtCnpjLocador.Text.Trim(), 2);
                //dr["Contato"] = txtContatoLocador.Text.Trim();
                //dr["Ativo"] = 1;
                //dr["Endereco"] = txtEndereco.Text.Trim();
                //dr["Bairro"] = txtBairro.Text.Trim();
            }

            dsInclusao.Tables[0].Rows.Add(dr);
            //sdaInclusao.Update(dsInclusao);
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
            if (string.IsNullOrEmpty(MetodosAuxiliares.VerificaMtxtVazio(mtxtIptu)))
            {
                return 0;
            }
            else
            {
                return MetodosAuxiliares.FormataValorSalvar(maskedTxt, tipoValor);
            }
        }

        private string RetornaNumeroRecibo(string p)
        {
            throw new NotImplementedException();
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
            DataTable dadosLocatario = CarregaDadosTabela(3);

            if (dadosLocatario.Rows.Count > 0)
            {
                lblLocatario.Text = dadosLocatario.DefaultView[0]["Locatario"].ToString();
                mtxtAluguel.Text = MetodosAuxiliares.FormataValorArmazenado(dadosLocatario.DefaultView[0]["Aluguel"].ToString(), 1);
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
                if (MetodosAuxiliares.validaCampoMtxt(mtxtLuz, errorProvider1))
                {
                    if (MetodosAuxiliares.validaCampoMtxt(mtxtAgua, errorProvider1))
                    {
                        if (MetodosAuxiliares.validaCampoMtxt(mtxtAluguel, errorProvider1))
                        {
                            if (MetodosAuxiliares.validaCampoTxt(txtFormaPagto, errorProvider1))
                            {
                                if (MetodosAuxiliares.validaCampoMtxt(mtxtValorTotal, errorProvider1))
                                {
                                    if (MetodosAuxiliares.validaCampoTxt(txtExtensoValorTotal, errorProvider1))
                                    {
                                        try
                                        {
                                            InsereDados(1);
                                            MessageBox.Show("Recibo inserido com Sucesso!");
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

        private void btCalcular_Click(object sender, EventArgs e)
        {

        }

        private void btTranscreveValor_Click(object sender, EventArgs e)
        {
            try
            {
                if (MetodosAuxiliares.validaCampoMtxt(mtxtValorTotal, errorProvider1))
                {
                    decimal valorTotal = MetodosAuxiliares.FormataValorSalvar(mtxtValorTotal, 1);
                    txtExtensoValorTotal.Text = MetodosAuxiliares.valorPorExtenso(valorTotal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
