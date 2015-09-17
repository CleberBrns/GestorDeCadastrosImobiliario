using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GestorDeCadastros
{
    public partial class InsereValores : Form
    {
        private string valorInserido;

        public string getValorInserido
        {
            get { return valorInserido; }
            //set { valorInserido = value; }
        }

        public InsereValores()
        {
            InitializeComponent();
            txtInsercao.Focus();
        }

        private void btInserir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInsercao.Text.Trim()))
            {             
                MessageBox.Show("O valor digitado não será inserido!");
            }
            else
            {
                if (Convert.ToDecimal(txtInsercao.Text.Trim()) == 0)
                {
                    MessageBox.Show("O valor digitado não será inserido!");
                }

                valorInserido = txtResultado.Text.Trim();
            }

        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtInsercao_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Somente números
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void txtInsercao_TextChanged(object sender, EventArgs e)
        {
            txtResultado.Text = formataTexto(txtInsercao.Text);

            if (!string.IsNullOrEmpty(txtResultado.Text))
            {
                decimal valorConvercao = Convert.ToDecimal(txtResultado.Text.Replace(".", string.Empty).Trim());

                if (valorConvercao != 0)
                {
                    txtPorExtenso.Text = Auxiliar.valorPorExtenso(valorConvercao);
                    btInserir.Enabled = true;
                }
                else
                {
                    btInserir.Enabled = false;
                }
            }
            else
            {
                txtPorExtenso.Text = string.Empty;
                btInserir.Enabled = false;
            }


            if (txtInsercao.Text.Length == 9)
            {
                errorProvider1.SetError(txtInsercao, "Quantidade máxima de dígitos atingida!");
            }
            else
            {
                errorProvider1.Clear();
            }

        }

        #region Formata Valores Inseridos

        private string _workingText;
        private char _thousandsSeparator = '.', _decimalsSeparator = ',';
        private int _decimalPlaces = 2;

        /// <summary>
        /// Contains the entered text without mask.
        /// </summary>
        public string WorkingText
        {
            get { return _workingText; }
            private set { _workingText = value; }
        }

        /// <summary>
        /// Formats the entered text.
        /// </summary>
        /// <returns></returns>
        public string formataTexto(string textoFormatar)
        {
            this.WorkingText = textoFormatar.Replace((_thousandsSeparator.ToString() != "") ? _thousandsSeparator.ToString() : " ", String.Empty)
                                        .Replace((_decimalsSeparator.ToString() != "") ? _decimalsSeparator.ToString() : " ", String.Empty).Trim();
            int counter = 1;
            int counter2 = 0;
            char[] charArray = this.WorkingText.ToCharArray();
            StringBuilder str = new StringBuilder();

            for (int i = charArray.Length - 1; i >= 0; i--)
            {
                str.Insert(0, charArray.GetValue(i));
                if (_decimalPlaces == 0 && counter == 3)
                {
                    counter2 = counter;
                }

                if (counter == _decimalPlaces && i > 0)
                {
                    if (_decimalsSeparator != Char.MinValue)
                        str.Insert(0, _decimalsSeparator);
                    counter2 = counter + 3;
                }
                else if (counter == counter2 && i > 0)
                {
                    if (_thousandsSeparator != Char.MinValue)
                        str.Insert(0, _thousandsSeparator);
                    counter2 = counter + 3;
                }
                counter = ++counter;
            }

            string retorno = (str.ToString() != "") ? str.ToString() : (str.ToString() != "") ? str.ToString() : "";
            return retorno;
        }

        #endregion

    }
}
