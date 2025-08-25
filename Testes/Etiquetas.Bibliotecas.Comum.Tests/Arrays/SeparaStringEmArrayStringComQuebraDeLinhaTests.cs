using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class SeparaStringEmArrayStringComQuebraDeLinhaTests
    {
        private const string TextoWindows = "linha1\r\nlinha2\r\n\r\nlinha4";
        private const string TextoUnix = "linha1\nlinha2\n\nlinha4";
        private const string TextoMisto = "linha1\r\nlinha2\n\nlinha4";

        // Testes para SemMarcadorFinalLinhaUnixWindows
        [Fact]
        public void SemMarcador_ComFiltro_DeveSepararERemoverVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.SemMarcadorFinalLinhaUnixWindows(TextoMisto, true);
            Assert.Equal(new[] { "linha1", "linha2", "linha4" }, resultado);
        }

        [Fact]
        public void SemMarcador_SemFiltro_DeveSepararEManterVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.SemMarcadorFinalLinhaUnixWindows(TextoMisto, false);
            Assert.Equal(new[] { "linha1", "linha2", "", "linha4" }, resultado);
        }

        // Testes para ComMarcadorFinalLinhaUnixWindows
        [Fact]
        public void ComMarcadorUnixWindows_ComFiltro_DeveManterMarcadoresERemoverVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaUnixWindows(TextoMisto, true);
            Assert.Equal(new[] { "linha1\r\n", "linha2\n", "linha4" }, resultado);
        }

        [Fact]
        public void ComMarcadorUnixWindows_SemFiltro_DeveManterMarcadoresELinhasVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaUnixWindows(TextoMisto, false);
            Assert.Equal(new[] { "linha1\r\n", "linha2\n", "\n", "linha4" }, resultado);
        }

        // Testes para ComMarcadorFinalLinhaWindows
        [Fact]
        public void ComMarcadorWindows_ComFiltro_DeveNormalizarParaWindowsERemoverVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaWindows(TextoMisto, true);
            Assert.Equal(new[] { "linha1\r\n", "linha2\r\n", "linha4" }, resultado);
        }

        [Fact]
        public void ComMarcadorWindows_SemFiltro_DeveNormalizarParaWindowsEManterVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaWindows(TextoMisto, false);
            Assert.Equal(new[] { "linha1\r\n", "linha2\r\n", "\r\n", "linha4" }, resultado);
        }

        // Testes para ComMarcadorFinalLinhaUnix
        [Fact]
        public void ComMarcadorUnix_ComFiltro_DeveNormalizarParaUnixERemoverVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaUnix(TextoMisto, true);
            Assert.Equal(new[] { "linha1\n", "linha2\n", "linha4" }, resultado);
        }

        [Fact]
        public void ComMarcadorUnix_SemFiltro_DeveNormalizarParaUnixEManterVazias()
        {
            var resultado = SeparaStringEmArrayStringComQuebraDeLinha.ComMarcadorFinalLinhaUnix(TextoMisto, false);
            Assert.Equal(new[] { "linha1\n", "linha2\n", "\n", "linha4" }, resultado);
        }
    }
}
