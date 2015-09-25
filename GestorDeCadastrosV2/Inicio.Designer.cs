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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCadastros
            // 
            this.btCadastros.BackColor = System.Drawing.Color.RoyalBlue;
            this.btCadastros.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btCadastros.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCadastros.Location = new System.Drawing.Point(69, 63);
            this.btCadastros.Name = "btCadastros";
            this.btCadastros.Size = new System.Drawing.Size(145, 50);
            this.btCadastros.TabIndex = 0;
            this.btCadastros.Text = "Cadastros";
            this.btCadastros.UseVisualStyleBackColor = false;
            this.btCadastros.Click += new System.EventHandler(this.btCadastros_Click);
            // 
            // btConsultas
            // 
            this.btConsultas.BackColor = System.Drawing.Color.RoyalBlue;
            this.btConsultas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btConsultas.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConsultas.Location = new System.Drawing.Point(319, 63);
            this.btConsultas.Name = "btConsultas";
            this.btConsultas.Size = new System.Drawing.Size(145, 50);
            this.btConsultas.TabIndex = 1;
            this.btConsultas.Text = "Consultas";
            this.btConsultas.UseVisualStyleBackColor = false;
            this.btConsultas.Click += new System.EventHandler(this.btConsultas_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btCadastros);
            this.panel1.Controls.Add(this.btConsultas);
            this.panel1.Location = new System.Drawing.Point(411, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 320);
            this.panel1.TabIndex = 2;
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1360, 739);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Inicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCadastros;
        private System.Windows.Forms.Button btConsultas;
        private System.Windows.Forms.Panel panel1;
    }
}