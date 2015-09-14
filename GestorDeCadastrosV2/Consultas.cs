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
            InitializeComponent();
        }

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
            else
            {
                cmd.CommandText = "select * from Locatarios where Ativo = 1";
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

        #endregion

        private void rbTipoPesquisa_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEndereco.Checked)
            {
                CarregaComboPesquisa(1);
            }
            else if (rbLocador.Checked)
            {
                CarregaComboPesquisa(2);
            }
            else if (rbLocatario.Checked)
            {
                CarregaComboPesquisa(3);
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

        private void btPesquisar_Click(object sender, EventArgs e)
        {
            if (cbTipoPesquisa.SelectedValue.ToString() != "0")
            {
                CarregaDadosGrid();
            }
        }

        private void cbTipoPesquisa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoPesquisa.SelectedValue.ToString() != "0" && cbTipoPesquisa.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                btPesquisar.Enabled = true;
            }
            else
            {
                btPesquisar.Enabled = false;
            }
        }

        private void CarregaDadosGrid()
        {
            DataTable dadosPesquisa = new DataTable();
            if (rbEndereco.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(1, 2);
            }
            else if (rbLocador.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(2, 2);
            }
            else if (rbLocatario.Checked)
            {
                dadosPesquisa = PreencheDadosTabela(3, 2);
            }

            dgvCadastros.DataSource = dadosPesquisa;
        }

        /// <summary>
        /// TipoPesquisa 1 = Endereço
        /// TipoPesquisa 2 = Locador
        /// TipoPesquisa 3 = Locatário
        /// </summary>
        /// <param name="tipoPesquisa"></param>
        private void CarregaComboPesquisa(int tipoPesquisa)
        {
            DataTable tabelaCombo = PreencheDadosTabela(tipoPesquisa, 1);
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

                cbTipoPesquisa.DataSource = tabelaCombo.DefaultView;
                cbTipoPesquisa.DisplayMember = valorExibicao;
                cbTipoPesquisa.ValueMember = "Id";
                cbTipoPesquisa.Enabled = true;
            }
            else
            {
                cbTipoPesquisa.Enabled = true;
            }
        }

        /// <summary>
        /// tipoCarramento 1 = Carrega Combos
        /// tipoCarramento 2 = Carrega Grid
        /// </summary>
        /// <param name="tipoPesquisa"></param>
        /// <param name="tipoCarramento"></param>
        /// <returns></returns>
        private DataTable PreencheDadosTabela(int tipoPesquisa, int tipoCarramento)
        {
            DataTable tabelaCombos = new DataTable();
            SqlCeDataAdapter sdaSelecao;
            DataSet dsSelecao;

            if (tipoCarramento == 1)
            {
                PreencheDataset(out dsSelecao, out sdaSelecao, tipoPesquisa);
            }
            else
            {
                PreenchePesquisaLocatario(out dsSelecao, out sdaSelecao, tipoPesquisa, cbTipoPesquisa.SelectedValue.ToString());
            }

            tabelaCombos = dsSelecao.Tables[0];

            return tabelaCombos;
        }

        private void dgvCadastros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCadastros.Rows.Count > 0 && e.RowIndex != -1)
            {
                int idxBtGerarRecibo = Convert.ToInt32(dgvCadastros.Columns["btGerarRecibo"].Index.ToString());
                int idxIdCadastro = Convert.ToInt32(dgvCadastros.Columns["idCadastro"].Index.ToString());

                //string teste = dgvCadastros.Rows[e.RowIndex].Cells[ixBtGerarRecibo].Selected.ToString();

                if (dgvCadastros.Rows[e.RowIndex].Cells[idxBtGerarRecibo].Selected)
                {
                    string sIdCadastro = dgvCadastros.Rows[e.RowIndex].Cells[idxIdCadastro].Value.ToString().Trim();

                    if (!string.IsNullOrEmpty(sIdCadastro) && sIdCadastro != "0")
                    {
                        Recibos formRecibos = new Recibos();
                        formRecibos.getIdLocatario = Convert.ToInt32(sIdCadastro);
                        this.Hide();
                        formRecibos.ShowDialog();
                        this.Close();
                    }
                }
            }
        }

        private void btInicio_Click(object sender, EventArgs e)
        {
            Inicio formInicio = new Inicio();
            this.Hide();
            formInicio.ShowDialog();
            this.Close();
        }
    }
}
