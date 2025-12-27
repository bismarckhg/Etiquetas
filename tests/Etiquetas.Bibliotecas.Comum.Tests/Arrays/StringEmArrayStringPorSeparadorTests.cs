using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringEmArrayStringPorSeparadorTests
    {
        // Testes para CriaDicionario(string texto, string separador, ...)
        [Fact]
        public void Execute_ComSeparadorStringSimples_DeveSepararCorretamente()
        {
            var texto = "a,b,c";
            var expected = new[] { "a", "b", "c" };
            var result = StringEmArrayStringPorSeparador.Execute(texto, ",");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSeparadorStringMultiChar_DeveSepararPorCadaChar()
        {
            var texto = "a;;b,c";
            var expected = new[] { "a", "", "b", "c" }; // Separa por ';' e por ','
            var result = StringEmArrayStringPorSeparador.Execute(texto, ";,");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSeparadorString_E_RemoveEmptyEntriesTrue()
        {
            var texto = "a;;b";
            var expected = new[] { "a", "b" };
            var result = StringEmArrayStringPorSeparador.Execute(texto, ";", true);
            Assert.Equal(expected, result);
        }

        // Testes para CriaDicionario(string texto, string[] separadores, ...)
        [Fact]
        public void Execute_ComSeparadorArray_DeveSepararCorretamente()
        {
            var texto = "a--b##c";
            var separadores = new[] { "--", "##" };
            var expected = new[] { "a", "b", "c" };
            var result = StringEmArrayStringPorSeparador.Execute(texto, separadores);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSeparadorArray_E_RemoveEmptyEntriesTrue()
        {
            var texto = "a--b----c";
            var separadores = new[] { "--" };
            var expected = new[] { "a", "b", "c" };
            var result = StringEmArrayStringPorSeparador.Execute(texto, separadores, true);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSeparadorArray_E_RemoveEmptyEntriesFalse()
        {
            var texto = "a--b----c";
            var separadores = new[] { "--" };
            var expected = new[] { "a", "b", "", "c" };
            var result = StringEmArrayStringPorSeparador.Execute(texto, separadores, false);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_DeveLancarExcecao()
        {
            Assert.Throws<System.NullReferenceException>(() => StringEmArrayStringPorSeparador.Execute(null, ","));
        }
    }
}
