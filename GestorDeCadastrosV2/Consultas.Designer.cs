namespace GestorDeCadastros
{
    partial class Consultas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tpLocatarios = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbTipoPesquisa = new System.Windows.Forms.ComboBox();
            this.btPesquisar = new System.Windows.Forms.Button();
            this.rbLocador = new System.Windows.Forms.RadioButton();
            this.rbEndereco = new System.Windows.Forms.RadioButton();
            this.rbLocatario = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.dgvCadastros = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRecibosP = new System.Windows.Forms.TabPage();
            this.tpRecibosL = new System.Windows.Forms.TabPage();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.Locatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CpfLocatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnderecoImovel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Locador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btGerarRecibo = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btAlterar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.idCadastro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpLocatarios.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCadastros)).BeginInit();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tpLocatarios
            // 
            this.tpLocatarios.BackColor = System.Drawing.Color.DimGray;
            this.tpLocatarios.Controls.Add(this.panel2);
            this.tpLocatarios.Controls.Add(this.dgvCadastros);
            this.tpLocatarios.Location = new System.Drawing.Point(4, 26);
            this.tpLocatarios.Name = "tpLocatarios";
            this.tpLocatarios.Padding = new System.Windows.Forms.Padding(3);
            this.tpLocatarios.Size = new System.Drawing.Size(974, 470);
            this.tpLocatarios.TabIndex = 1;
            this.tpLocatarios.Text = "Locatários";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.cbTipoPesquisa);
            this.panel2.Controls.Add(this.btPesquisar);
            this.panel2.Controls.Add(this.rbLocador);
            this.panel2.Controls.Add(this.rbEndereco);
            this.panel2.Controls.Add(this.rbLocatario);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Location = new System.Drawing.Point(3, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(965, 53);
            this.panel2.TabIndex = 1;
            // 
            // cbTipoPesquisa
            // 
            this.cbTipoPesquisa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipoPesquisa.Enabled = false;
            this.cbTipoPesquisa.FormattingEnabled = true;
            this.cbTipoPesquisa.Location = new System.Drawing.Point(397, 14);
            this.cbTipoPesquisa.Name = "cbTipoPesquisa";
            this.cbTipoPesquisa.Size = new System.Drawing.Size(313, 25);
            this.cbTipoPesquisa.TabIndex = 6;
            this.cbTipoPesquisa.SelectedIndexChanged += new System.EventHandler(this.cbTipoPesquisa_SelectedIndexChanged);
            // 
            // btPesquisar
            // 
            this.btPesquisar.Enabled = false;
            this.btPesquisar.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPesquisar.Location = new System.Drawing.Point(732, 13);
            this.btPesquisar.Name = "btPesquisar";
            this.btPesquisar.Size = new System.Drawing.Size(48, 27);
            this.btPesquisar.TabIndex = 5;
            this.btPesquisar.Text = "OK";
            this.btPesquisar.UseVisualStyleBackColor = true;
            this.btPesquisar.Click += new System.EventHandler(this.btPesquisar_Click);
            // 
            // rbLocador
            // 
            this.rbLocador.AutoSize = true;
            this.rbLocador.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLocador.Location = new System.Drawing.Point(207, 17);
            this.rbLocador.Name = "rbLocador";
            this.rbLocador.Size = new System.Drawing.Size(78, 20);
            this.rbLocador.TabIndex = 3;
            this.rbLocador.TabStop = true;
            this.rbLocador.Text = "Locador";
            this.rbLocador.UseVisualStyleBackColor = true;
            this.rbLocador.CheckedChanged += new System.EventHandler(this.rbTipoPesquisa_CheckedChanged);
            // 
            // rbEndereco
            // 
            this.rbEndereco.AutoSize = true;
            this.rbEndereco.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbEndereco.Location = new System.Drawing.Point(291, 16);
            this.rbEndereco.Name = "rbEndereco";
            this.rbEndereco.Size = new System.Drawing.Size(86, 20);
            this.rbEndereco.TabIndex = 2;
            this.rbEndereco.TabStop = true;
            this.rbEndereco.Text = "Endereço";
            this.rbEndereco.UseVisualStyleBackColor = true;
            this.rbEndereco.CheckedChanged += new System.EventHandler(this.rbTipoPesquisa_CheckedChanged);
            // 
            // rbLocatario
            // 
            this.rbLocatario.AutoSize = true;
            this.rbLocatario.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLocatario.Location = new System.Drawing.Point(115, 17);
            this.rbLocatario.Name = "rbLocatario";
            this.rbLocatario.Size = new System.Drawing.Size(86, 20);
            this.rbLocatario.TabIndex = 1;
            this.rbLocatario.TabStop = true;
            this.rbLocatario.Text = "Locatário";
            this.rbLocatario.UseVisualStyleBackColor = true;
            this.rbLocatario.CheckedChanged += new System.EventHandler(this.rbTipoPesquisa_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 18);
            this.label12.TabIndex = 0;
            this.label12.Text = "Pesquisar por:";
            // 
            // dgvCadastros
            // 
            this.dgvCadastros.AllowUserToAddRows = false;
            this.dgvCadastros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCadastros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Locatario,
            this.CpfLocatario,
            this.EnderecoImovel,
            this.Locador,
            this.btGerarRecibo,
            this.btAlterar,
            this.idCadastro});
            this.dgvCadastros.Location = new System.Drawing.Point(2, 67);
            this.dgvCadastros.Name = "dgvCadastros";
            this.dgvCadastros.ReadOnly = true;
            this.dgvCadastros.Size = new System.Drawing.Size(966, 397);
            this.dgvCadastros.TabIndex = 0;
            this.dgvCadastros.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCadastros_CellContentClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpLocatarios);
            this.tabControl1.Controls.Add(this.tpRecibosP);
            this.tabControl1.Controls.Add(this.tpRecibosL);
            this.tabControl1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(8, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(982, 500);
            this.tabControl1.TabIndex = 1;
            // 
            // tpRecibosP
            // 
            this.tpRecibosP.Location = new System.Drawing.Point(4, 26);
            this.tpRecibosP.Name = "tpRecibosP";
            this.tpRecibosP.Size = new System.Drawing.Size(974, 470);
            this.tpRecibosP.TabIndex = 2;
            this.tpRecibosP.Text = "Recibos Principais";
            this.tpRecibosP.UseVisualStyleBackColor = true;
            // 
            // tpRecibosL
            // 
            this.tpRecibosL.Location = new System.Drawing.Point(4, 26);
            this.tpRecibosL.Name = "tpRecibosL";
            this.tpRecibosL.Size = new System.Drawing.Size(974, 470);
            this.tpRecibosL.TabIndex = 3;
            this.tpRecibosL.Text = "Recibos Locador";
            this.tpRecibosL.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Locatario
            // 
            this.Locatario.DataPropertyName = "Locatario";
            this.Locatario.HeaderText = "Locatário";
            this.Locatario.Name = "Locatario";
            this.Locatario.ReadOnly = true;
            this.Locatario.Width = 200;
            // 
            // CpfLocatario
            // 
            this.CpfLocatario.DataPropertyName = "CpfLocatario";
            this.CpfLocatario.HeaderText = "CPF";
            this.CpfLocatario.Name = "CpfLocatario";
            this.CpfLocatario.ReadOnly = true;
            this.CpfLocatario.Width = 130;
            // 
            // EnderecoImovel
            // 
            this.EnderecoImovel.DataPropertyName = "Endereco";
            this.EnderecoImovel.HeaderText = "Endereço Imóvel";
            this.EnderecoImovel.Name = "EnderecoImovel";
            this.EnderecoImovel.ReadOnly = true;
            this.EnderecoImovel.Width = 150;
            // 
            // Locador
            // 
            this.Locador.DataPropertyName = "Locador";
            this.Locador.HeaderText = "Locador";
            this.Locador.Name = "Locador";
            this.Locador.ReadOnly = true;
            this.Locador.Width = 150;
            // 
            // btGerarRecibo
            // 
            this.btGerarRecibo.HeaderText = "Gerar Recibo";
            this.btGerarRecibo.Name = "btGerarRecibo";
            this.btGerarRecibo.ReadOnly = true;
            this.btGerarRecibo.Text = "Ok";
            this.btGerarRecibo.UseColumnTextForButtonValue = true;
            this.btGerarRecibo.Width = 150;
            // 
            // btAlterar
            // 
            this.btAlterar.HeaderText = "Alterar";
            this.btAlterar.Name = "btAlterar";
            this.btAlterar.ReadOnly = true;
            this.btAlterar.Text = "Ok";
            this.btAlterar.UseColumnTextForButtonValue = true;
            // 
            // idCadastro
            // 
            this.idCadastro.DataPropertyName = "Id";
            this.idCadastro.HeaderText = "IdCadastro";
            this.idCadastro.Name = "idCadastro";
            this.idCadastro.ReadOnly = true;
            this.idCadastro.Visible = false;
            // 
            // Consultas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 561);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Consultas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consultas";
            this.tpLocatarios.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCadastros)).EndInit();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tpLocatarios;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbTipoPesquisa;
        private System.Windows.Forms.Button btPesquisar;
        private System.Windows.Forms.RadioButton rbLocador;
        private System.Windows.Forms.RadioButton rbEndereco;
        private System.Windows.Forms.RadioButton rbLocatario;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgvCadastros;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpRecibosP;
        private System.Windows.Forms.TabPage tpRecibosL;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Locatario;
        private System.Windows.Forms.DataGridViewTextBoxColumn CpfLocatario;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnderecoImovel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Locador;
        private System.Windows.Forms.DataGridViewButtonColumn btGerarRecibo;
        private System.Windows.Forms.DataGridViewButtonColumn btAlterar;
        private System.Windows.Forms.DataGridViewTextBoxColumn idCadastro;


    }
}