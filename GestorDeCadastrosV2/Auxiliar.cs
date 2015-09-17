using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;

namespace GestorDeCadastros
{
    class Auxiliar
    {
        public static SqlCeConnection retornaConexao()
        {
            SqlCeConnection conexao = new SqlCeConnection("Data Source="
               + System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CadastrosAplicacao.sdf"));

            string path = System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "CadastrosAplicacao.sdf");

            return conexao;
        }

        public static bool validaCampoTxt(TextBox txtCampo, ErrorProvider errorProvider)
        {
            bool campoValido = false;

            if (!string.IsNullOrEmpty(txtCampo.Text))
            {
                campoValido = true;
                errorProvider.SetError(txtCampo, string.Empty);
            }
            else
            {
                campoValido = false;
                errorProvider.SetError(txtCampo, "O campo deve ser preenchido");
            }

            return campoValido;
        }

        public static bool validaCampoMtxt(MaskedTextBox MtxtCampo, ErrorProvider erroProvider)
        {
            bool campoValido = false;
            string valorCampo = MtxtCampo.Text.Trim();

            if (MtxtCampo.Text.Contains(',') || MtxtCampo.Text.Contains('.') || MtxtCampo.Text.Contains('-'))
            {
                valorCampo = valorCampo.Replace(",", "").Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim();
            }

            if (!string.IsNullOrEmpty(valorCampo))
            {
                campoValido = true;
                erroProvider.SetError(MtxtCampo, string.Empty);
            }
            else
            {
                campoValido = false;
                erroProvider.SetError(MtxtCampo, "O campo deve ser preenchido");
            }

            return campoValido;
        }

        public static bool verificaPrenchimentoMtxt(MaskedTextBox maskedTxt, ErrorProvider errorProvider)
        {
            if (maskedTxt.MaskFull)
            {
                return true;
            }
            else
            {
                errorProvider.SetError(maskedTxt, "O campo deve ser preenchido por completo");
                return false;
            }
        }

        public static string VerificaMtxtVazio(MaskedTextBox maskedTxt)
        {
            string rawCampo = maskedTxt.Text.Trim().Replace(".", string.Empty).Replace(",", string.Empty).
                              Replace("-", string.Empty).Replace("/", string.Empty);

            if (string.IsNullOrEmpty(rawCampo))
            {
                return rawCampo;
            }
            else
            {
                return maskedTxt.Text.Trim();
            }
        }

        /// <summary>
        /// tipoValor 0 = Valor Total
        /// tipoValor 1 = Aluguel
        /// tipoValor 2 = Outros Campos
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tipoValor"></param>
        /// <returns></returns>
        public static string FormataValorArmazenado(string valor, int tipoValor)
        {
            string valorFinal = string.Empty;
            if (!string.IsNullOrEmpty(valor) && valor.Contains(','))
            {
                 valor = InsereZeroAposVirgula(valor);
            }
           
            valorFinal = valor.Replace(".", string.Empty).Replace(",", string.Empty).Trim();

            if (tipoValor == 0)
            {
                if (valorFinal.Length >= 5)
                {
                    valorFinal = "00." + valor.Trim();
                }
                else if (valorFinal.Length == 4)
                {
                    valorFinal = "00.0" + valor.Trim();
                }
                else if (valorFinal.Length == 3)
                {
                    valorFinal = "00.00" + valor.Trim();
                }
                else if (valorFinal.Length == 2)
                {
                    valorFinal = "00.000" + valor.Trim();
                }
                else
                {
                    valorFinal = valor.Trim();
                }
            }
            else if (tipoValor == 1)
            {
                if (valorFinal.Length == 5)
                {
                    valorFinal = "0." + valor.Trim();
                }
                else if (valorFinal.Length == 4)
                {
                    valorFinal = "0.0" + valor.Trim();
                }
                else if (valorFinal.Length == 3)
                {
                    valorFinal = "0.00" + valor.Trim();
                }
                else if (valorFinal.Length == 2)
                {
                    valorFinal = "0.000" + valor.Trim();
                }
                else
                {
                    valorFinal = valor.Trim();
                }
            }
            else if (tipoValor == 2)
            {
                if (valorFinal.Length == 4)
                {
                    valorFinal = "0" + valor.Trim();
                }
                else if (valorFinal.Length == 3)
                {
                    valorFinal = "00" + valor.Trim();
                }
                else if (valorFinal.Length == 2)
                {
                    valorFinal = "000" + valor.Trim();
                }
                else if (valorFinal.Length == 1)
                {
                    valorFinal = "000,0" + valor.Trim();
                }
                else
                {
                    valorFinal = valor.Trim();
                }
            }

            return valorFinal;
        }

        private static string InsereZeroAposVirgula(string valorVirgula)
        {
            string[] aValor = valorVirgula.Split(',');

            if (aValor[1].Length == 1)
            {
                valorVirgula = valorVirgula + "0";
            }

            return valorVirgula;          
        }

        /// <summary>
        /// tipoValor 1 = Campo Aluguel, Campo Valor Total
        /// tipoValor 2 = Outros Campos
        /// </summary>
        /// <param name="maskedTxt"></param>
        /// <param name="tipoValor"></param>
        /// <returns></returns>
        public static Decimal FormataValorSalvar(MaskedTextBox maskedTxt, int tipoValor)
        {
            decimal dValor = 0;
            string valorFinal = string.Empty;
            valorFinal = maskedTxt.Text.Replace(".", string.Empty).Replace(",", string.Empty).Trim();

            if (!string.IsNullOrEmpty(valorFinal))
            {
                if (tipoValor == 1)
                {
                    valorFinal = maskedTxt.Text.Replace(".", string.Empty).Trim();
                }
                else
                {
                    valorFinal = maskedTxt.Text.Trim();
                }

                dValor = Convert.ToDecimal(valorFinal);
            }

            return dValor;
        }

        /// <summary>
        /// tipoValor 1 = Campo Aluguel, Campo Valor Total
        /// tipoValor 2 = Outros Campos
        /// </summary>
        /// <param name="maskedTxt"></param>
        /// <param name="tipoValor"></param>
        /// <returns></returns>
        public static Decimal FormataValorSalvar(TextBox maskedTxt, int tipoValor)
        {
            decimal dValor = 0;
            string valorFinal = string.Empty;
            valorFinal = maskedTxt.Text.Replace(".", string.Empty).Replace(",", string.Empty).Trim();

            if (!string.IsNullOrEmpty(valorFinal))
            {
                if (tipoValor == 1)
                {
                    valorFinal = maskedTxt.Text.Replace(".", string.Empty).Trim();
                }
                else
                {
                    valorFinal = maskedTxt.Text.Trim();
                }

                dValor = Convert.ToDecimal(valorFinal);
            }

            return dValor;
        }

        #region Retorna Valor Por Extenso

        // O método valorPorExtenso recebe um valor do tipo decimal
        public static string valorPorExtenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                    valor_por_extenso += " DE";
                                else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                        valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }

        #endregion
    }
}
