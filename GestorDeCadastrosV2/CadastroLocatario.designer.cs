namespace GestorDeCadastros_V2
{
    partial class CadastroLocatario
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCadastro = new System.Windows.Forms.TabPage();
            this.btCadastrar = new System.Windows.Forms.Button();
            this.btLimparFormCadastro = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.mtxtAluguel = new System.Windows.Forms.MaskedTextBox();
            this.mtxtCpfLocador = new System.Windows.Forms.MaskedTextBox();
            this.mtxtCpfLocatario = new System.Windows.Forms.MaskedTextBox();
            this.mtxtCep = new System.Windows.Forms.MaskedTextBox();
            this.mtxtTelefone = new System.Windows.Forms.MaskedTextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNomeLocador = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtCidade = new System.Windows.Forms.TextBox();
            this.txtEnderecoImovel = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNomeLocatario = new System.Windows.Forms.TextBox();
            this.tpConsulta = new System.Windows.Forms.TabPage();
            this.dgvCadastros = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cadastrosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gestorCadastrosDataSet1 = new GestorDeCadastros.GestorCadastrosDataSet1();
            this.cadastrosTableAdapter = new GestorDeCadastros.GestorCadastrosDataSet1TableAdapters.CadastrosTableAdapter();
            this.btAtualizar = new System.Windows.Forms.Button();
            this.btExcluir = new System.Windows.Forms.Button();
            this.lblIdCadastro = new System.Windows.Forms.Label();
            this.btCadastrado = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Locatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CpfLocatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnderecoImovel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Telefone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Locador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aluguel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idCadastro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tpCadastro.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCadastros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cadastrosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gestorCadastrosDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpConsulta);
            this.tabControl1.Controls.Add(this.tpCadastro);
            this.tabControl1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(8, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(865, 500);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpCadastro
            // 
            this.tpCadastro.BackColor = System.Drawing.Color.DimGray;
            this.tpCadastro.Controls.Add(this.btExcluir);
            this.tpCadastro.Controls.Add(this.btAtualizar);
            this.tpCadastro.Controls.Add(this.btCadastrar);
            this.tpCadastro.Controls.Add(this.btLimparFormCadastro);
            this.tpCadastro.Controls.Add(this.panel1);
            this.tpCadastro.Location = new System.Drawing.Point(4, 26);
            this.tpCadastro.Name = "tpCadastro";
            this.tpCadastro.Padding = new System.Windows.Forms.Padding(3);
            this.tpCadastro.Size = new System.Drawing.Size(857, 470);
            this.tpCadastro.TabIndex = 0;
            this.tpCadastro.Text = "Cadastro";
            // 
            // btCadastrar
            // 
            this.btCadastrar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCadastrar.Location = new System.Drawing.Point(678, 420);
            this.btCadastrar.Name = "btCadastrar";
            this.btCadastrar.Size = new System.Drawing.Size(79, 35);
            this.btCadastrar.TabIndex = 2;
            this.btCadastrar.Text = "Cadastrar";
            this.btCadastrar.UseVisualStyleBackColor = true;
            this.btCadastrar.Click += new System.EventHandler(this.btCadastrar_Click);
            // 
            // btLimparFormCadastro
            // 
            this.btLimparFormCadastro.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLimparFormCadastro.Location = new System.Drawing.Point(521, 420);
            this.btLimparFormCadastro.Name = "btLimparFormCadastro";
            this.btLimparFormCadastro.Size = new System.Drawing.Size(139, 35);
            this.btLimparFormCadastro.TabIndex = 1;
            this.btLimparFormCadastro.Text = "Limpar Formulário";
            this.btLimparFormCadastro.UseVisualStyleBackColor = true;
            this.btLimparFormCadastro.Click += new System.EventHandler(this.btLimpar_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblIdCadastro);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.mtxtAluguel);
            this.panel1.Controls.Add(this.mtxtCpfLocador);
            this.panel1.Controls.Add(this.mtxtCpfLocatario);
            this.panel1.Controls.Add(this.mtxtCep);
            this.panel1.Controls.Add(this.mtxtTelefone);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txtNomeLocador);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.txtCidade);
            this.panel1.Controls.Add(this.txtEnderecoImovel);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtNomeLocatario);
            this.panel1.Location = new System.Drawing.Point(49, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 389);
            this.panel1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(472, 118);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 18);
            this.label11.TabIndex = 46;
            this.label11.Text = "R$:";
            // 
            // mtxtAluguel
            // 
            this.mtxtAluguel.Location = new System.Drawing.Point(513, 116);
            this.mtxtAluguel.Mask = "99,999.99";
            this.mtxtAluguel.Name = "mtxtAluguel";
            this.mtxtAluguel.Size = new System.Drawing.Size(76, 25);
            this.mtxtAluguel.TabIndex = 45;
            this.mtxtAluguel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtxtCpfLocador
            // 
            this.mtxtCpfLocador.Location = new System.Drawing.Point(475, 79);
            this.mtxtCpfLocador.Mask = "999,999,999-90";
            this.mtxtCpfLocador.Name = "mtxtCpfLocador";
            this.mtxtCpfLocador.Size = new System.Drawing.Size(114, 25);
            this.mtxtCpfLocador.TabIndex = 44;
            this.mtxtCpfLocador.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtxtCpfLocatario
            // 
            this.mtxtCpfLocatario.Location = new System.Drawing.Point(80, 79);
            this.mtxtCpfLocatario.Mask = "999,999,999-90";
            this.mtxtCpfLocatario.Name = "mtxtCpfLocatario";
            this.mtxtCpfLocatario.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mtxtCpfLocatario.Size = new System.Drawing.Size(112, 25);
            this.mtxtCpfLocatario.TabIndex = 43;
            this.mtxtCpfLocatario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtxtCep
            // 
            this.mtxtCep.Location = new System.Drawing.Point(80, 225);
            this.mtxtCep.Mask = "99999-999";
            this.mtxtCep.Name = "mtxtCep";
            this.mtxtCep.Size = new System.Drawing.Size(78, 25);
            this.mtxtCep.TabIndex = 42;
            this.mtxtCep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtxtTelefone
            // 
            this.mtxtTelefone.Location = new System.Drawing.Point(91, 263);
            this.mtxtTelefone.Mask = "(99) 09999-9999";
            this.mtxtTelefone.Name = "mtxtTelefone";
            this.mtxtTelefone.Size = new System.Drawing.Size(124, 25);
            this.mtxtTelefone.TabIndex = 41;
            this.mtxtTelefone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Location = new System.Drawing.Point(21, 358);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(155, 18);
            this.label27.TabIndex = 40;
            this.label27.Text = "Campos obrigatórios";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(8, 358);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 18);
            this.label26.TabIndex = 39;
            this.label26.Text = "*";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(455, 79);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(14, 17);
            this.label25.TabIndex = 38;
            this.label25.Text = "*";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Location = new System.Drawing.Point(420, 19);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(14, 17);
            this.label24.TabIndex = 37;
            this.label24.Text = "*";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(417, 116);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(14, 17);
            this.label21.TabIndex = 34;
            this.label21.Text = "*";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(78, 264);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 17);
            this.label20.TabIndex = 33;
            this.label20.Text = "*";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(47, 224);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 17);
            this.label19.TabIndex = 32;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(67, 189);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(14, 17);
            this.label18.TabIndex = 31;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(136, 116);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 17);
            this.label17.TabIndex = 30;
            this.label17.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(45, 79);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 17);
            this.label16.TabIndex = 29;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(57, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 17);
            this.label15.TabIndex = 28;
            this.label15.Text = "*";
            // 
            // txtNomeLocador
            // 
            this.txtNomeLocador.Location = new System.Drawing.Point(357, 44);
            this.txtNomeLocador.Name = "txtNomeLocador";
            this.txtNomeLocador.Size = new System.Drawing.Size(300, 25);
            this.txtNomeLocador.TabIndex = 21;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(80, 301);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(232, 25);
            this.txtEmail.TabIndex = 20;
            // 
            // txtCidade
            // 
            this.txtCidade.Location = new System.Drawing.Point(80, 189);
            this.txtCidade.Name = "txtCidade";
            this.txtCidade.Size = new System.Drawing.Size(172, 25);
            this.txtCidade.TabIndex = 17;
            // 
            // txtEnderecoImovel
            // 
            this.txtEnderecoImovel.Location = new System.Drawing.Point(14, 148);
            this.txtEnderecoImovel.Name = "txtEnderecoImovel";
            this.txtEnderecoImovel.Size = new System.Drawing.Size(300, 25);
            this.txtEnderecoImovel.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(355, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 18);
            this.label10.TabIndex = 10;
            this.label10.Text = "Aluguel: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(354, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 18);
            this.label9.TabIndex = 9;
            this.label9.Text = "CPF Locador:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(354, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 18);
            this.label8.TabIndex = 8;
            this.label8.Text = "Locador;";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 18);
            this.label7.TabIndex = 7;
            this.label7.Text = "E-mail:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 266);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "Telefone:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "Cep:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Cidade:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Endereço Imóvel;";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "CPF:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nome;";
            // 
            // txtNomeLocatario
            // 
            this.txtNomeLocatario.ForeColor = System.Drawing.Color.Black;
            this.txtNomeLocatario.Location = new System.Drawing.Point(14, 44);
            this.txtNomeLocatario.Name = "txtNomeLocatario";
            this.txtNomeLocatario.Size = new System.Drawing.Size(300, 25);
            this.txtNomeLocatario.TabIndex = 0;
            // 
            // tpConsulta
            // 
            this.tpConsulta.BackColor = System.Drawing.Color.DimGray;
            this.tpConsulta.Controls.Add(this.panel2);
            this.tpConsulta.Controls.Add(this.dgvCadastros);
            this.tpConsulta.Location = new System.Drawing.Point(4, 26);
            this.tpConsulta.Name = "tpConsulta";
            this.tpConsulta.Padding = new System.Windows.Forms.Padding(3);
            this.tpConsulta.Size = new System.Drawing.Size(857, 470);
            this.tpConsulta.TabIndex = 1;
            this.tpConsulta.Text = "Consulta";
            // 
            // dgvCadastros
            // 
            this.dgvCadastros.AllowUserToAddRows = false;
            this.dgvCadastros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCadastros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btCadastrado,
            this.Locatario,
            this.CpfLocatario,
            this.EnderecoImovel,
            this.Cidade,
            this.Telefone,
            this.Locador,
            this.Aluguel,
            this.idCadastro});
            this.dgvCadastros.Location = new System.Drawing.Point(2, 67);
            this.dgvCadastros.Name = "dgvCadastros";
            this.dgvCadastros.Size = new System.Drawing.Size(849, 397);
            this.dgvCadastros.TabIndex = 0;
            this.dgvCadastros.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCadastros_CellContentClick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Location = new System.Drawing.Point(3, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(795, 53);
            this.panel2.TabIndex = 1;
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
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(115, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(86, 20);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Locatário";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(207, 16);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(86, 20);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Endereço";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3.Location = new System.Drawing.Point(300, 17);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(78, 20);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Locador";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(383, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(343, 25);
            this.textBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(732, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cadastrosBindingSource
            // 
            this.cadastrosBindingSource.DataMember = "Cadastros";
            this.cadastrosBindingSource.DataSource = this.gestorCadastrosDataSet1;
            // 
            // gestorCadastrosDataSet1
            // 
            this.gestorCadastrosDataSet1.DataSetName = "GestorCadastrosDataSet1";
            this.gestorCadastrosDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cadastrosTableAdapter
            // 
            this.cadastrosTableAdapter.ClearBeforeFill = true;
            // 
            // btAtualizar
            // 
            this.btAtualizar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAtualizar.Location = new System.Drawing.Point(88, 420);
            this.btAtualizar.Name = "btAtualizar";
            this.btAtualizar.Size = new System.Drawing.Size(79, 35);
            this.btAtualizar.TabIndex = 3;
            this.btAtualizar.Text = "Atualizar";
            this.btAtualizar.UseVisualStyleBackColor = true;
            this.btAtualizar.Visible = false;
            this.btAtualizar.Click += new System.EventHandler(this.btAtualizar_Click);
            // 
            // btExcluir
            // 
            this.btExcluir.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExcluir.Location = new System.Drawing.Point(186, 420);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(79, 35);
            this.btExcluir.TabIndex = 4;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Visible = false;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // lblIdCadastro
            // 
            this.lblIdCadastro.AutoSize = true;
            this.lblIdCadastro.Location = new System.Drawing.Point(104, 18);
            this.lblIdCadastro.Name = "lblIdCadastro";
            this.lblIdCadastro.Size = new System.Drawing.Size(93, 17);
            this.lblIdCadastro.TabIndex = 47;
            this.lblIdCadastro.Text = "lblIdCadastro";
            this.lblIdCadastro.Visible = false;
            // 
            // btCadastrado
            // 
            this.btCadastrado.HeaderText = "";
            this.btCadastrado.Name = "btCadastrado";
            this.btCadastrado.Text = "Selecionar";
            this.btCadastrado.UseColumnTextForButtonValue = true;
            // 
            // Locatario
            // 
            this.Locatario.DataPropertyName = "Locatario";
            this.Locatario.HeaderText = "Locatário";
            this.Locatario.Name = "Locatario";
            // 
            // CpfLocatario
            // 
            this.CpfLocatario.DataPropertyName = "CpfLocatario";
            this.CpfLocatario.HeaderText = "CPF Locatário";
            this.CpfLocatario.Name = "CpfLocatario";
            // 
            // EnderecoImovel
            // 
            this.EnderecoImovel.DataPropertyName = "EnderecoImovel";
            this.EnderecoImovel.HeaderText = "Endereço Imóvel";
            this.EnderecoImovel.Name = "EnderecoImovel";
            // 
            // Cidade
            // 
            this.Cidade.DataPropertyName = "Cidade";
            this.Cidade.HeaderText = "Cidade";
            this.Cidade.Name = "Cidade";
            // 
            // Telefone
            // 
            this.Telefone.DataPropertyName = "Telefone";
            this.Telefone.HeaderText = "Telefone";
            this.Telefone.Name = "Telefone";
            // 
            // Locador
            // 
            this.Locador.DataPropertyName = "Locador";
            this.Locador.HeaderText = "Locador";
            this.Locador.Name = "Locador";
            // 
            // Aluguel
            // 
            this.Aluguel.DataPropertyName = "Aluguel";
            this.Aluguel.HeaderText = "Aluguel";
            this.Aluguel.Name = "Aluguel";
            // 
            // idCadastro
            // 
            this.idCadastro.DataPropertyName = "Id";
            this.idCadastro.HeaderText = "IdCadastro";
            this.idCadastro.Name = "idCadastro";
            this.idCadastro.ReadOnly = true;
            this.idCadastro.Visible = false;
            // 
            // CadastroLocatario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 569);
            this.Controls.Add(this.tabControl1);
            this.Name = "CadastroLocatario";
            this.Text = "Gestor de Cadastros";      
            this.tabControl1.ResumeLayout(false);
            this.tpCadastro.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpConsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCadastros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cadastrosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gestorCadastrosDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCadastro;
        private System.Windows.Forms.Button btCadastrar;
        private System.Windows.Forms.Button btLimparFormCadastro;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNomeLocador;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtCidade;
        private System.Windows.Forms.TextBox txtEnderecoImovel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNomeLocatario;
        private System.Windows.Forms.TabPage tpConsulta;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.MaskedTextBox mtxtTelefone;
        private System.Windows.Forms.MaskedTextBox mtxtCep;
        private System.Windows.Forms.MaskedTextBox mtxtCpfLocador;
        private System.Windows.Forms.MaskedTextBox mtxtCpfLocatario;
        private System.Windows.Forms.MaskedTextBox mtxtAluguel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvCadastros;
        private GestorDeCadastros.GestorCadastrosDataSet1 gestorCadastrosDataSet1;
        private System.Windows.Forms.BindingSource cadastrosBindingSource;
        private GestorDeCadastros.GestorCadastrosDataSet1TableAdapters.CadastrosTableAdapter cadastrosTableAdapter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.Button btAtualizar;
        private System.Windows.Forms.Label lblIdCadastro;
        private System.Windows.Forms.DataGridViewButtonColumn btCadastrado;
        private System.Windows.Forms.DataGridViewTextBoxColumn Locatario;
        private System.Windows.Forms.DataGridViewTextBoxColumn CpfLocatario;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnderecoImovel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Telefone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Locador;
        private System.Windows.Forms.DataGridViewTextBoxColumn Aluguel;
        private System.Windows.Forms.DataGridViewTextBoxColumn idCadastro;
    }
}

