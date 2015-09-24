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

            Auxiliar.CentralizaControle(tcConsultas, this);
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
            else if (tcConsultas.SelectedTab == tpRcbLocadores)
            {
                CarregaDadosAba(5);
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

        private void CarregaDadosAba(int tipoCadastro)
        {
            if (tipoCadastro == 4)
            {

            }
            else if (tipoCadastro == 5)
            {
                CarregaComboPesquisa(2, cboRecibosLocador);
                dtpInicialRL.Value = DateTime.Now;
                dtpInicialRL.Value = DateTime.Now;
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
            Login formInicio = new Login();
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
            else if (sender == dtpInicialRL)
            {
                defineDataFinal(dtpInicialRL, dtpFinalRL);
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
                cmd.CommandText = "select * from Imoveis where Ativo = 1 and Id = " + idCadastro + "";
            }
            else if (tipoTabela == 2)
            {
                cmd.CommandText = "select * from Locadores where Ativo = 1 and Id = " + idCadastro + "";
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
                cmd.CommandText = "select Lc.Id, Lc.Locatario, Lc.CpfLocatario, Im.Endereco, Ld.Locador  from Locatarios Lc inner join Imoveis Im " +
                    "on Lc.fkIdImovel = Im.Id" +
                    " inner join Locadores Ld on Lc.fkIdLocador = Ld.Id" +
                    " where Lc.Ativo = 1 and Im.Ativo = 1 and Lc.fkIdImovel = " + valorTabela + "";
            }
            else if (tipoPesquisa == 2)
            {
                cmd.CommandText = "select Lc.Id, Lc.Locatario, Lc.CpfLocatario, Im.Endereco, Ld.Locador  from Locatarios Lc inner join Imoveis Im " +
                    "on Lc.fkIdImovel = Im.Id" +
                    " inner join Locadores Ld on Lc.fkIdLocador = Ld.Id" +
                    " where Lc.Ativo = 1 and Im.Ativo = 1 and Ld.Id = " + valorTabela + "";
            }
            else
            {
                cmd.CommandText = "select Lc.Id, Lc.Locatario, Lc.CpfLocatario, Im.Endereco, Ld.Locador  from Locatarios Lc inner join Imoveis Im " +
                    "on Lc.fkIdImovel = Im.Id" +
                    " inner join Locadores Ld on Lc.fkIdLocador = Ld.Id" +
                    " where Lc.Ativo = 1 and Im.Ativo = 1 and Lc.Id = " + valorTabela + "";
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


                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP, rp.Data as DataReciboRP" +
                                                " from RecibosPrincipais rp" +
                                                " inner join Locatarios lct" +
                                                " on lct.Id = rp.fkIdLocatario " +
                                                " inner join Imoveis im" +
                                                " on lct.fkIdImovel = im.Id " +
                                                " inner join Locadores lcds" +
                                                " on lct.fkIdLocador = lcds.Id " +
                                                comandoData + " order by rp.Data desc";
            }
            else if (tipoPesquisa == 1)
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP, rp.Data as DataReciboRP" +
                                                " from RecibosPrincipais rp" +
                                                " inner join Locatarios lct" +
                                                " on lct.Id = rp.fkIdLocatario " +
                                                " inner join Imoveis im" +
                                                " on lct.fkIdImovel = im.Id " +
                                                " inner join Locadores lcds" +
                                                " on lct.fkIdLocador = lcds.Id " +
                                                comandoData + " and im.Id = " + idCadastro + " order by rp.Data desc";
            }
            else if (tipoPesquisa == 2)
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP, rp.Data as DataReciboRP" +
                                " from RecibosPrincipais rp" +
                                " inner join Locatarios lct" +
                                " on lct.Id = rp.fkIdLocatario " +
                                " inner join Imoveis im" +
                                " on lct.fkIdImovel = im.Id " +
                                " inner join Locadores lcds" +
                                " on lct.fkIdLocador = lcds.Id " +
                                comandoData + " and lcds.Id = " + idCadastro + " order by rp.Data desc";
            }
            else
            {
                cmd.CommandText = "select rp.Id as IdRP, lct.Locatario as LocatarioRP, lcds.Locador as LocadorRP, im.Endereco as EnderecoRP, rp.Data as DataReciboRP" +
                                  " from RecibosPrincipais rp" +
                                  " inner join Locatarios lct" +
                                  " on lct.Id = rp.fkIdLocatario " +
                                  " inner join Imoveis im" +
                                  " on lct.fkIdImovel = im.Id " +
                                  " inner join Locadores lcds" +
                                  " on lct.fkIdLocador = lcds.Id " +
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
        /// <param name="dsDados"></param>
        /// <param name="sdaDados"></param>
        /// <param name="tipoPesquisa"></param>
        /// <param name="idCadastro"></param>
        private static void PreenchePesquisaRecibosLocadores(out DataSet dsDados, out SqlCeDataAdapter sdaDados, int tipoPesquisa, int idCadastro,
                                                             DateTime dataIncial, DateTime dataFinal)
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
                cmd.CommandText = "select rl.Id as IdRL, lcd.Locador as LocadorRL, lct.Locatario as LocatarioRL, im.Endereco as EnderecoRL, rp.Data as DataReciboRL" +
                              " from RecibosLocadores rl" +
                              " inner join RecibosPrincipais rp" +
                              " on rl.fkIdRecibo = rp.Id" +
                              " inner join Locatarios lct" +
                              " on rp.fkIdLocatario = lct.Id" +
                              " inner join Locadores lcd" +
                              " on lct.fkIdLocador = lcd.Id" +
                              " inner join Imoveis im" +
                              " on lct.fkIdImovel = im.Id" +
                              comandoData + " order by rp.Data";
            }
            else
            {
                cmd.CommandText = "select rl.Id as IdRL, lcd.Locador as LocadorRL, lct.Locatario as LocatarioRL, im.Endereco as EnderecoRL, rp.Data as DataReciboRL" +
                                              " from RecibosLocadores rl" +
                                              " inner join RecibosPrincipais rp" +
                                              " on rl.fkIdRecibo = rp.Id" +
                                              " inner join Locatarios lct" +
                                              " on rp.fkIdLocatario = lct.Id" +
                                              " inner join Locadores lcd" +
                                              " on lct.fkIdLocador = lcd.Id" +
                                              " inner join Imoveis im" +
                                              " on lct.fkIdImovel = im.Id" +
                                              comandoData + " and rl.Id = " + idCadastro + " order by rp.Data";
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
                DataTable dtSemDados = new DataTable();

                dtSemDados.Columns.Add("Descricao");
                dtSemDados.Columns.Add("Id");

                dtSemDados.Rows.Add("Faltam dados cadastrados para exibir", 0);

                cbPesquisa.DataSource = dtSemDados;
                cbPesquisa.DisplayMember = "Descricao";
                cbPesquisa.ValueMember = "Id";

                cbPesquisa.Enabled = false;
            }
        }

        /// <summary>
        /// tipoCarramento 1 = Carrega Combos
        /// tipoCarramento 2 = Carrega Grid Aba Locatarios
        /// tipoCarramento 3 = Carrega Grid Aba Locadores e Imovéis
        /// tipoPesquisa 1 = Endereço
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
            else if (tipoCarramento == 5)
            {
                PreenchePesquisaRecibosLocadores(out dsSelecao, out sdaSelecao, tipoPesquisa, idCadastro, dataIncial, dataFinal);
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
            }
            else
            {
                MessageBox.Show("Não existem Locatários cadastrados para essa pesquisa!");
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

        #endregion

        #region Ações aba Recibos Locador

        private void btBuscarRL_Click(object sender, EventArgs e)
        {
            if (cboRecibosLocador.Items.Count != 0 && cboRecibosLocador.SelectedValue.ToString() != "0" &&
               cboRecibosLocador.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                CarregaDadosGridRL(Convert.ToInt32(cboRecibosLocador.SelectedValue.ToString()), dtpInicialRL.Value, dtpFinalRL.Value);
            }
            else
            {
                CarregaDadosGridRL(0, dtpInicialRL.Value, dtpFinalRL.Value);
            }
        }

        private void CarregaDadosGridRL(int idComboSelecionado, DateTime dataIncial, DateTime dataFinal)
        {
            DataTable dadosPesquisa = new DataTable();

            if (idComboSelecionado == 0)
            {
                dadosPesquisa = PreencheDadosTabela(0, 5, idComboSelecionado, dataIncial, dataFinal);
            }
            else
            {
                dadosPesquisa = PreencheDadosTabela(2, 5, idComboSelecionado, dataIncial, dataFinal);
            }

            if (dadosPesquisa.Rows.Count > 0)
            {
                dgvRecibosLocador.DataSource = dadosPesquisa;
            }
            else
            {
                dadosPesquisa = new DataTable();
                dadosPesquisa.Columns.Add("IdRL");
                dadosPesquisa.Columns.Add("LocadorRL");
                dadosPesquisa.Columns.Add("LocatarioRL");                
                dadosPesquisa.Columns.Add("EnderecoRL");
                dadosPesquisa.Columns.Add("DataReciboRL");

                dgvRecibosLocador.DataSource = dadosPesquisa;

                Auxiliar.MostraMensagemAlerta("Não existem Recibos salvos para essa pesquisa!", 2);
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
                dgvLocadores.DataSource = cadastros;
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
                dgvImoveis.DataSource = cadastros;
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
                        formCadastros.ShowDialog();
                    }
                }
            }
        }

        #endregion

    }
}
