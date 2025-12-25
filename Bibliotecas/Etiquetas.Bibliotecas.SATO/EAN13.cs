using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Classe responsável por calcular e validar códigos EAN-13 (European Article Number).
    /// O EAN-13 é um padrão de código de barras de 13 dígitos usado internacionalmente.
    /// </summary>
    public static class EAN13
    {
        /// <summary>
        /// Calcula o dígito verificador de um código EAN-13 a partir dos primeiros 12 dígitos.
        /// </summary>
        /// <param name="ean12">String contendo exatamente 12 dígitos numéricos</param>
        /// <returns>Retorna o dígito verificador calculado (0-9) inteiro</returns>
        /// <exception cref="ArgumentException">Lançada quando o código não possui exatamente 12 dígitos</exception>
        /// <exception cref="FormatException">Lançada quando o código contém caracteres não numéricos</exception>
        /// <example>
        /// <code>
        /// int digito = EAN13.CalcularDigitoVerificador("200002348517");
        /// // Retorna: 2
        /// </code>
        /// </example>
        public static int CalcularDigitoVerificador(string ean12)
        {
            if (string.IsNullOrEmpty(ean12) || ean12.Length != 12)
                throw new ArgumentException("O código deve ter exatamente 12 dígitos", nameof(ean12));

            int somaImpar = 0;
            int somaPar = 0;

            for (int i = 0; i < 12; i++)
            {
                if (!char.IsDigit(ean12[i]))
                    throw new FormatException($"Caractere inválido na posição {i + 1}: '{ean12[i]}'");

                int digito = int.Parse(ean12[i].ToString());

                // Posições ímpares (índice par, pois começa em 0)
                if (i % 2 == 0)
                    somaImpar += digito;
                else
                    somaPar += digito;
            }

            int somaTotal = somaImpar + (somaPar * 3);
            int digitoVerificador = (10 - (somaTotal % 10)) % 10;

            return digitoVerificador;
        }

        /// <summary>
        /// Valida se um código EAN-13 completo (13 dígitos) está correto.
        /// Verifica se o último dígito corresponde ao dígito verificador calculado.
        /// </summary>
        /// <param name="ean13">String contendo exatamente 13 dígitos numéricos</param>
        /// <returns>True se o código for válido, False caso contrário</returns>
        /// <example>
        /// <code>
        /// bool valido = EAN13.ValidarEAN13("2000023485172");
        /// // Retorna: true
        /// </code>
        /// </example>
        public static bool ValidarEAN13(string ean13)
        {
            if (string.IsNullOrEmpty(ean13) || ean13.Length != 13)
                return false;

            if (!ean13.All(char.IsDigit))
                return false;

            try
            {
                string ean12 = ean13.Substring(0, 12);
                int digitoCalculado = CalcularDigitoVerificador(ean12);
                int digitoInformado = int.Parse(ean13[12].ToString());

                return digitoCalculado == digitoInformado;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gera um código EAN-13 completo a partir dos primeiros 12 dígitos.
        /// Calcula automaticamente o dígito verificador e retorna o código completo.
        /// </summary>
        /// <param name="ean12">String contendo exatamente 12 dígitos numéricos</param>
        /// <returns>String com o código EAN-13 completo (13 dígitos)</returns>
        /// <exception cref="ArgumentException">Lançada quando o código não possui exatamente 12 dígitos</exception>
        /// <example>
        /// <code>
        /// string codigoCompleto = EAN13.GerarEAN13("200002348517");
        /// // Retorna: "2000023485172"
        /// </code>
        /// </example>
        public static string GerarEAN13(string ean12)
        {
            int digitoVerificador = CalcularDigitoVerificador(ean12);
            return ean12 + digitoVerificador.ToString();
        }

        /// <summary>
        /// Formata um código EAN-13 para exibição visual com separadores.
        /// </summary>
        /// <param name="ean13">String contendo exatamente 13 dígitos numéricos</param>
        /// <returns>String formatada no padrão: X-XXXXXX-XXXXXX-X</returns>
        /// <exception cref="ArgumentException">Lançada quando o código não possui exatamente 13 dígitos</exception>
        /// <example>
        /// <code>
        /// string formatado = EAN13.FormatarEAN13("2000023485172");
        /// // Retorna: "2-000023-485172"
        /// </code>
        /// </example>
        public static string FormatarEAN13(string ean13)
        {
            if (string.IsNullOrEmpty(ean13) || ean13.Length != 13)
                throw new ArgumentException("O código deve ter exatamente 13 dígitos", nameof(ean13));

            return $"{ean13[0]}-{ean13.Substring(1, 6)}-{ean13.Substring(7, 6)}";
        }
    }
}
