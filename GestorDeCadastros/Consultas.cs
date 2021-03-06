﻿using System;
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
    public partial class Consultas : Form
    {
        public Consultas()
        {
            //tipoTabela 1 = Endereço
            //tipoTabela 2 = Locador
            //tipoTabela 3 = Locatário
            //tipoTabela 4 = Recibos Principais
            //tipoTabela 5 = Recibos Locadores

            InitializeComponent();

            //Auxiliar.CentralizaControle(tcConsultas, this);
        }

        #region Ações Gerais

        private void tcConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaAbas();
        }

        private void CarregaAbas()
        {
            if (tcConsultas.SelectedTab == tpRcbPrincipais)
            {
                CarregaDadosAba(4);
            }
            else if (tcConsultas.SelectedTab == tpLocadores)
            {
                CarregaDadosAba(2);
            }
            else if (tcConsultas.SelectedTab == tpImoveis)
            {
                CarregaDadosAba(1);
            }
        }

        public void CarregaDadosAba(int tipoCadastro)
        {
            if (tipoCadastro == 4)
            {

            }
            else if (tipoCadastro == 2)
            {
                CarregaDadosAbaLocadores();
            }
            else if (tipoCadastro == 1)
            {
                CarregaDadosAbaImoveis();
            }
        }

        private void btInicio_Click(object sender, EventArgs e)
        {
            Inicio formInicio = new Inicio();
            this.Hide();
            formInicio.ShowDialog();
            this.Close();
        }

        private void defineDataFinal(DateTimePicker dtpIncio, DateTimePicker dtpFim)
        {
            if (dtpIncio.Value.Month != dtpFim.Value.Month)
            {
                DateTime proximoMes = dtpFim.Value.AddMonths(1);
                dtpFim.Value = proximoMes;
            }
        }

        private void selecionaProximoMes_ValueChanged(object sender, EventArgs e)
        {
            if (sender == dtpInicialRP)
            {
                defineDataFinal(dtpInicialRP, dtpFinalRP);
            }
        }

        #endregion

        #region Ações Banco de Dados

        private static SqlCeConnection retornaConexao()
        {
            SqlCeConnection conexao = new SqlCeConnection("Data Source="
               + System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CadastrosAplicacao.sdf"));

            string path = System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CadastrosAplicacao.sdf");

            return conexao;
        }

        /// <summary>
        /// tipoTabela 1 = Endereço
        /// tipoTabela 2 = Locador
        /// tipoTabela 3 = Locatário
        /// tipoTabela 4 = Recibos Principais
        /// tipoTabela 5 = Recibos Locadores
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        private static void PreencheDataset(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select * from Imoveis where Ativo = 1";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select * from Locadores where Ativo = 1";
            }
            else if (tipoTabela == 3)
            {
                cmd.CommandText = "select * from Locatarios where Ativo = 1";
            }
            else if (tipoTabela == 4)
            {
                cmd.CommandText = "select * from RecibosPrincipais";
            }
            else if (tipoTabela == 5)
            {
                cmd.CommandText = "select * from RecibosLocadores";
            }

            sdaDados.SelectCommand = cmd;

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>
        /// tipoTabela 1 = Imóveis
        /// tipoTabela 2 = Locador
        /// tipoTabela 3 = Locatário
        /// tipoTabela 4 = Recibos Principais
        /// tipoTabela 5 = Recibos Locadores
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoTabela"></param>
        private static void PreencheDatasetPorId(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoTabela, int idCadastro)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = retornaConexao().CreateCommand();

            if (tipoTabela == 1)
            {
                cmd.CommandText = "select Id, Endereco, Bairro, Cidade, Cep "+
                    "from Imoveis where Ativo = 1 and Id = " + idCadastro + "";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select Id, Locador, Cpf, Cnpj  " +
                "from Locadores where Ativo = 1 and Id = " + idCadastro + "";
            }

            sdaDados.SelectCommand = cmd;

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>
        /// TipoPesquisa 1 = Endereço
        /// TipoPesquisa 2 = Locador
        /// TipoPesquisa 3 = Locatário
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoPesquisa"></param>
        /// <param name="valorTabela"></param>
        private static void PreenchePesquisaLocatario(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoPesquisa, string valorTabela)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = retornaConexao().CreateCommand();

            if (tipoPesquisa == 1)
            {
                cmd.CommandText = "select lct.Id, lct.Locatario, lct.CpfLocatario, Im.Endereco, lcds.Locador from Locatarios lct "+
                    " inner join Imoveis Im on lct.fkIdImovel = Im.Id" +
                    " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id" +
                    " where lct.Ativo = 1 and lct.fkIdImovel = " + valorTabela + "";
            }
            else if (tipoPesquisa == 2)
            {          
                cmd.CommandText = "select lct.Id, lct.Locatario, lct.CpfLocatario, Im.Endereco, lcds.Locador from Locatarios lct "+
                   " inner join Imoveis Im on lct.fkIdImovel = Im.Id" +
                   " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id" +
                   " where lct.Ativo = 1 and lcds.Id = " + valorTabela + "";
            }
            else
            {
                cmd.CommandText = "select lct.Id, lct.Locatario, lct.CpfLocatario, Im.Endereco, lcds.Locador  from Locatarios lct"+
                    " inner join Imoveis Im on lct.fkIdImovel = Im.Id" +
                    " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id" +
                    " where lct.Ativo = 1 and lct.Id = " + valorTabela + "";
            }

            sdaDados.SelectCommand = cmd;

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>
        /// TipoPesquisa 0 = Somente por data
        /// TipoPesquisa 1 = Endereço
        /// TipoPesquisa 2 = Locador
        /// TipoPesquisa 3 = Locatário
        /// </summary>
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoPesquisa"></param>
        /// <param name="idCadastro"></param>
        private static void PreenchePesquisaRecibosPrincipais(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoPesquisa, int idCadastro, DateTime dataIncial, DateTime dataFinal)
        {
            sdaDados = new SqlCeDataAdapter();

            SqlCeCommand cmd = retornaConexao().CreateCommand();

            string formatoDataBD = "yyyy/M/d";

            string comandoData = string.Empty;

            string sdataInicial = dataIncial.ToString(formatoDataBD) + " 00:00:00";
            string sdataFinal = dataFinal.ToString(formatoDataBD) + " 23:59:59";

            //Data Igual pesquisa o mesmo dia, o dia todo
            if (dataIncial.ToShortDateString() == dataFinal.ToShortDateString())
            {
                sdataFinal = dataFinal.ToString(formatoDataBD) + " 23:59:59";
            }

            comandoData = " where rp.Data between '" + sdataInicial + "' and '" + sdataFinal + "'";


            if (tipoPesquisa == 0)
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP," +
                                                " rp.Data as DataReciboRP, rl.Id as IdRL" +
                                                " from RecibosPrincipais rp" +
                                                " inner join Locatarios lct on lct.Id = rp.fkIdLocatario " +
                                                " inner join Imoveis im on lct.fkIdImovel = im.Id " +
                                                " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id " +
                                                " inner join RecibosLocadores rl on rl.fkIdRecibo = rp.Id " +
                                                comandoData + " order by rp.Data desc";
            }
            else if (tipoPesquisa == 1)
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP," +
                                                " rp.Data as DataReciboRP, rl.Id as IdRL" +
                                                " from RecibosPrincipais rp" +
                                                " inner join Locatarios lct on lct.Id = rp.fkIdLocatario " +
                                                " inner join Imoveis im on lct.fkIdImovel = im.Id " +
                                                " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id " +
                                                " inner join RecibosLocadores rl on rl.fkIdRecibo = rp.Id " +
                                                comandoData + " and im.Id = " + idCadastro + " order by rp.Data desc";
            }
            else if (tipoPesquisa == 2)
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP," +
                                " rp.Data as DataReciboRP, rl.Id as IdRL" +
                                " from RecibosPrincipais rp" +
                                " inner join Locatarios lct on lct.Id = rp.fkIdLocatario " +
                                " inner join Imoveis im on lct.fkIdImovel = im.Id " +
                                " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id " +
                                " inner join RecibosLocadores rl on rl.fkIdRecibo = rp.Id " +
                                comandoData + " and lcds.Id = " + idCadastro + " order by rp.Data desc";
            }
            else
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP," +
                                  " rp.Data as DataReciboRP, rl.Id as IdRL" +
                                  " from RecibosPrincipais rp" +
                                  " inner join Locatarios lct on lct.Id = rp.fkIdLocatario " +
                                  " inner join Imoveis im on lct.fkIdImovel = im.Id " +
                                  " inner join Locadores lcds on im.fkIdLocador1 = lcds.Id " +
                                  " inner join RecibosLocadores rl on rl.fkIdRecibo = rp.Id " +
                                  comandoData + " and lct.Id = " + idCadastro + " order by rp.Data desc";
            }

            sdaDados.SelectCommand = cmd;

            dsDados = new DataSet();
            sdaDados.Fill(dsDados);
        }

        /// <summary>
        /// TipoPesquisa 1 = Endereço
        /// TipoPesquisa 2 = Locador
        /// TipoPesquisa 3 = Locatário
        /// </summary>
        /// <param name="tipoPesquisa"></param>
        private void CarregaComboPesquisa(int tipoPesquisa, ComboBox cbPesquisa)
        {
            DataTable tabelaCombo = PreencheDadosTabela(tipoPesquisa, 1, 0, DateTime.Now, DateTime.Now);
            if (tabelaCombo.Rows.Count > 0)
            {
                string valorExibicao = string.Empty;

                if (tipoPesquisa == 1)
                {
                    valorExibicao = "Endereco";
                }
                else if (tipoPesquisa == 2)
                {
                    valorExibicao = "Locador";
                }
                else
                {
                    valorExibicao = "Locatario";
                }

                tabelaCombo.Rows.Add(0, "Por favor, selecione um item...");
                tabelaCombo.DefaultView.Sort = "Id";

                cbPesquisa.DataSource = tabelaCombo.DefaultView;
                cbPesquisa.DisplayMember = valorExibicao;
                cbPesquisa.ValueMember = "Id";
                cbPesquisa.Enabled = true;
            }
            else
            {
                ComboDefaultLct(cbPesquisa);
            }
        }

        private static void ComboDefaultLct(ComboBox cbPesquisa)
        {
            DataTable dtSemDados = new DataTable();

            dtSemDados.Columns.Add("Descricao");
            dtSemDados.Columns.Add("Id");

            dtSemDados.Rows.Add("Faltam dados cadastrados para exibir", 0);

            cbPesquisa.DataSource = dtSemDados;
            cbPesquisa.DisplayMember = "Descricao";
            cbPesquisa.ValueMember = "Id";

            cbPesquisa.Enabled = false;
        }

        /// <summary>
        /// tipoCarramento 1 = Carrega Combos
        /// tipoCarramento 2 = Carrega Grid Aba Locatarios
        /// tipoCarramento 3 = Carrega Grid Aba Locadores e Imovéis
        /// tipoPesquisa 1 = Imovel
        /// tipoPesquisa 2 = Locador
        /// tipoPesquisa 3 = Locatário
        /// tipoPesquisa 4 = Recibos Principais
        /// tipoPesquisa 5 = Recibos Locadores
        /// </summary>
        /// <param name="tipoPesquisa"></param>
        /// <param name="tipoCarramento"></param>
        /// <returns></returns>
        private DataTable PreencheDadosTabela(int tipoPesquisa, int tipoCarramento, int idCadastro, DateTime dataIncial, DateTime dataFinal)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao = new DataSet();

            if (tipoCarramento == 1)
            {
                PreencheDataset(out dsSelecao, out sdaSelecao, tipoPesquisa);
            }
            else if (tipoCarramento == 2)
            {
                PreenchePesquisaLocatario(out dsSelecao, out sdaSelecao, tipoPesquisa, idCadastro.ToString());
            }
            else if (tipoCarramento == 3)
            {
                PreencheDatasetPorId(out dsSelecao, out sdaSelecao, tipoPesquisa, idCadastro);
            }
            else if (tipoCarramento == 4)
            {
                PreenchePesquisaRecibosPrincipais(out dsSelecao, out sdaSelecao, tipoPesquisa, idCadastro, dataIncial, dataFinal);
            }

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        #endregion

        #region Ações tab Locatarios

        private void rbTipoPesquisa_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEndereco.Checked)
            {
                CarregaComboPesquisa(1, cbTipoPesquisa);
            }
            else if (rbLocador.Checked)
            {
                CarregaComboPesquisa(2, cbTipoPesquisa);
            }
            else if (rbLocatario.Checked)
            {
                CarregaComboPesquisa(3, cbTipoPesquisa);
            }

            DadosDefaulGrid();
        }

        private void DadosDefaulGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Locatario");
            dt.Columns.Add("CpfLocatario");
            dt.Columns.Add("Endereco");
            dt.Columns.Add("Locador");
            dt.Columns.Add("Id");

            dgvCadastros.DataSource = dt;
        }

        private void cbTipoPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoPesquisa.SelectedValue.ToString() != "0" && cbTipoPesquisa.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                CarregaDadosGridLocatario();
            }
        }

        private void CarregaDadosGridLocatario()
        {
            DataTable dadosPesquisa = new DataTable();
            if (rbEndereco.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(1, 2, Convert.ToInt32(cbTipoPesquisa.SelectedValue), DateTime.Now, DateTime.Now);
            }
            else if (rbLocador.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(2, 2, Convert.ToInt32(cbTipoPesquisa.SelectedValue), DateTime.Now, DateTime.Now);
            }
            else if (rbLocatario.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(3, 2, Convert.ToInt32(cbTipoPesquisa.SelectedValue), DateTime.Now, DateTime.Now);
            }

            if (dadosPesquisa.Rows.Count > 0)
            {
                dgvCadastros.DataSource = dadosPesquisa;
                dgvCadastros.Visible = true;
            }
            else
            {
                MessageBox.Show("Não existem Locatários cadastrados para essa pesquisa!");
                dgvCadastros.Visible = false;
            }
        }

        private void dgvCadastros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCadastros.Rows.Count > 0 && e.RowIndex != -1)
            {
                int idxBtGerarRecibo = Convert.ToInt32(dgvCadastros.Columns["btGerarRecibo"].Index.ToString());
                int idxBtAlterar = Convert.ToInt32(dgvCadastros.Columns["btAlterar"].Index.ToString());
                int idxIdCadastro = Convert.ToInt32(dgvCadastros.Columns["idCadastro"].Index.ToString());

                string sIdCadastro = dgvCadastros.Rows[e.RowIndex].Cells[idxIdCadastro].Value.ToString().Trim();

                //string teste = dgvCadastros.Rows[e.RowIndex].Cells[ixBtGerarRecibo].Selected.ToString();

                if (dgvCadastros.Rows[e.RowIndex].Cells[idxBtGerarRecibo].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                    {
                        Recibos formRecibos = new Recibos();
                        formRecibos.getIdLocatario = Convert.ToInt32(sIdCadastro);
                        formRecibos.ShowDialog();
                    }
                }
                else if (dgvCadastros.Rows[e.RowIndex].Cells[idxBtAlterar].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                    {
                        Cadastros formCadastros = new Cadastros();
                        formCadastros.getIdLocatario = Convert.ToInt32(sIdCadastro);
                        formCadastros.getIdTipoAcao = 2;
                        tcConsultas.SelectedTab = tpLocatarios;
                        dgvCadastros.Visible = false;
                        ComboDefaultLct(cbTipoPesquisa);
                        formCadastros.ShowDialog();
                    }
                }
            }
        }

        #endregion

        #region Ações aba Recibos Principais

        private void rbRecibosPrincipais_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEnderecoRp.Checked)
            {
                CarregaComboPesquisa(1, cboRecibosPrincipais);
            }
            else if (rbLocadorRP.Checked)
            {
                CarregaComboPesquisa(2, cboRecibosPrincipais);
            }
            else if (rbLocatarioRp.Checked)
            {
                CarregaComboPesquisa(3, cboRecibosPrincipais);
            }
        }

        private void btBuscarRP_Click(object sender, EventArgs e)
        {
            if (cboRecibosPrincipais.Items.Count != 0 && cboRecibosPrincipais.SelectedValue.ToString() != "0" &&
                cboRecibosPrincipais.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                CarregaDadosGridRP(Convert.ToInt32(cboRecibosPrincipais.SelectedValue.ToString()), dtpInicialRP.Value, dtpFinalRP.Value);
            }
            else
            {
                CarregaDadosGridRP(0, dtpInicialRP.Value, dtpFinalRP.Value);
            }
        }

        private void CarregaDadosGridRP(int idComboSelecionado, DateTime dataIncial, DateTime dataFinal)
        {
            DataTable dadosPesquisa = new DataTable();

            if (idComboSelecionado == 0)
            {
                dadosPesquisa = PreencheDadosTabela(0, 4, idComboSelecionado, dataIncial, dataFinal);
            }
            else if (rbEnderecoRp.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(1, 4, idComboSelecionado, dataIncial, dataFinal);
            }
            else if (rbLocadorRP.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(2, 4, idComboSelecionado, dataIncial, dataFinal);
            }
            else if (rbLocatarioRp.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(3, 4, idComboSelecionado, dataIncial, dataFinal);
            }

            if (dadosPesquisa.Rows.Count > 0)
            {
                dgvRecibosPrincipais.DataSource = dadosPesquisa;
            }
            else
            {
                dadosPesquisa = new DataTable();
                dadosPesquisa.Columns.Add("IdRL");
                dadosPesquisa.Columns.Add("IdRP");
                dadosPesquisa.Columns.Add("LocatarioRP");
                dadosPesquisa.Columns.Add("LocadorRP");
                dadosPesquisa.Columns.Add("EnderecoRP");
                dadosPesquisa.Columns.Add("DataReciboRP");

                dgvRecibosPrincipais.DataSource = dadosPesquisa;

                Auxiliar.MostraMensagemAlerta("Não existem Recibos salvos para essa pesquisa!", 2);
            }
        }

        private void btDesmarcaPesquisasRP_Click(object sender, EventArgs e)
        {
            rbLocatarioRp.Checked = false;
            rbLocadorRP.Checked = false;
            rbEnderecoRp.Checked = false;

            DataTable clean = new DataTable();
            cboRecibosPrincipais.DataSource = clean;
            cboRecibosPrincipais.Enabled = false;
        }

        private void dgvRecibosPrincipais_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRecibosPrincipais.Rows.Count > 0 && e.RowIndex != -1)
            {
                int idxbtVisualizarRP = Convert.ToInt32(dgvRecibosPrincipais.Columns["btVisualizarRP"].Index.ToString());
                int idxbtPreviewRP = Convert.ToInt32(dgvRecibosPrincipais.Columns["btPreviewRP"].Index.ToString());
                int idxbtRL = Convert.ToInt32(dgvRecibosPrincipais.Columns["btRL"].Index.ToString());
                int idxIdRecibo = Convert.ToInt32(dgvRecibosPrincipais.Columns["IdRP"].Index.ToString());
                int idxIdRL = Convert.ToInt32(dgvRecibosPrincipais.Columns["IdRL"].Index.ToString());

                string sIdRecibo = dgvRecibosPrincipais.Rows[e.RowIndex].Cells[idxIdRecibo].Value.ToString().Trim();
                string sIdRL = dgvRecibosPrincipais.Rows[e.RowIndex].Cells[idxIdRL].Value.ToString().Trim();

                if (dgvRecibosPrincipais.Rows[e.RowIndex].Cells[idxbtVisualizarRP].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdRecibo) && sIdRecibo != "0")
                    {
                        Auxiliar.VisualizaRecibo(1, Convert.ToInt32(sIdRecibo));
                    }
                }
                else if (dgvRecibosPrincipais.Rows[e.RowIndex].Cells[idxbtPreviewRP].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdRecibo) && sIdRecibo != "0")
                    {
                        Auxiliar.PreviewReciboImpressao(1, Convert.ToInt32(sIdRecibo));
                    }
                }
                else if (dgvRecibosPrincipais.Rows[e.RowIndex].Cells[idxbtRL].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdRL) && sIdRL != "0")
                    {
                        Auxiliar.VisualizaRecibo(2, Convert.ToInt32(sIdRL));
                    }
                    else
                    {
                        Auxiliar.MostraMensagemAlerta("Não foi possivel carregar esse Boleto pois o mesmo pode não ter sido salvo corretamente", 3);
                    }
                }
            }
        }

        #endregion

        #region Ações aba Locadores

        private void CarregaDadosAbaLocadores()
        {
            CarregaComboPesquisa(2, cboLocadores);
        }

        private void cboLocadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLocadores.SelectedValue.ToString() != "0" && cboLocadores.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                DataTable cadastros = PreencheDadosTabela(2, 3, Convert.ToInt32(cboLocadores.SelectedValue.ToString()), DateTime.Now, DateTime.Now);
                if (cadastros.Rows.Count > 0)
                {
                    dgvLocadores.DataSource = cadastros;
                    dgvLocadores.Visible = true;
                }
                else
                {
                    MessageBox.Show("Não existem Locadores cadastrados para essa pesquisa!");
                    dgvLocadores.Visible = false;
                }               
            }
        }

        private void dgvLocadores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLocadores.Rows.Count > 0 && e.RowIndex != -1)
            {
                int idxBtAlterar = Convert.ToInt32(dgvLocadores.Columns["btAlterarLocador"].Index.ToString());
                int idxIdCadastro = Convert.ToInt32(dgvLocadores.Columns["IdLocador"].Index.ToString());

                string sIdCadastro = dgvLocadores.Rows[e.RowIndex].Cells[idxIdCadastro].Value.ToString().Trim();

                if (dgvLocadores.Rows[e.RowIndex].Cells[idxBtAlterar].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                    {
                        Cadastros formCadastros = new Cadastros();
                        formCadastros.getIdLocador = Convert.ToInt32(sIdCadastro);
                        formCadastros.getIdTipoAcao = 2;
                        tcConsultas.SelectedTab = tpLocatarios;
                        dgvLocadores.Visible = false;
                        formCadastros.ShowDialog();
                    }
                }
            }
        }

        #endregion

        #region Ações aba Imóveis

        private void CarregaDadosAbaImoveis()
        {
            CarregaComboPesquisa(1, cboImoveis);
        }

        private void cboImoveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboImoveis.SelectedValue.ToString() != "0" && cboImoveis.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                DataTable cadastros = PreencheDadosTabela(1, 3, Convert.ToInt32(cboImoveis.SelectedValue.ToString()), DateTime.Now, DateTime.Now);               
                if (cadastros.Rows.Count > 0)
                {
                    dgvImoveis.DataSource = cadastros;
                    dgvImoveis.Visible = true;
                }
                else
                {
                    MessageBox.Show("Não existem Imóveis cadastrados para essa pesquisa!");
                    dgvImoveis.Visible = false;
                }
            }
        }

        private void dgvImoveis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvImoveis.Rows.Count > 0 && e.RowIndex != -1)
            {
                int idxBtAlterar = Convert.ToInt32(dgvImoveis.Columns["btAlterarImovel"].Index.ToString());
                int idxIdCadastro = Convert.ToInt32(dgvImoveis.Columns["IdImovel"].Index.ToString());

                string sIdCadastro = dgvImoveis.Rows[e.RowIndex].Cells[idxIdCadastro].Value.ToString().Trim();

                if (dgvImoveis.Rows[e.RowIndex].Cells[idxBtAlterar].Selected)
                {
                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                    {
                        Cadastros formCadastros = new Cadastros();
                        formCadastros.getIdImovel = Convert.ToInt32(sIdCadastro);
                        formCadastros.getIdTipoAcao = 2;
                        dgvImoveis.Visible = false;
                        tcConsultas.SelectedTab = tpLocatarios;
                        formCadastros.ShowDialog();
                    }
                }
            }
        }

        #endregion
    }
}
