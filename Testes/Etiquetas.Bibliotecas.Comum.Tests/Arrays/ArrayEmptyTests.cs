using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayEmptyTests
    {
        [Fact]
        public void Executa_RetornaArrayVazio()
        {
            // Arrange & Act
            var result = ArrayEmpty.Executa<int>();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Executa_ParaOMesmoTipo_RetornaAMesmaInstancia()
        {
            // Arrange & Act
            var result1 = ArrayEmpty.Executa<string>();
            var result2 = ArrayEmpty.Executa<string>();

            // Assert
            Assert.Same(result1, result2);
        }

        [Fact]
        public void Executa_ParaTiposDiferentes_RetornaInstanciasDiferentes()
        {
            // Arrange & Act
            var resultInt = ArrayEmpty.Executa<int>();
            var resultString = ArrayEmpty.Executa<string>();

            // Assert
            Assert.NotSame(resultInt, resultString);
        }
    }
}
