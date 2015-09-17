namespace GestorDeCadastros
{
    partial class InsereValores
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
            this.txtPorExtenso = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btInserir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btCancelar = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInsercao = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPorExtenso
            // 
            this.txtPorExtenso.Enabled = false;
            this.txtPorExtenso.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPorExtenso.Location = new System.Drawing.Point(42, 141);
            this.txtPorExtenso.Multiline = true;
            this.txtPorExtenso.Name = "txtPorExtenso";
            this.txtPorExtenso.Size = new System.Drawing.Size(487, 96);
            this.txtPorExtenso.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(173, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "R$";
            // 
            // btInserir
            // 
            this.btInserir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btInserir.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btInserir.Enabled = false;
            this.btInserir.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInserir.Location = new System.Drawing.Point(312, 265);
            this.btInserir.Name = "btInserir";
            this.btInserir.Size = new System.Drawing.Size(100, 40);
            this.btInserir.TabIndex = 3;
            this.btInserir.Text = "Inserir";
            this.btInserir.UseVisualStyleBackColor = true;
            this.btInserir.Click += new System.EventHandler(this.btInserir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(6, 366);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "*Somente números são válidos";
            // 
            // btCancelar
            // 
            this.btCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancelar.Location = new System.Drawing.Point(124, 265);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(110, 40);
            this.btCancelar.TabIndex = 7;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Enabled = false;
            this.txtResultado.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResultado.Location = new System.Drawing.Point(210, 90);
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtResultado.Size = new System.Drawing.Size(139, 32);
            this.txtResultado.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(211, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 22);
            this.label3.TabIndex = 9;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(101, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 22);
            this.label4.TabIndex = 10;
            this.label4.Text = "Digite aqui;";
            // 
            // txtInsercao
            // 
            this.txtInsercao.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInsercao.Location = new System.Drawing.Point(228, 43);
            this.txtInsercao.MaxLength = 9;
            this.txtInsercao.Name = "txtInsercao";
            this.txtInsercao.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtInsercao.Size = new System.Drawing.Size(120, 32);
            this.txtInsercao.TabIndex = 0;
            this.txtInsercao.TextChanged += new System.EventHandler(this.txtInsercao_TextChanged);
            this.txtInsercao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInsercao_KeyPress);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(55, 308);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(474, 22);
            this.label5.TabIndex = 11;
            this.label5.Text = "A inserção é ativada somente para valores válidos";
            // 
            // InsereValores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(580, 397);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btInserir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPorExtenso);
            this.Controls.Add(this.txtInsercao);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InsereValores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   Insere Valores";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPorExtenso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btInserir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInsercao;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
    }
}