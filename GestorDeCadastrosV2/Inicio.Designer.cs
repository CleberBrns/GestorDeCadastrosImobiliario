namespace GestorDeCadastros
{
    partial class Inicio
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
            this.btCadastros = new System.Windows.Forms.Button();
            this.btConsultas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btCadastros
            // 
            this.btCadastros.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCadastros.Location = new System.Drawing.Point(97, 86);
            this.btCadastros.Name = "btCadastros";
            this.btCadastros.Size = new System.Drawing.Size(120, 42);
            this.btCadastros.TabIndex = 0;
            this.btCadastros.Text = "Cadastros";
            this.btCadastros.UseVisualStyleBackColor = true;
            this.btCadastros.Click += new System.EventHandler(this.btCadastros_Click);
            // 
            // btConsultas
            // 
            this.btConsultas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConsultas.Location = new System.Drawing.Point(308, 86);
            this.btConsultas.Name = "btConsultas";
            this.btConsultas.Size = new System.Drawing.Size(117, 42);
            this.btConsultas.TabIndex = 1;
            this.btConsultas.Text = "Consultas";
            this.btConsultas.UseVisualStyleBackColor = true;
            this.btConsultas.Click += new System.EventHandler(this.btConsultas_Click);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btConsultas);
            this.Controls.Add(this.btCadastros);
            this.Name = "Inicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCadastros;
        private System.Windows.Forms.Button btConsultas;
    }
}